using System;
using System.Collections.Generic;

namespace XelaTechSoporte;

class Solicitud
{
    private static string[] nombresEstados = new string[] { "Recibida", "En espera", "Asignada", "En diagnóstico", "En reparación", "Lista para entregar", "Entregada", "Cancelada" };

    private string numeroSolicitud;
    private Cliente cliente;
    private Equipo equipo;
    private Tecnico tecnicoAsignado;
    private int estado;
    private int prioridad;
    private string motivoIngreso;
    private string diagnostico;
    private string fechaIngreso;
    private int tiempoEstimado;
    private double costoManoObra;
    private double costoRepuestos;
    private double total;
    private int calificacionServicio;
    private List<UsoRepuesto> repuestosUtilizados;
    private Stack<int> historialEstados;
    public Solicitud(string numeroSolicitud, Cliente cliente, Equipo equipo, string motivoIngreso, int prioridad, string fechaIngreso)
    {
        if (string.IsNullOrWhiteSpace(numeroSolicitud))
            throw new ArgumentException("El código de la solicitud no puede estar vacío.");
        if (cliente == null)
            throw new ArgumentException("La solicitud necesita un cliente.");
        if (equipo == null)
            throw new ArgumentException("La solicitud necesita un equipo.");
        if (!equipo.Propietario.Codigo.Equals(cliente.Codigo, StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("El equipo debe pertenecer al cliente de la solicitud.");
        if (string.IsNullOrWhiteSpace(motivoIngreso))
            throw new ArgumentException("La descripción del problema no puede estar vacía.");
        if (prioridad < 1 || prioridad > 2)
            throw new ArgumentException("La prioridad debe ser 1 (Normal) o 2 (Urgente).");
        if (string.IsNullOrWhiteSpace(fechaIngreso))
            throw new ArgumentException("La fecha de recepción no puede estar vacía.");

        this.numeroSolicitud = numeroSolicitud.Trim();
        this.cliente = cliente;
        this.equipo = equipo;
        this.motivoIngreso = motivoIngreso.Trim();
        this.prioridad = prioridad;
        this.fechaIngreso = fechaIngreso.Trim();
        this.estado = 1;
        this.diagnostico = "Pendiente de diagnóstico";
        this.tiempoEstimado = equipo.ObtenerTiempoEstimado();
        this.costoManoObra = 0.0;
        this.costoRepuestos = 0.0;
        this.total = 0.0;
        this.calificacionServicio = 0;
        this.repuestosUtilizados = new List<UsoRepuesto>();
        this.historialEstados = new Stack<int>();
    }

    public Solicitud(string numeroSolicitud, Cliente cliente, Equipo equipo, string motivoIngreso)
        : this(numeroSolicitud, cliente, equipo, motivoIngreso, 1, "Sin fecha registrada")
    {
    }

    public string NumeroSolicitud { get { return numeroSolicitud; } }
    public string Codigo { get { return numeroSolicitud; } }
    public Cliente Cliente { get { return cliente; } }
    public Equipo Equipo { get { return equipo; } }
    public Tecnico TecnicoAsignado { get { return tecnicoAsignado; } }
    public int Estado { get { return estado; } }
    public int Prioridad { get { return prioridad; } }
    public string MotivoIngreso { get { return motivoIngreso; } }
    public string Diagnostico { get { return diagnostico; } }
    public string FechaIngreso { get { return fechaIngreso; } }
    public int TiempoEstimado { get { return tiempoEstimado; } }
    public double CostoManoObra { get { return costoManoObra; } }
    public double CostoRepuestos { get { return costoRepuestos; } }
    public double Total { get { return total; } }
    public int CalificacionServicio { get { return calificacionServicio; } }
    public List<UsoRepuesto> RepuestosUtilizados { get { return repuestosUtilizados; } }

    public static string NombreEstado(int numero)
    {
        if (numero < 1 || numero > nombresEstados.Length) return "Estado desconocido";
        return nombresEstados[numero - 1];
    }

    public bool EstaActiva()
    {
        return estado != 7 && estado != 8;
    }
    public bool TieneDiagnostico()
    {
        return diagnostico != "Pendiente de diagnóstico";
    }
    public bool CambiarEstado(int nuevoEstado)
    {
        if (estado == 7 || estado == 8) return false;
        if (nuevoEstado < 1 || nuevoEstado > 8 || nuevoEstado == estado) return false;

        if (nuevoEstado == 8)
        {
            historialEstados.Push(estado);
            estado = nuevoEstado;
            return true;
        }

        if (nuevoEstado != estado + 1) return false;
        if (nuevoEstado == 3 && tecnicoAsignado == null) return false;
        if (nuevoEstado == 4 && tecnicoAsignado == null) return false;
        if (nuevoEstado == 5 && !TieneDiagnostico()) return false;
        if (nuevoEstado == 6 && (estado != 5 || costoManoObra <= 0)) return false;
        if (nuevoEstado == 7 && (estado != 6 || CalcularTotal() <= 0)) return false;

        historialEstados.Push(estado);
        estado = nuevoEstado;
        return true;
    }

    public bool CambiarEstado(int nuevoEstado, string comentario)
    {
        return CambiarEstado(nuevoEstado);
    }

    public bool DeshacerEstado()
    {

        if (estado == 7 || estado == 8 || historialEstados.Count == 0) return false;
        int estadoAnterior = historialEstados.Pop();
        estado = estadoAnterior;
        return true;
    }

    public void ConsultarHistorial()
    {
        if (historialEstados.Count == 0)
        {
            Console.WriteLine("Historial vacío.");
            return;
        }
        Console.WriteLine("Historial de estados (del más reciente al más antiguo):");
        foreach (int e in historialEstados)
        {
            Console.WriteLine("   " + e + " - " + NombreEstado(e));
        }
    }

    public void AsignarTecnico(Tecnico tecnico)
    {
        if (tecnico == null)
            throw new ArgumentException("El técnico no puede ser nulo.");
        if (tecnico.Especialidad != equipo.ObtenerEspecialidadRequerida())
            throw new ArgumentException("La especialidad del técnico no coincide con el equipo.");
        tecnicoAsignado = tecnico;
    }

    public bool RegistrarDiagnostico(string texto)
    {
        if (estado != 4 || string.IsNullOrWhiteSpace(texto)) return false;
        diagnostico = texto.Trim();
        return true;
    }

    public void AgregarRepuesto(UsoRepuesto uso)
    {
        if (uso != null)
        {
            repuestosUtilizados.Add(uso);
            costoRepuestos = CalcularCostoRepuestos();
        }
    }

    public bool AgregarRepuesto(Repuesto repuesto, int cantidad)
    {
        if (estado != 5) return false;
        if (repuesto == null) return false;
        if (!repuesto.HayExistencia(cantidad)) return false;
        repuesto.Descontar(cantidad);
        AgregarRepuesto(new UsoRepuesto(repuesto, cantidad));
        return true;
    }

    public double CalcularCostoRepuestos()
    {
        double suma = 0.0;
        for (int i = 0; i < repuestosUtilizados.Count; i++)
        {
            suma += repuestosUtilizados[i].CalcularSubtotal();
        }
        return suma;
    }

    public bool RegistrarManoObra(double costoAdicional)
    {
        if (estado != 5 || costoAdicional < 0) return false;
        costoManoObra = equipo.CalcularCostoRevision() + costoAdicional;
        CalcularTotal();
        return true;
    }

    public double CalcularTotal()
    {
        costoRepuestos = CalcularCostoRepuestos();
        total = costoManoObra + costoRepuestos;
        return total;
    }

    public bool RegistrarCalificacion(int valor)
    {
        if (estado != 7) return false;
        if (valor < 1 || valor > 5) return false;
        if (calificacionServicio != 0) return false;
        calificacionServicio = valor;
        return true;
    }

    public bool Cancelar()
    {
        return CambiarEstado(8);
    }


    // Restaura los datos principales de una solicitud cargada desde el archivo TXT.
    public void RestaurarDatosPersistidos(int estadoGuardado, string diagnosticoGuardado, double manoObraGuardada, double repuestosGuardados, double totalGuardado, int calificacionGuardada)
    {
        if (estadoGuardado < 1 || estadoGuardado > 8) estadoGuardado = 1;
        estado = estadoGuardado;
        diagnostico = string.IsNullOrWhiteSpace(diagnosticoGuardado) ? "Pendiente de diagnóstico" : diagnosticoGuardado;
        costoManoObra = manoObraGuardada < 0 ? 0 : manoObraGuardada;
        costoRepuestos = repuestosGuardados < 0 ? 0 : repuestosGuardados;
        total = totalGuardado < 0 ? 0 : totalGuardado;
        calificacionServicio = calificacionGuardada >= 1 && calificacionGuardada <= 5 ? calificacionGuardada : 0;
    }

    public void RestaurarTecnicoPersistido(Tecnico tecnico)
    {
        if (tecnico != null && tecnico.Especialidad == equipo.ObtenerEspecialidadRequerida())
            tecnicoAsignado = tecnico;
    }

    public void MostrarInformacion()
    {
        string nomTec = tecnicoAsignado != null ? tecnicoAsignado.NombreCompleto : "Sin asignar";
        string textoPrioridad = prioridad == 2 ? "Urgente" : "Normal";

        Console.WriteLine("Solicitud: " + numeroSolicitud +
                          " | Cliente: " + cliente.NombreCompleto +
                          " | Equipo: " + equipo.CodigoInterno +
                          " | " + textoPrioridad +
                          " | Técnico: " + nomTec +
                          " | Estado: " + estado + " " + NombreEstado(estado) +
                          " | Tiempo: " + tiempoEstimado + "h" +
                          " | Total: Q" + total.ToString("F2") +
                          " | Motivo: " + motivoIngreso);
    }

    public void MostrarDetalle()
    {
        Console.WriteLine("Detalle de la solicitud " + numeroSolicitud + ":");
        Console.WriteLine("   Mano de obra: Q" + costoManoObra.ToString("F2"));
        for (int i = 0; i < repuestosUtilizados.Count; i++)
        {
            repuestosUtilizados[i].MostrarInformacion();
        }
        Console.WriteLine("   Repuestos: Q" + costoRepuestos.ToString("F2"));
        Console.WriteLine("   TOTAL: Q" + total.ToString("F2"));
    }
}