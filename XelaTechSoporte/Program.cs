using System;

namespace XelaTechSoporte;

class Program
{
    static void Main(string[] args)
    {
        CentroServicio centro = new CentroServicio();
        try
        {
            int cargados = AlmacenamientoTxt.Cargar(centro);
            Console.WriteLine("Datos cargados desde XelaTechDatos.txt: " + cargados + " registros.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("No se pudieron cargar los datos guardados: " + ex.Message);
        }

        Menu menu = new Menu(centro);
        menu.Iniciar();
        try
        {
            AlmacenamientoTxt.Guardar(centro);
            Console.WriteLine("Datos guardados correctamente.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("No se pudieron guardar los datos: " + ex.Message);
        }
    }
}
