using System;

namespace XelaTechSoporte;

class Cliente
{
    private string codigo;
    private string nombreCompleto;
    private string telefono;
    private string correo;
    private string tipoCliente;
    private int cantidadSolicitudes;


    public Cliente(string codigo, string nombreCompleto, string telefono, string correo, string tipoCliente)
    {
        Codigo = codigo;
        NombreCompleto = nombreCompleto;
        Telefono = telefono;
        Correo = correo;
        TipoCliente = tipoCliente;
        cantidadSolicitudes = 0;
    }


    public Cliente(string codigo, string nombreCompleto, string telefono, string correo)
        : this(codigo, nombreCompleto, telefono, correo, "Particular")
    {
    }


    public string Codigo
    {
        get { return codigo; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("El código no puede estar vacío.");
            codigo = value.Trim();
        }
    }


    public string NombreCompleto
    {
        get { return nombreCompleto; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("El nombre no puede estar vacío.");
            nombreCompleto = value.Trim();
        }
    }


    public string TipoCliente
    {
        get { return tipoCliente; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("El tipo de cliente no puede estar vacío.");
            tipoCliente = value.Trim();
        }
    }


    public string Telefono
    {
        get { return telefono; }
        set
        {
            if (!Validador.EsTelefonoValido(value))
                throw new ArgumentException("Teléfono inválido.");
            telefono = value.Trim();
        }
    }

    // Propiedad Correo
    public string Correo
    {
        get { return correo; }
        set
        {
            if (!Validador.EsCorreoValido(value))
                throw new ArgumentException("Correo inválido.");
            correo = value.Trim();
        }
    }

    public int CantidadSolicitudes
    {
        get { return cantidadSolicitudes; }
    }

    public void ActualizarDatos(string telefono, string correo, string tipoCliente)
    {
        Telefono = telefono;
        Correo = correo;
        TipoCliente = tipoCliente;
    }

    public void IncrementarSolicitudes()
    {
        cantidadSolicitudes++;
    }

    public void MostrarInformacion()
    {
        Console.WriteLine("Código: " + codigo +
                          " | Nombre: " + nombreCompleto +
                          " | Teléfono: " + telefono +
                          " | Correo: " + correo +
                          " | Tipo: " + tipoCliente +
                          " | Solicitudes: " + cantidadSolicitudes);
    }
}