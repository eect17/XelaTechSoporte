using System;
using System.Collections.Generic;

namespace XelaTechSoporte;

class CentroServicio
{
    private List<Cliente> clientes;
    private List<Tecnico> tecnicos;
    private List<Repuesto> repuestos;
    private List<Equipo> equipos;
    private List<Solicitud> solicitudes;

    private Queue<Solicitud> colaPortatiles;
    private Queue<Solicitud> colaEscritorio;
    private Queue<Solicitud> colaImpresoras;

    public CentroServicio()
    {
        clientes = new List<Cliente>();
        tecnicos = new List<Tecnico>();
        repuestos = new List<Repuesto>();
        equipos = new List<Equipo>();
        solicitudes = new List<Solicitud>();

        colaPortatiles = new Queue<Solicitud>();
        colaEscritorio = new Queue<Solicitud>();
        colaImpresoras = new Queue<Solicitud>();
    }

    public List<Cliente> Clientes { get { return clientes; } }
    public List<Tecnico> Tecnicos { get { return tecnicos; } }
    public List<Repuesto> Repuestos { get { return repuestos; } }
    public List<Equipo> Equipos { get { return equipos; } }
    public List<Solicitud> Solicitudes { get { return solicitudes; } }

    public Queue<Solicitud> ColaPortatiles { get { return colaPortatiles; } }
    public Queue<Solicitud> ColaEscritorio { get { return colaEscritorio; } }
    public Queue<Solicitud> ColaImpresoras { get { return colaImpresoras; } }

    public Cliente BuscarClientePorCodigo(string codigo)
    {
        if (string.IsNullOrWhiteSpace(codigo)) return null;

        for (int i = 0; i < clientes.Count; i++)
        {
            if (clientes[i].Codigo == codigo.Trim())
            {
                return clientes[i];
            }
        }
        return null;
    }

    public bool RegistrarCliente(Cliente nuevoCliente)
    {
        if (nuevoCliente == null) return false;

        if (BuscarClientePorCodigo(nuevoCliente.Codigo) != null)
        {
            Console.WriteLine("ERROR: Ya existe un cliente registrado con ese código.");
            return false;
        }

        clientes.Add(nuevoCliente);
        Console.WriteLine("¡Cliente registrado exitosamente!");
        return true;
    }

    public Tecnico BuscarTecnicoPorCodigo(string codigo)
    {
        if (string.IsNullOrWhiteSpace(codigo)) return null;

        for (int i = 0; i < tecnicos.Count; i++)
        {
            if (tecnicos[i].Codigo == codigo.Trim())
            {
                return tecnicos[i];
            }
        }
        return null;
    }

    public bool RegistrarTecnico(Tecnico nuevoTecnico)
    {
        if (nuevoTecnico == null) return false;

        if (BuscarTecnicoPorCodigo(nuevoTecnico.Codigo) != null)
        {
            Console.WriteLine("ERROR: Ya existe un técnico con ese código.");
            return false;
        }

        tecnicos.Add(nuevoTecnico);
        Console.WriteLine("¡Técnico registrado exitosamente!");
        return true;
    }
    // ==========================================
    // MÉTODOS DE BÚSQUEDA Y REGISTRO: REPUESTOS
    // ==========================================

    public Repuesto BuscarRepuestoPorCodigo(string codigo)
    {
        if (string.IsNullOrWhiteSpace(codigo)) return null;

        for (int i = 0; i < repuestos.Count; i++)
        {
            if (repuestos[i].Codigo == codigo.Trim())
            {
                return repuestos[i];
            }
        }
        return null;
    }

    public bool RegistrarRepuesto(Repuesto nuevoRepuesto)
    {
        if (nuevoRepuesto == null) return false;

        if (BuscarRepuestoPorCodigo(nuevoRepuesto.Codigo) != null)
        {
            Console.WriteLine("ERROR: Ya existe un repuesto con ese código.");
            return false;
        }

        repuestos.Add(nuevoRepuesto);
        Console.WriteLine("¡Repuesto registrado exitosamente!");
        return true;
    }


    public Equipo BuscarEquipoPorCodigo(string codigoInterno)
    {
        if (string.IsNullOrWhiteSpace(codigoInterno)) return null;

        for (int i = 0; i < equipos.Count; i++)
        {
            if (equipos[i].CodigoInterno == codigoInterno.Trim())
            {
                return equipos[i];
            }
        }
        return null;
    }

    public bool RegistrarEquipo(Equipo nuevoEquipo)
    {
        if (nuevoEquipo == null) return false;

        if (BuscarEquipoPorCodigo(nuevoEquipo.CodigoInterno) != null)
        {
            Console.WriteLine("ERROR: Ya existe un equipo con ese código interno.");
            return false;
        }

        equipos.Add(nuevoEquipo);
        Console.WriteLine("¡Equipo registrado exitosamente!");
        return true;
    }

    public Solicitud BuscarSolicitudPorNumero(string numeroSolicitud)
    {
        if (string.IsNullOrWhiteSpace(numeroSolicitud)) return null;

        for (int i = 0; i < solicitudes.Count; i++)
        {
            if (solicitudes[i].NumeroSolicitud == numeroSolicitud.Trim())
            {
                return solicitudes[i];
            }
        }
        return null;
    }

    public bool RegistrarSolicitud(Solicitud nuevaSolicitud)
    {
        if (nuevaSolicitud == null) return false;

        if (BuscarSolicitudPorNumero(nuevaSolicitud.NumeroSolicitud) != null)
        {
            Console.WriteLine("ERROR: Ya existe una solicitud con ese número.");
            return false;
        }

        solicitudes.Add(nuevaSolicitud);

        if (nuevaSolicitud.Cliente != null)
        {
            nuevaSolicitud.Cliente.IncrementarSolicitudes();
        }

        if (nuevaSolicitud.Equipo is ComputadoraPortatil)
        {
            colaPortatiles.Enqueue(nuevaSolicitud);
            Console.WriteLine("Solicitud encolada en la fila de Portátiles.");
        }
        else if (nuevaSolicitud.Equipo is ComputadoraEscritorio)
        {
            colaEscritorio.Enqueue(nuevaSolicitud);
            Console.WriteLine("Solicitud encolada en la fila de Escritorio.");
        }
        else if (nuevaSolicitud.Equipo is Impresora)
        {
            colaImpresoras.Enqueue(nuevaSolicitud);
            Console.WriteLine("Solicitud encolada en la fila de Impresoras.");
        }

        Console.WriteLine("¡Solicitud registrada exitosamente!");
        return true;
    }
}