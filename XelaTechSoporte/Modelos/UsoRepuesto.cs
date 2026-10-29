using System;

namespace XelaTechSoporte;

class UsoRepuesto
{
    private Repuesto repuesto;
    private int cantidadUtilizada;

    public UsoRepuesto(Repuesto repuesto, int cantidadUtilizada)
    {
        if (repuesto == null)
            throw new ArgumentException("El repuesto no puede ser nulo.");
        if (cantidadUtilizada <= 0)
            throw new ArgumentException("La cantidad utilizada debe ser mayor a 0.");

        this.repuesto = repuesto;
        this.cantidadUtilizada = cantidadUtilizada;
    }

    public Repuesto Repuesto { get { return repuesto; } }
    public int CantidadUtilizada { get { return cantidadUtilizada; } }

    public double CalcularSubtotal()
    {
        return cantidadUtilizada * repuesto.PrecioVenta;
    }

    public void MostrarInformacion()
    {
        Console.WriteLine("Repuesto: " + repuesto.Nombre +
                          " | Cantidad: " + cantidadUtilizada +
                          " | Precio Venta: Q" + repuesto.PrecioVenta.ToString("F2") +
                          " | Subtotal: Q" + CalcularSubtotal().ToString("F2"));
    }
}