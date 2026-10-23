
using System;

namespace XelaTechSoporte;

class ComputadoraPortatil : Equipo
{
    private bool tieneCargador;

    public ComputadoraPortatil(string codigoInterno, string numeroSerie, string marca, string modelo, Cliente propietario, string estadoFisico, string accesorios, bool tieneCargador)
        : base(codigoInterno, numeroSerie, marca, modelo, propietario, 1, estadoFisico, accesorios)
    {
        this.tieneCargador = tieneCargador;
    }

    public bool TieneCargador
    {
        get { return tieneCargador; }
        set { tieneCargador = value; }
    }

    public double CalcularCostoRevision()
    {
        double costo = 150.00;
        if (!tieneCargador)
        {
            costo += 25.00;
        }
        return costo;
    }

    public int ObtenerTiempoEstimado()
    {
        return 4;
    }


    public int ObtenerEspecialidadRequerida()
    {
        return 1;
    }

    public override void MostrarInformacion()
    {
        base.MostrarInformacion();
        string cargadorStr = tieneCargador ? "Sí" : "No";
        Console.WriteLine("Tipo: Computadora Portátil | Trae Cargador: " + cargadorStr +
                          " | Costo Revisión: Q" + CalcularCostoRevision() +
                          " | Tiempo: " + ObtenerTiempoEstimado() + "h");
    }
}