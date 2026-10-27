using System;
using System.Collections.Generic;

namespace XelaTechSoporte;

class Solicitud
{
    private string numeroSolicitud;
    private Cliente cliente;
    private Equipo equipo;
    private Tecnico tecnicoAsignado;
    private int estado;
    private string motivoIngreso;
    private string diagnostico;
    private DateTime fechaIngreso;
    private DateTime fechaEntrega;
    private List<UsoRepuesto> repuestosUtilizados;
    private Stack<string> historialEstados;
    private int calificacionServicio;

    public Solicitud(string numeroSolicitud, Cliente cliente, Equipo equipo, string motivoIngreso)
    {
        this.numeroSolicitud = numeroSolicitud;
        this.cliente = cliente;
        this.equipo = equipo;
        this.motivoIngreso = motivoIngreso;
        this.estado = 1;
        this.diagnostico = "Pendiente de diagnóstico";
        this.fechaIngreso = DateTime.Now;
        this.fechaEntrega = DateTime.MinValue;
        this.repuestosUtilizados = new List<UsoRepuesto>();
        this.historialEstados = new Stack<string>();
        this.calificacionServicio = 0;

        historialEstados.Push("Recibido el " + fechaIngreso.ToString("dd/MM/yyyy HH:mm"));
    }

    public string NumeroSolicitud { get { return numeroSolicitud; } }
    public Cliente Cliente { get { return cliente; } }
    public Equipo Equipo { get { return equipo; } }
    public Tecnico TecnicoAsignado { get { return tecnicoAsignado; } set { tecnicoAsignado = value; } }
    public int Estado { get { return estado; } }
    public string MotivoIngreso { get { return motivoIngreso; } }
    public string Diagnostico { get { return diagnostico; } set { diagnostico = value; } }
    public DateTime FechaIngreso { get { return fechaIngreso; } }
    public DateTime FechaEntrega { get { return fechaEntrega; } set { fechaEntrega = value; } }
    public List<UsoRepuesto> RepuestosUtilizados { get { return repuestosUtilizados; } }
    public Stack<string> HistorialEstados { get { return historialEstados; } }
    public int CalificacionServicio { get { return calificacionServicio; } set { calificacionServicio = value; } }

    public void CambiarEstado(int nuevoEstado, string comentario)
    {
        if (nuevoEstado < 1 || nuevoEstado > 7)
        {
            Console.WriteLine("ERROR: Estado inválido.");
            return;
        }

        estado = nuevoEstado;
        string registro = "Estado " + nuevoEstado + " (" + comentario + ") el " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        historialEstados.Push(registro);
    }

    public void AgregarRepuesto(UsoRepuesto uso)
    {
        if (uso != null)
        {
            repuestosUtilizados.Add(uso);
        }
    }

    public double CalcularCostoRepuestos()
    {
        double total = 0.0;
        for (int i = 0; i < repuestosUtilizados.Count; i++)
        {
            total += repuestosUtilizados[i].CalcularSubtotal();
        }
        return total;
    }

    public void MostrarInformacion()
    {
        string nomCliente = cliente != null ? cliente.NombreCompleto : "Sin cliente";
        string codEquipo = equipo != null ? equipo.CodigoInterno : "Sin equipo";
        string nomTec = tecnicoAsignado != null ? tecnicoAsignado.NombreCompleto : "Sin asignar";

        Console.WriteLine("Solicitud: " + numeroSolicitud +
                          " | Cliente: " + nomCliente +
                          " | Equipo: " + codEquipo +
                          " | Técnico: " + nomTec +
                          " | Estado: " + estado +
                          " | Motivo: " + motivoIngreso);
    }
}