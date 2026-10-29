using System;

namespace XelaTechSoporte;

class ComputadoraEscritorio : Equipo
{
    private bool tieneMonitor;

    public ComputadoraEscritorio(string codigoInterno, string numeroSerie, string marca, string modelo, Cliente propietario, string estadoFisico, string accesorios, bool tieneMonitor)
        : base(codigoInterno, numeroSerie, marca, modelo, propietario, 2, estadoFisico, accesorios)
    {
        this.tieneMonitor = tieneMonitor;
    }

    public bool TieneMonitor
    {
        get { return tieneMonitor; }
        set { tieneMonitor = value; }
    }
    public override double CalcularCostoRevision()
    {
        double costo = 120.00;
        if (!tieneMonitor)
        {
            costo += 20.00;
        }
        return costo;
    }
    public override int ObtenerTiempoEstimado()
    {
        return 3;
    }
    public override int ObtenerEspecialidadRequerida()
    {
        return 2;
    }
    public override void MostrarInformacion()
    {
        base.MostrarInformacion();
        string monitorStr = tieneMonitor ? "Sí" : "No";
        Console.WriteLine("   Tipo: Computadora Escritorio | Trae Monitor: " + monitorStr +
                          " | Costo Revisión: Q" + CalcularCostoRevision().ToString("F2") +
                          " | Tiempo: " + ObtenerTiempoEstimado() + "h");
    }
}