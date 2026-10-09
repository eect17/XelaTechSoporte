using System;
using System.Collections.Generic;
using System.Text;

namespace XelaTechSoporte;

static class Validador
{
    public static string LeerTextoNoVacio(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            string texto = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(texto))
            {
                Console.WriteLine("ERROR: El texto no puede estar vacío.");
            }
            else
            {
                return texto.Trim();
            }
        }
    }
}