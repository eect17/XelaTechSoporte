using System;

namespace XelaTechSoporte;

class Tecnico
{
    private string codigo;
    private string nombreCompleto;
    private int especialidad;
    private int disponibilidad;
    private int trabajoActivos;
    private int serviciosFinalizados;
    private int sumaCalificaciones;
    private int cantidadCalificaciones;
    private double calificacionPromedio;
    private const int limiteTrabajoActivos = 3;

    public Tecnico(string codigo, string nombreCompleto, int especialidad, int disponibilidad)
    {
        Codigo = codigo;
        NombreCompleto = nombreCompleto;
        Especialidad = especialidad;
        Disponibilidad = disponibilidad;
        trabajoActivos = 0;
        serviciosFinalizados = 0;
        sumaCalificaciones = 0;
        cantidadCalificaciones = 0;
        calificacionPromedio = 0.0;
    }
    public Tecnico(string codigo, string nombreCompleto, int especialidad)
        : this(codigo, nombreCompleto, especialidad, 1)
    {
    }

    public string Codigo
    {
        get { return codigo; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("El código del técnico no puede estar vacío.");
            codigo = value.Trim();
        }
    }

    public string NombreCompleto
    {
        get { return nombreCompleto; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("El nombre completo no puede estar vacío.");
            nombreCompleto = value.Trim();
        }
    }

    public int Especialidad
    {
        get { return especialidad; }
        set
        {
            if (value < 1 || value > 3)
                throw new ArgumentException("La especialidad debe ser 1, 2 o 3.");
            especialidad = value;
        }
    }
    public int Disponibilidad
    {
        get { return disponibilidad; }
        set
        {
            if (value < 1 || value > 3)
                throw new ArgumentException("La disponibilidad debe ser 1, 2 o 3.");
            disponibilidad = value;
        }
    }

    public int TrabajoActivos { get { return trabajoActivos; } }
    public int ServiciosFinalizados { get { return serviciosFinalizados; } }
    public double CalificacionPromedio { get { return calificacionPromedio; } }
    public int LimiteTrabajoActivos { get { return limiteTrabajoActivos; } }
    public bool PuedeRecibir(int especialidadRequerida)
    {
        if (especialidad != especialidadRequerida) return false;
        if (disponibilidad != 1) return false;
        if (trabajoActivos >= limiteTrabajoActivos) return false;

        return true;
    }
    public void AsignarTrabajo()
    {
        trabajoActivos++;
        if (trabajoActivos >= limiteTrabajoActivos)
        {
            disponibilidad = 2;
        }
    }
    public void LiberarTrabajo()
    {
        if (trabajoActivos > 0)
        {
            trabajoActivos--;
        }
        if (disponibilidad == 2 && trabajoActivos < limiteTrabajoActivos)
        {
            disponibilidad = 1;
        }
    }
    public void FinalizarTrabajo()
    {
        LiberarTrabajo();
        serviciosFinalizados++;
    }

    public void RegistrarCalificacion(int calificacion)
    {
        if (calificacion < 1 || calificacion > 5)
            throw new ArgumentException("La calificación debe estar entre 1 y 5.");
        sumaCalificaciones += calificacion;
        cantidadCalificaciones++;
        calificacionPromedio = (double)sumaCalificaciones / cantidadCalificaciones;
    }

    public string NombreEspecialidad()
    {
        if (especialidad == 1) return "Portátil";
        if (especialidad == 2) return "Escritorio";
        return "Impresora";
    }
    public string NombreDisponibilidad()
    {
        if (disponibilidad == 1) return "Disponible";
        if (disponibilidad == 2) return "Con carga completa";
        return "Fuera de servicio";
    }

    public void MostrarInformacion()
    {
        Console.WriteLine("Código: " + codigo
            + " | Nombre: " + nombreCompleto
            + " | Especialidad: " + NombreEspecialidad()
            + " | Estado: " + NombreDisponibilidad()
            + " | Trabajos Activos: " + trabajoActivos + "/" + limiteTrabajoActivos
            + " | Finalizados: " + serviciosFinalizados
            + " | Promedio: " + calificacionPromedio.ToString("F1"));
    }
}