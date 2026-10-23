using System;

namespace XelaTechSoporte;

class Equipo
{
    private string codigoInterno;
    private string numeroSerie;
    private string marca;
    private string modelo;
    private Cliente propietario;
    private int tipoEquipo;          
    private string estadoFisico;
    private string accesorios;

    public Equipo(string codigoInterno, string numeroSerie, string marca, string modelo, Cliente propietario, int tipoEquipo, string estadoFisico, string accesorios)
    {
        CodigoInterno = codigoInterno;
        NumeroSerie = numeroSerie;
        Marca = marca;
        Modelo = modelo;
        this.propietario = propietario;
        this.tipoEquipo = tipoEquipo;
        EstadoFisico = estadoFisico;
        Accesorios = accesorios;
    }

    public string CodigoInterno
    {
        get { return codigoInterno; }
        set
        {
            if (value != null && value.Trim() != "")
                codigoInterno = value.Trim();
            else
                Console.WriteLine("ERROR: El código interno no puede estar vacío.");
        }
    }

    public string NumeroSerie
    {
        get { return numeroSerie; }
        set
        {
            if (value != null && value.Trim() != "")
                numeroSerie = value.Trim();
            else
                Console.WriteLine("ERROR: El número de serie no puede estar vacío.");
        }
    }

    public string Marca
    {
        get { return marca; }
        set
        {
            if (value != null && value.Trim() != "")
                marca = value.Trim();
            else
                Console.WriteLine("ERROR: La marca no puede estar vacía.");
        }
    }

    public string Modelo
    {
        get { return modelo; }
        set
        {
            if (value != null && value.Trim() != "")
                modelo = value.Trim();
            else
                Console.WriteLine("ERROR: El modelo no puede estar vacío.");
        }
    }

    public Cliente Propietario
    {
        get { return propietario; }
        set { propietario = value; }
    }

    public int TipoEquipo
    {
        get { return tipoEquipo; }
        set { tipoEquipo = value; }
    }

    public string EstadoFisico
    {
        get { return estadoFisico; }
        set
        {
            if (value != null && value.Trim() != "")
                estadoFisico = value.Trim();
            else
                Console.WriteLine("ERROR: El estado físico no puede estar vacío.");
        }
    }

    public string Accesorios
    {
        get { return accesorios; }
        set { accesorios = value != null ? value.Trim() : ""; }
    }

    public virtual void MostrarInformacion()
    {
        string nombrePropietario = propietario != null ? propietario.NombreCompleto : "Sin propietario";
        Console.WriteLine("Código: " + codigoInterno +
                          " | Serie: " + numeroSerie +
                          " | Marca: " + marca +
                          " | Modelo: " + modelo +
                          " | Propietario: " + nombrePropietario +
                          " | Estado: " + estadoFisico);
    }
}