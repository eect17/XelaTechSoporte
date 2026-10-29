using System;
using System.Collections.Generic;

namespace XelaTechSoporte;

class CentroServicio
{
    public List<Cliente> Clientes { get; private set; }
    public List<Tecnico> Tecnicos { get; private set; }
    public List<Repuesto> Repuestos { get; private set; }
    public List<Solicitud> Solicitudes { get; private set; }
    public Queue<Solicitud> ColaUrgentes { get; private set; }
    public Queue<Solicitud> ColaNormales { get; private set; }
    public int[,] MatrizCargaSemanal { get; private set; }

    public CentroServicio()
    {
        Clientes = new List<Cliente>();
        Tecnicos = new List<Tecnico>();
        Repuestos = new List<Repuesto>();
        Solicitudes = new List<Solicitud>();
        ColaUrgentes = new Queue<Solicitud>();
        ColaNormales = new Queue<Solicitud>();
        MatrizCargaSemanal = new int[10, 5];
    }

    public bool RegistrarCliente(Cliente cliente)
    {
        if (cliente == null || Clientes.Exists(c => c.Codigo.Equals(cliente.Codigo, StringComparison.OrdinalIgnoreCase))) return false;
        Clientes.Add(cliente); return true;
    }

    public bool RegistrarTecnico(Tecnico tecnico)
    {
        if (tecnico == null || Tecnicos.Exists(t => t.Codigo.Equals(tecnico.Codigo, StringComparison.OrdinalIgnoreCase))) return false;
        Tecnicos.Add(tecnico);
        int[,] nuevaMatriz = new int[Tecnicos.Count, 5];
        int filasCopiar = MatrizCargaSemanal.GetLength(0) < nuevaMatriz.GetLength(0) ? MatrizCargaSemanal.GetLength(0) : nuevaMatriz.GetLength(0);
        for (int i = 0; i < filasCopiar; i++)
            for (int j = 0; j < 5; j++) nuevaMatriz[i, j] = MatrizCargaSemanal[i, j];
        MatrizCargaSemanal = nuevaMatriz;
        return true;
    }

    public bool RegistrarRepuesto(Repuesto repuesto)
    {
        if (repuesto == null || Repuestos.Exists(r => r.Codigo.Equals(repuesto.Codigo, StringComparison.OrdinalIgnoreCase))) return false;
        Repuestos.Add(repuesto); return true;
    }

    public bool RegistrarSolicitud(Solicitud solicitud)
    {
        if (solicitud == null || Solicitudes.Exists(s => s.NumeroSolicitud.Equals(solicitud.NumeroSolicitud, StringComparison.OrdinalIgnoreCase))) return false;
        foreach (Solicitud anterior in Solicitudes)
            if (anterior.Equipo.NumeroSerie.Equals(solicitud.Equipo.NumeroSerie, StringComparison.OrdinalIgnoreCase) && anterior.EstaActiva()) return false;
        Solicitudes.Add(solicitud);
        solicitud.Cliente.IncrementarSolicitudes();
        if (solicitud.Prioridad == 2) ColaUrgentes.Enqueue(solicitud); else ColaNormales.Enqueue(solicitud);
        return true;
    }

    public bool RegistrarEquipo(Equipo equipo)
    {
        if (equipo == null) return false;
        foreach (Equipo existente in Equipos)
            if (existente.CodigoInterno.Equals(equipo.CodigoInterno, StringComparison.OrdinalIgnoreCase) || existente.NumeroSerie.Equals(equipo.NumeroSerie, StringComparison.OrdinalIgnoreCase)) return false;
        equipos.Add(equipo); return true;
    }

    private List<Equipo> equipos = new List<Equipo>();
    public List<Equipo> Equipos { get { return equipos; } }

    public Tecnico? BuscarTecnicoDisponible(int especialidad)
    {
        foreach (Tecnico tecnico in Tecnicos)
            if (tecnico.PuedeRecibir(especialidad)) return tecnico;
        return null;
    }

    public bool AsignarSolicitud(Solicitud solicitud, Tecnico tecnico, int diaSemana)
    {
        if (solicitud == null || tecnico == null || !solicitud.EstaActiva()) return false;
        if (!tecnico.PuedeRecibir(solicitud.Equipo.ObtenerEspecialidadRequerida())) return false;
        if (solicitud.Estado == 1 && !solicitud.CambiarEstado(2)) return false;
        if (solicitud.Estado != 2) return false;
        solicitud.AsignarTecnico(tecnico);
        if (!solicitud.CambiarEstado(3)) return false;
        tecnico.AsignarTrabajo();
        int indice = Tecnicos.IndexOf(tecnico);
        if (indice >= 0 && indice < MatrizCargaSemanal.GetLength(0))
        {
            if (diaSemana < 0 || diaSemana > 4) diaSemana = DateTime.Now.DayOfWeek == DayOfWeek.Saturday ? 4 : DateTime.Now.DayOfWeek == DayOfWeek.Sunday ? 0 : (int)DateTime.Now.DayOfWeek - 1;
            MatrizCargaSemanal[indice, diaSemana]++;
        }
        return true;
    }

    public bool AtenderSiguiente()
    {
        Solicitud? solicitud = ObtenerSiguienteDeCola(ColaUrgentes);
        if (solicitud == null) solicitud = ObtenerSiguienteDeCola(ColaNormales);
        if (solicitud == null) return false;
        Tecnico? tecnico = BuscarTecnicoDisponible(solicitud.Equipo.ObtenerEspecialidadRequerida());
        if (tecnico == null)
        {
            if (solicitud.Prioridad == 2) ColaUrgentes.Enqueue(solicitud); else ColaNormales.Enqueue(solicitud);
            Console.WriteLine("No hay técnico disponible para la especialidad requerida. La solicitud sigue en espera.");
            return false;
        }
        if (!AsignarSolicitud(solicitud, tecnico, -1))
        {
            if (solicitud.Prioridad == 2) ColaUrgentes.Enqueue(solicitud); else ColaNormales.Enqueue(solicitud);
            return false;
        }
        Console.WriteLine("Solicitud " + solicitud.NumeroSolicitud + " asignada a " + tecnico.NombreCompleto + ".");
        return true;
    }

    private Solicitud? ObtenerSiguienteDeCola(Queue<Solicitud> cola)
    {
        while (cola.Count > 0)
        {
            Solicitud solicitud = cola.Dequeue();
            if (solicitud.Estado == 1 || solicitud.Estado == 2) return solicitud;
        }
        return null;
    }
}
