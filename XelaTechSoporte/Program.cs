using System;

namespace XelaTechSoporte;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;   // Para que se vean tildes y caracteres especiales
        Menu menu = new Menu();
        menu.Iniciar();
    }
}