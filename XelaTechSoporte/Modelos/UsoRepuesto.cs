using System;

namespace XelaTechSoporte;

class UsoRepuesto
{
    private Repuesto repuesto;
    private int cantidadUtilizada;

    public UsoRepuesto(Repuesto repuesto, int cantidadUtilizada)
    {
        if (repuesto == null)
        {
            Console.WriteLine("ERROR: El repuesto no puede ser nulo.");
            return;
        }

        if (cantidadUtilizada <= 0)
        {
            Console.WriteLine("ERROR: La cantidad utilizada debe ser mayor a 0.");
            return;
        }

        this.repuesto = repuesto;
        this.cantidadUtilizada = cantidadUtilizada;
    }

    public Repuesto Repuesto { get { return repuesto; } }
    public int CantidadUtilizada { get { return cantidadUtilizada; } }

    public double CalcularSubtotal()
    {
        if (repuesto == null) return 0.0;
        return cantidadUtilizada * repuesto.PrecioVenta;
    }

    public void MostrarInformacion()
    {
        if (repuesto == null) return;

        Console.WriteLine("Repuesto: " + repuesto.Nombre +
                          " | Cantidad: " + cantidadUtilizada +
                          " | Precio Venta: Q" + repuesto.PrecioVenta +
                          " | Subtotal: Q" + CalcularSubtotal());
    }
}