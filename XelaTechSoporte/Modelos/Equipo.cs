using System;

namespace XelaTechSoporte;

abstract class Equipo
{
    protected string codigoInterno;
    protected string numeroSerie;
    protected string marca;
    protected string modelo;
    protected Cliente propietario;
    protected int tipoEquipo;
    protected string estadoFisico;
    protected string accesorios;

    protected Equipo(string codigoInterno, string numeroSerie, string marca, string modelo, Cliente propietario, int tipoEquipo, string estadoFisico, string accesorios)
    {
        CodigoInterno = codigoInterno;
        NumeroSerie = numeroSerie;
        Marca = marca;
        Modelo = modelo;
        Propietario = propietario;
        TipoEquipo = tipoEquipo;
        EstadoFisico = estadoFisico;
        Accesorios = accesorios;
    }

    public string CodigoInterno
    {
        get { return codigoInterno; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("El código interno no puede estar vacío.");
            codigoInterno = value.Trim();
        }
    }

    public string NumeroSerie
    {
        get { return numeroSerie; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("El número de serie no puede estar vacío.");
            numeroSerie = value.Trim();
        }
    }

    public string Marca
    {
        get { return marca; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("La marca no puede estar vacía.");
            marca = value.Trim();
        }
    }

    public string Modelo
    {
        get { return modelo; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("El modelo no puede estar vacío.");
            modelo = value.Trim();
        }
    }

    public Cliente Propietario
    {
        get { return propietario; }
        set
        {
            if (value == null)
                throw new ArgumentException("El equipo necesita un propietario.");
            propietario = value;
        }
    }

    public int TipoEquipo
    {
        get { return tipoEquipo; }
        set
        {
            if (value < 1 || value > 3)
                throw new ArgumentException("El tipo de equipo debe ser 1, 2 o 3.");
            tipoEquipo = value;
        }
    }

    public string EstadoFisico
    {
        get { return estadoFisico; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("El estado físico no puede estar vacío.");
            estadoFisico = value.Trim();
        }
    }

    public string Accesorios
    {
        get { return accesorios; }
        set { accesorios = value != null ? value.Trim() : ""; }
    }

    public abstract double CalcularCostoRevision();

    public abstract int ObtenerTiempoEstimado();

    public abstract int ObtenerEspecialidadRequerida();

    public virtual void MostrarInformacion()
    {
        Console.WriteLine("Código: " + codigoInterno +
                          " | Serie: " + numeroSerie +
                          " | Marca: " + marca +
                          " | Modelo: " + modelo +
                          " | Propietario: " + propietario.NombreCompleto +
                          " | Estado: " + estadoFisico +
                          " | Accesorios: " + accesorios);
    }
}