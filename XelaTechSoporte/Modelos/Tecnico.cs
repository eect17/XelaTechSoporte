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

    public int LimiteTrabajoActivos = 3;

    public Tecnico(string codigo, string nombreCompleto, int especialidad, int disponibilidad)
    {
        this.codigo = codigo;
        this.nombreCompleto = nombreCompleto;
        this.especialidad = especialidad;
        this.disponibilidad = disponibilidad;
        this.trabajoActivos = 0;
        this.serviciosFinalizados = 0;
        this.sumaCalificaciones = 0;
        this.cantidadCalificaciones = 0;
        this.calificacionPromedio = 0.0;
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
            if (value != null && value.Trim() != "")
            {
                codigo = value.Trim();
            }
            else
            {
                Console.WriteLine("ERROR: El código no puede estar vacío.");
            }
        }
    }

    public string NombreCompleto
    {
        get { return nombreCompleto; }
        set
        {
            if (value != null && value.Trim() != "")
            {
                nombreCompleto = value.Trim();
            }
            else
            {
                Console.WriteLine("ERROR: El nombre completo no puede estar vacío.");
            }
        }
    }

    public int Especialidad
    {
        get { return especialidad; }
        set
        {
            if (value >= 1 && value <= 3)
            {
                especialidad = value;
            }
            else
            {
                Console.WriteLine("ERROR: La especialidad debe ser 1, 2 o 3.");
            }
        }
    }

    public int Disponibilidad
    {
        get { return disponibilidad; }
        set
        {
            if (value >= 1 && value <= 3)
            {
                disponibilidad = value;
            }
            else
            {
                Console.WriteLine("ERROR: Disponibilidad invalida.");
            }
        }
    }

    public int TrabajoActivos { get { return trabajoActivos; } }
    public int ServiciosFinalizados { get { return serviciosFinalizados; } }
    public double CalificacionPromedio { get { return calificacionPromedio; } }

    public bool PuedeRecibir(int especialidadRequerida)
    {
        if (especialidad != especialidadRequerida) return false;
        if (disponibilidad == 3) return false;
        if (trabajoActivos >= LimiteTrabajoActivos) return false;

        return true;
    }

    public void AsignarTrabajo()
    {
        trabajoActivos++;
        if (trabajoActivos >= LimiteTrabajoActivos)
        {
            disponibilidad = 2;
        }
    }

    public void FinalizarTrabajo()
    {
        if (trabajoActivos > 0)
        {
            trabajoActivos--;
        }
        serviciosFinalizados++;
        if (disponibilidad == 2 && trabajoActivos < LimiteTrabajoActivos)
        {
            disponibilidad = 1;
        }
    }

    public void RegistrarCalificacion(int calificacion)
    {
        if (calificacion < 1 || calificacion > 5)
        {
            Console.WriteLine("ERROR: La calificación debe estar entre 1 y 5.");
            return;
        }
        sumaCalificaciones += calificacion;
        cantidadCalificaciones++;
        calificacionPromedio = (double)sumaCalificaciones / cantidadCalificaciones;
    }

    public void MostrarInformacion()
    {
        Console.WriteLine("Código: " + codigo
            + " | Nombre: " + nombreCompleto
            + " | Especialidad: " + especialidad
            + " | Estado: " + disponibilidad
            + " | Trabajos Activos: " + trabajoActivos
            + " | Finalizados: " + serviciosFinalizados
            + " | Promedio: " + calificacionPromedio);
    }
}
