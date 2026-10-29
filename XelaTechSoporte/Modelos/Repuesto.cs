using System;

namespace XelaTechSoporte;

class Repuesto
{
    private string codigo;
    private string nombre;
    private string categoria;
    private int existencia;
    private int existenciaMinima;
    private double costoUnitario;
    private double precioVenta;
    private int cantidadUtilizada;

    public Repuesto(string codigo, string nombre, string categoria, int existencia, int existenciaMinima, double costoUnitario, double precioVenta)
    {
        Codigo = codigo;
        Nombre = nombre;
        Categoria = categoria;
        ExistenciaMinima = existenciaMinima;
        Existencia = existencia;
        CostoUnitario = costoUnitario;
        PrecioVenta = precioVenta;
        cantidadUtilizada = 0;
    }

    public string Codigo
    {
        get { return codigo; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("El código del repuesto no puede estar vacío.");
            codigo = value.Trim();
        }
    }

    public string Nombre
    {
        get { return nombre; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("El nombre del repuesto no puede estar vacío.");
            nombre = value.Trim();
        }
    }

    public string Categoria
    {
        get { return categoria; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("La categoría no puede estar vacía.");
            categoria = value.Trim();
        }
    }

    public int Existencia
    {
        get { return existencia; }
        set
        {
            if (value < 0)
                throw new ArgumentException("La existencia no puede ser negativa.");
            existencia = value;
        }
    }

    public int ExistenciaMinima
    {
        get { return existenciaMinima; }
        set
        {
            if (value < 0)
                throw new ArgumentException("La existencia mínima no puede ser negativa.");
            existenciaMinima = value;
        }
    }

    public double CostoUnitario
    {
        get { return costoUnitario; }
        set
        {
            if (value < 0)
                throw new ArgumentException("El costo unitario no puede ser negativo.");
            costoUnitario = value;
        }
    }

    public double PrecioVenta
    {
        get { return precioVenta; }
        set
        {
            if (value < 0)
                throw new ArgumentException("El precio de venta no puede ser negativo.");
            if (value < costoUnitario)
                throw new ArgumentException("El precio de venta no puede ser menor al costo unitario.");
            precioVenta = value;
        }
    }

    public int CantidadUtilizada { get { return cantidadUtilizada; } }

    public bool HayExistencia(int cantidad)
    {
        return cantidad > 0 && cantidad <= existencia;
    }

    public void Descontar(int cantidad)
    {
        if (!HayExistencia(cantidad))
            throw new ArgumentException("No hay suficiente existencia en inventario.");
        existencia -= cantidad;
        cantidadUtilizada += cantidad;
    }

    public void Reponer(int cantidad)
    {
        if (cantidad <= 0)
            throw new ArgumentException("La cantidad a reponer debe ser mayor a 0.");
        existencia += cantidad;
    }

    public bool EstaEnMinimo()
    {
        return existencia <= existenciaMinima;
    }

    public bool EstaAgotado()
    {
        return existencia == 0;
    }
    public void MostrarInformacion()
    {
        Console.WriteLine("Código: " + codigo +
                          " | Nombre: " + nombre +
                          " | Categoría: " + categoria +
                          " | Stock: " + existencia +
                          " | Mínimo: " + existenciaMinima +
                          " | Costo: Q" + costoUnitario.ToString("F2") +
                          " | Precio: Q" + precioVenta.ToString("F2") +
                          " | Cantidad utilizada: " + cantidadUtilizada);
    }
}