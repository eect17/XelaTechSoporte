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

    public Repuesto(string codigo, string nombre, string categoria, int existencia, int existenciaMinima, double costoUnitario, double precioVenta)
    {
        Codigo = codigo;
        Nombre = nombre;
        Categoria = categoria;
        ExistenciaMinima = existenciaMinima;
        Existencia = existencia;
        CostoUnitario = costoUnitario;
        PrecioVenta = precioVenta;
    }

    public string Codigo
    {
        get { return codigo; }
        set
        {
            if (value != null && value.Trim() != "")
            {
                codigo = value.Trim();
            }
            else
            {
                Console.WriteLine("ERROR: El código del repuesto no puede estar vacío.");
            }
        }
    }

    public string Nombre
    {
        get { return nombre; }
        set
        {
            if (value != null && value.Trim() != "")
            {
                nombre = value.Trim();
            }
            else
            {
                Console.WriteLine("ERROR: El nombre del repuesto no puede estar vacío.");
            }
        }
    }

    public string Categoria
    {
        get { return categoria; }
        set
        {
            if (value != null && value.Trim() != "")
            {
                categoria = value.Trim();
            }
            else
            {
                Console.WriteLine("ERROR: La categoría no puede estar vacía.");
            }
        }
    }

    public int Existencia
    {
        get { return existencia; }
        set
        {
            if (value >= 0)
            {
                existencia = value;
            }
            else
            {
                Console.WriteLine("ERROR: La existencia no puede ser negativa.");
            }
        }
    }

    public int ExistenciaMinima
    {
        get { return existenciaMinima; }
        set
        {
            if (value >= 0)
            {
                existenciaMinima = value;
            }
            else
            {
                Console.WriteLine("ERROR: La existencia mínima no puede ser negativa.");
            }
        }
    }

    public double CostoUnitario
    {
        get { return costoUnitario; }
        set
        {
            if (value >= 0)
            {
                costoUnitario = value;
            }
            else
            {
                Console.WriteLine("ERROR: El costo unitario no puede ser negativo.");
            }
        }
    }

    public double PrecioVenta
    {
        get { return precioVenta; }
        set
        {
            if (value < 0)
            {
                Console.WriteLine("ERROR: El precio de venta no puede ser negativo.");
            }
            else if (value < costoUnitario)
            {
                Console.WriteLine("ERROR: El precio de venta no puede ser menor al costo unitario.");
            }
            else
            {
                precioVenta = value;
            }
        }
    }

    public bool HayExistencia(int cantidad)
    {
        return cantidad > 0 && cantidad <= existencia;
    }

    public void Descontar(int cantidad)
    {
        if (!HayExistencia(cantidad))
        {
            Console.WriteLine("ERROR: No hay suficiente existencia en inventario.");
            return;
        }
        existencia -= cantidad;
    }

    public void Reponer(int cantidad)
    {
        if (cantidad <= 0)
        {
            Console.WriteLine("ERROR: La cantidad a reponer debe ser mayor a 0.");
            return;
        }
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
                          " | Costo: Q" + costoUnitario +
                          " | Precio: Q" + precioVenta);
    }
}