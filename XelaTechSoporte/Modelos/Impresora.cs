using System;

namespace XelaTechSoporte;

class Impresora : Equipo
{
    private bool esMultifuncional;

    public Impresora(string codigoInterno, string numeroSerie, string marca, string modelo, Cliente propietario, string estadoFisico, string accesorios, bool esMultifuncional)
        : base(codigoInterno, numeroSerie, marca, modelo, propietario, 3, estadoFisico, accesorios)
    {
        this.esMultifuncional = esMultifuncional;
    }

    public bool EsMultifuncional
    {
        get { return esMultifuncional; }
        set { esMultifuncional = value; }
    }
    public override double CalcularCostoRevision()
    {
        if (esMultifuncional)
        {
            return 110.00;
        }
        return 80.00;
    }

    public override int ObtenerTiempoEstimado()
    {
        if (esMultifuncional)
        {
            return 3;
        }
        return 2;
    }

    public override int ObtenerEspecialidadRequerida()
    {
        return 3;
    }
    public override void MostrarInformacion()
    {
        base.MostrarInformacion();
        string multiStr = esMultifuncional ? "Sí" : "No";
        Console.WriteLine("   Tipo: Impresora | Es Multifuncional: " + multiStr +
                          " | Costo Revisión: Q" + CalcularCostoRevision().ToString("F2") +
                          " | Tiempo: " + ObtenerTiempoEstimado() + "h");
    }
}