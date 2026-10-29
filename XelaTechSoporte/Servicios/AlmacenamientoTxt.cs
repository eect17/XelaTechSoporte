using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace XelaTechSoporte;
class AlmacenamientoTxt
{
    private static readonly string rutaArchivo = Path.Combine(AppContext.BaseDirectory, "XelaTechDatos.txt");
    private static readonly JsonSerializerOptions opcionesJson = new JsonSerializerOptions { WriteIndented = true };

    public static void Guardar(CentroServicio centro)
    {
        DatosGuardados datos = new DatosGuardados();

        foreach (Cliente c in centro.Clientes)
        {
            datos.Clientes.Add(new ClienteGuardado
            {
                Codigo = c.Codigo,
                Nombre = c.NombreCompleto,
                Telefono = c.Telefono,
                Correo = c.Correo,
                Tipo = c.TipoCliente
            });
        }

        foreach (Tecnico t in centro.Tecnicos)
        {
            datos.Tecnicos.Add(new TecnicoGuardado
            {
                Codigo = t.Codigo,
                Nombre = t.NombreCompleto,
                Especialidad = t.Especialidad,
                Disponibilidad = t.Disponibilidad
            });
        }

        foreach (Equipo e in centro.Equipos)
        {
            EquipoGuardado equipo = new EquipoGuardado
            {
                Codigo = e.CodigoInterno,
                Serie = e.NumeroSerie,
                Marca = e.Marca,
                Modelo = e.Modelo,
                CodigoCliente = e.Propietario.Codigo,
                Tipo = e.TipoEquipo,
                EstadoFisico = e.EstadoFisico,
                Accesorios = e.Accesorios
            };

            if (e is ComputadoraPortatil portatil) equipo.Caracteristica = portatil.TieneCargador;
            else if (e is ComputadoraEscritorio escritorio) equipo.Caracteristica = escritorio.TieneMonitor;
            else if (e is Impresora impresora) equipo.Caracteristica = impresora.EsMultifuncional;
            datos.Equipos.Add(equipo);
        }

        foreach (Repuesto r in centro.Repuestos)
        {
            datos.Repuestos.Add(new RepuestoGuardado
            {
                Codigo = r.Codigo,
                Nombre = r.Nombre,
                Categoria = r.Categoria,
                Existencia = r.Existencia,
                ExistenciaMinima = r.ExistenciaMinima,
                CostoUnitario = r.CostoUnitario,
                PrecioVenta = r.PrecioVenta
            });
        }

        foreach (Solicitud s in centro.Solicitudes)
        {
            datos.Solicitudes.Add(new SolicitudGuardada
            {
                Codigo = s.NumeroSolicitud,
                CodigoCliente = s.Cliente.Codigo,
                CodigoEquipo = s.Equipo.CodigoInterno,
                CodigoTecnico = s.TecnicoAsignado == null ? "" : s.TecnicoAsignado.Codigo,
                Motivo = s.MotivoIngreso,
                Prioridad = s.Prioridad,
                Fecha = s.FechaIngreso,
                Estado = s.Estado,
                Diagnostico = s.Diagnostico,
                CostoManoObra = s.CostoManoObra,
                CostoRepuestos = s.CostoRepuestos,
                Total = s.Total,
                Calificacion = s.CalificacionServicio
            });
        }

        string contenido = JsonSerializer.Serialize(datos, opcionesJson);
        File.WriteAllText(rutaArchivo, contenido);
    }

    public static int Cargar(CentroServicio centro)
    {
        if (!File.Exists(rutaArchivo)) return 0;

        string contenido = File.ReadAllText(rutaArchivo);
        if (string.IsNullOrWhiteSpace(contenido)) return 0;
        DatosGuardados? datos = JsonSerializer.Deserialize<DatosGuardados>(contenido);
        if (datos == null) return 0;
        foreach (ClienteGuardado d in datos.Clientes)
        {
            try { centro.RegistrarCliente(new Cliente(d.Codigo, d.Nombre, d.Telefono, d.Correo, d.Tipo)); }
            catch {  }
        }

        foreach (TecnicoGuardado d in datos.Tecnicos)
        {
            try { centro.RegistrarTecnico(new Tecnico(d.Codigo, d.Nombre, d.Especialidad, d.Disponibilidad)); }
            catch { }
        }

        foreach (EquipoGuardado d in datos.Equipos)
        {
            Cliente? cliente = centro.Clientes.Find(c => c.Codigo.Equals(d.CodigoCliente, StringComparison.OrdinalIgnoreCase));
            if (cliente == null) continue;
            try
            {
                Equipo equipo;
                if (d.Tipo == 1) equipo = new ComputadoraPortatil(d.Codigo, d.Serie, d.Marca, d.Modelo, cliente, d.EstadoFisico, d.Accesorios, d.Caracteristica);
                else if (d.Tipo == 2) equipo = new ComputadoraEscritorio(d.Codigo, d.Serie, d.Marca, d.Modelo, cliente, d.EstadoFisico, d.Accesorios, d.Caracteristica);
                else equipo = new Impresora(d.Codigo, d.Serie, d.Marca, d.Modelo, cliente, d.EstadoFisico, d.Accesorios, d.Caracteristica);
                centro.RegistrarEquipo(equipo);
            }
            catch { }
        }

        foreach (RepuestoGuardado d in datos.Repuestos)
        {
            try { centro.RegistrarRepuesto(new Repuesto(d.Codigo, d.Nombre, d.Categoria, d.Existencia, d.ExistenciaMinima, d.CostoUnitario, d.PrecioVenta)); }
            catch { }
        }

        foreach (SolicitudGuardada d in datos.Solicitudes)
        {
            Cliente? cliente = centro.Clientes.Find(c => c.Codigo.Equals(d.CodigoCliente, StringComparison.OrdinalIgnoreCase));
            Equipo? equipo = centro.Equipos.Find(e => e.CodigoInterno.Equals(d.CodigoEquipo, StringComparison.OrdinalIgnoreCase));
            if (cliente == null || equipo == null) continue;

            try
            {
                Solicitud solicitud = new Solicitud(d.Codigo, cliente, equipo, d.Motivo, d.Prioridad, d.Fecha);
                Tecnico? tecnico = centro.Tecnicos.Find(t => t.Codigo.Equals(d.CodigoTecnico, StringComparison.OrdinalIgnoreCase));
                if (tecnico != null)
                {
                    solicitud.RestaurarTecnicoPersistido(tecnico);
                    if (d.Estado >= 3 && d.Estado <= 6) tecnico.AsignarTrabajo();
                    else if (d.Estado == 7) tecnico.FinalizarTrabajo();
                }
                solicitud.RestaurarDatosPersistidos(d.Estado, d.Diagnostico, d.CostoManoObra, d.CostoRepuestos, d.Total, d.Calificacion);
                if (centro.Solicitudes.Exists(x => x.NumeroSolicitud.Equals(solicitud.NumeroSolicitud, StringComparison.OrdinalIgnoreCase))) continue;
                centro.Solicitudes.Add(solicitud);
                cliente.IncrementarSolicitudes();
                if (solicitud.Estado == 1 || solicitud.Estado == 2)
                {
                    if (solicitud.Prioridad == 2) centro.ColaUrgentes.Enqueue(solicitud);
                    else centro.ColaNormales.Enqueue(solicitud);
                }
            }
            catch { }
        }

        return centro.Clientes.Count + centro.Tecnicos.Count + centro.Equipos.Count + centro.Repuestos.Count + centro.Solicitudes.Count;
    }

    public static string ObtenerRutaArchivo() { return rutaArchivo; }
}

class DatosGuardados
{
    public List<ClienteGuardado> Clientes { get; set; } = new List<ClienteGuardado>();
    public List<TecnicoGuardado> Tecnicos { get; set; } = new List<TecnicoGuardado>();
    public List<EquipoGuardado> Equipos { get; set; } = new List<EquipoGuardado>();
    public List<RepuestoGuardado> Repuestos { get; set; } = new List<RepuestoGuardado>();
    public List<SolicitudGuardada> Solicitudes { get; set; } = new List<SolicitudGuardada>();
}

class ClienteGuardado { public string Codigo { get; set; } = ""; public string Nombre { get; set; } = ""; public string Telefono { get; set; } = ""; public string Correo { get; set; } = ""; public string Tipo { get; set; } = ""; }
class TecnicoGuardado { public string Codigo { get; set; } = ""; public string Nombre { get; set; } = ""; public int Especialidad { get; set; } public int Disponibilidad { get; set; } }
class EquipoGuardado { public string Codigo { get; set; } = ""; public string Serie { get; set; } = ""; public string Marca { get; set; } = ""; public string Modelo { get; set; } = ""; public string CodigoCliente { get; set; } = ""; public int Tipo { get; set; } public string EstadoFisico { get; set; } = ""; public string Accesorios { get; set; } = ""; public bool Caracteristica { get; set; } }
class RepuestoGuardado { public string Codigo { get; set; } = ""; public string Nombre { get; set; } = ""; public string Categoria { get; set; } = ""; public int Existencia { get; set; } public int ExistenciaMinima { get; set; } public double CostoUnitario { get; set; } public double PrecioVenta { get; set; } }
class SolicitudGuardada { public string Codigo { get; set; } = ""; public string CodigoCliente { get; set; } = ""; public string CodigoEquipo { get; set; } = ""; public string CodigoTecnico { get; set; } = ""; public string Motivo { get; set; } = ""; public int Prioridad { get; set; } public string Fecha { get; set; } = ""; public int Estado { get; set; } public string Diagnostico { get; set; } = ""; public double CostoManoObra { get; set; } public double CostoRepuestos { get; set; } public double Total { get; set; } public int Calificacion { get; set; } }
