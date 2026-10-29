using System;
using System.Collections.Generic;

namespace XelaTechSoporte;

class Reportes
{
    private CentroServicio centro;
    private Ordenador ordenador;
    private Buscador buscador;

    public Reportes(CentroServicio centro)
    {
        this.centro = centro;
        this.ordenador = new Ordenador();
        this.buscador = new Buscador(centro);
    }


    public void Reporte1_SolicitudesEnEspera()
    {
        Console.WriteLine("\n--- 1. Solicitudes en Espera por Prioridad ---");
        Console.WriteLine("[Cola Urgentes]");
        foreach (var s in centro.ColaUrgentes)
        {
            if (s.Estado == 1 || s.Estado == 2) s.MostrarInformacion();
        }
        Console.WriteLine("[Cola Normales]");
        foreach (var s in centro.ColaNormales)
        {
            if (s.Estado == 1 || s.Estado == 2) s.MostrarInformacion();
        }
    }

    public void Reporte2_EstadoSolicitudes()
    {
        Console.WriteLine("\n--- 2. Solicitudes por Estado ---");
        foreach (var s in centro.Solicitudes)
        {
            string estadoTxt = s.Estado == 7 ? "Entregada" : (s.Estado == 8 ? "Cancelada" : "Activa");
            Console.WriteLine($"Solicitud: {s.Codigo} | Estado: {estadoTxt} (Código {s.Estado})");
        }
    }


    public void Reporte3_EstadoTecnicos()
    {
        Console.WriteLine("\n--- 3. Estado de Técnicos ---");
        foreach (var t in centro.Tecnicos)
        {
            string disp = t.NombreDisponibilidad();
            Console.WriteLine($"Técnico: {t.NombreCompleto} | Estado: {disp} | Activos: {t.TrabajoActivos}");
        }
    }


    public void Reporte4_TecnicoTopServicios()
    {
        Console.WriteLine("\n--- 4. Técnico con Más Servicios Finalizados ---");
        if (centro.Tecnicos.Count == 0) { Console.WriteLine("No hay técnicos."); return; }

        Tecnico mejor = centro.Tecnicos[0];
        foreach (var t in centro.Tecnicos)
        {
            if (t.ServiciosFinalizados > mejor.ServiciosFinalizados)
            {
                mejor = t;
            }
        }
        Console.WriteLine($"¡Destacado!: {mejor.NombreCompleto} con {mejor.ServiciosFinalizados} servicios finalizados.");
    }

    public void Reporte5_EquiposPorTipo()
    {
        Console.WriteLine("\n--- 5. Equipos Atendidos por Tipo ---");
        int portatiles = 0, escritorios = 0, impresoras = 0;
        foreach (var eq in centro.Equipos)
        {
            if (eq is ComputadoraPortatil) portatiles++;
            else if (eq is ComputadoraEscritorio) escritorios++;
            else if (eq is Impresora) impresoras++;
        }
        Console.WriteLine($"- Computadoras Portátiles: {portatiles}");
        Console.WriteLine($"- Computadoras de Escritorio: {escritorios}");
        Console.WriteLine($"- Impresoras: {impresoras}");
    }

    public void Reporte6_OrdenadasPorCosto()
    {
        Console.WriteLine("\n--- 6. Solicitudes Ordenadas por Costo Total (Bubble Sort) ---");
        List<Solicitud> copia = new List<Solicitud>(centro.Solicitudes);
        ordenador.OrdenarSolicitudesPorCostoTotal(copia);
        foreach (var s in copia)
        {
            Console.WriteLine($"Solicitud: {s.Codigo} | Total: Q{s.Total}");
        }
    }


    public void Reporte7_OrdenadasPorTiempo()
    {
        Console.WriteLine("\n--- 7. Solicitudes Ordenadas por Tiempo Estimado ---");
        List<Solicitud> copia = new List<Solicitud>(centro.Solicitudes);

        for (int i = 0; i < copia.Count - 1; i++)
        {
            for (int j = 0; j < copia.Count - 1 - i; j++)
            {
                if (copia[j].TiempoEstimado > copia[j + 1].TiempoEstimado)
                {
                    var temp = copia[j];
                    copia[j] = copia[j + 1];
                    copia[j + 1] = temp;
                }
            }
        }
        foreach (var s in copia)
        {
            Console.WriteLine($"Solicitud: {s.Codigo} | Tiempo Estimado: {s.TiempoEstimado} horas");
        }
    }

    public void Reporte8_RepuestosPorExistencia()
    {
        Console.WriteLine("\n--- 8. Repuestos Ordenados por Existencia ---");
        List<Repuesto> copia = new List<Repuesto>(centro.Repuestos);
        for (int i = 0; i < copia.Count - 1; i++)
        {
            for (int j = 0; j < copia.Count - 1 - i; j++)
            {
                if (copia[j].Existencia > copia[j + 1].Existencia)
                {
                    var temp = copia[j];
                    copia[j] = copia[j + 1];
                    copia[j + 1] = temp;
                }
            }
        }
        foreach (var r in copia)
        {
            Console.WriteLine($"Repuesto: {r.Nombre} | Existencia: {r.Existencia}");
        }
    }

    public void MostrarRepuestosMinimos()
    {
        Console.WriteLine("\n--- 9. Repuestos con Existencia Mínima o Agotados ---");
        bool hayAlertas = false;
        foreach (var r in centro.Repuestos)
        {
            if (r.EstaEnMinimo() || r.EstaAgotado())
            {
                Console.WriteLine($"- [{r.Codigo}] {r.Nombre} | Stock: {r.Existencia} | Mínimo: {r.ExistenciaMinima}");
                hayAlertas = true;
            }
        }
        if (!hayAlertas) Console.WriteLine("No hay repuestos en nivel crítico.");
    }


    public void Reporte10_IngresosTotales()
    {
        Console.WriteLine("\n--- 10. Ingresos Totales por Servicios Finalizados ---");
        double ingresosTotal = 0;
        foreach (var s in centro.Solicitudes)
        {
            if (s.Estado == 7)
            {
                ingresosTotal += s.Total;
            }
        }
        Console.WriteLine($"Ingresos acumulados por servicios finalizados: Q{ingresosTotal}");
    }

    public void Reporte11_ResultadosBusquedas()
    {
        Console.WriteLine("\n--- 11. Resultados de búsquedas secuenciales y binaria ---");
        Console.Write("Nombre parcial del cliente: ");
        string nombre = Console.ReadLine() ?? "";
        int clientes = 0;
        foreach (Cliente c in centro.Clientes)
            if (c.NombreCompleto.IndexOf(nombre, StringComparison.OrdinalIgnoreCase) >= 0)
            { c.MostrarInformacion(); clientes++; }
        Console.WriteLine("Coincidencias de clientes: " + clientes);

        Console.Write("Marca parcial del equipo: ");
        string marca = Console.ReadLine() ?? "";
        int equipos = 0;
        foreach (Equipo e in centro.Equipos)
            if (e.Marca.IndexOf(marca, StringComparison.OrdinalIgnoreCase) >= 0)
            { e.MostrarInformacion(); equipos++; }
        Console.WriteLine("Coincidencias de equipos: " + equipos);

        Console.Write("Estado de solicitud (1 a 8): ");
        string entradaEstado = Console.ReadLine() ?? "";
        int estado;
        if (int.TryParse(entradaEstado, out estado) && estado >= 1 && estado <= 8)
        {
            List<Solicitud> encontradas = buscador.BuscarSolicitudesPorEstado(estado);
            foreach (Solicitud s in encontradas) s.MostrarInformacion();
            Console.WriteLine("Coincidencias de solicitudes por estado: " + encontradas.Count);
        }
        else Console.WriteLine("Estado inválido; se omitió esta búsqueda.");

        Console.Write("Código de solicitud para búsqueda binaria: ");
        string codigo = Console.ReadLine() ?? "";
        Solicitud? solicitud = buscador.BuscarSolicitudBinaria(codigo);
        if (solicitud == null) Console.WriteLine("La búsqueda binaria no encontró la solicitud.");
        else { Console.WriteLine("Resultado de búsqueda binaria:"); solicitud.MostrarInformacion(); }
    }

    public void MostrarMatrizCarga()
    {
        Console.WriteLine("\n--- 12. Matriz Semanal de Carga de Trabajo ---");
        Console.WriteLine("Técnico          | Lun | Mar | Mié | Jue | Vie ");
        Console.WriteLine("--------------------------------------------------");

        int[,] matriz = centro.MatrizCargaSemanal;
        int mayorCarga = -1;
        int indiceTecnico = -1;
        int indiceDia = -1;
        string[] dias = { "Lunes", "Martes", "Miércoles", "Jueves", "Viernes" };
        for (int i = 0; i < centro.Tecnicos.Count; i++)
        {
            string nombreTec = centro.Tecnicos[i].NombreCompleto;
            if (nombreTec.Length > 15) nombreTec = nombreTec.Substring(0, 15);
            Console.Write($"{nombreTec,-16} | ");
            for (int j = 0; j < 5; j++)
            {
                Console.Write($"{matriz[i, j],3} | ");
                if (matriz[i, j] > mayorCarga) { mayorCarga = matriz[i, j]; indiceTecnico = i; indiceDia = j; }
            }
            Console.WriteLine();
        }
        if (indiceTecnico >= 0)
            Console.WriteLine("Mayor carga: " + centro.Tecnicos[indiceTecnico].NombreCompleto + " el día " + dias[indiceDia] + " con " + mayorCarga + " solicitud(es).");
        else Console.WriteLine("No hay técnicos registrados.");
    }
}