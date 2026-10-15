using System;

namespace XelaTechSoporte;

class Menu
{
    public void Iniciar()
    {
        int opcion = 0;
        while (opcion != 11)
        {
            MostrarMenuPrincipal();
            opcion = Validador.LeerEntero("Seleccione una opción: ", 1, 11);
            switch (opcion)
            {
                case 11:
                    Console.WriteLine("Hasta pronto.");
                    break;
                default:
                    Console.WriteLine("Módulo en construcción.");
                    break;
            }
        }
    }

    // Muestra en consola las opciones del menú principal
    private void MostrarMenuPrincipal()
    {
        Console.WriteLine("============================================");
        Console.WriteLine("              XELATECH SOPORTE");
        Console.WriteLine("============================================");
        Console.WriteLine("1. Gestión de clientes");
        Console.WriteLine("2. Gestión de técnicos");
        Console.WriteLine("3. Gestión de equipos");
        Console.WriteLine("4. Gestión de solicitudes");
        Console.WriteLine("5. Gestión de repuestos");
        Console.WriteLine("6. Atención de colas");
        Console.WriteLine("7. Ordenamientos");
        Console.WriteLine("8. Búsquedas");
        Console.WriteLine("9. Reportes");
        Console.WriteLine("10. Generar datos de prueba");
        Console.WriteLine("11. Salir");
        Console.WriteLine("--------------------------------------------");
    }
}