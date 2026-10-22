using System;
using System.Collections.Generic;

namespace XelaTechSoporte;

class Validador
{
    public static int LeerEntero(string mensaje, int minimo, int maximo)
    {
        while (true)
        {
            Console.Write(mensaje);
            string texto = Console.ReadLine();
            int numero;

            bool esValido = int.TryParse(texto, out numero);

            if (!esValido)
            {
                Console.WriteLine("ERROR: Escriba solo números enteros.");
                continue;
            }
            if (numero < minimo || numero > maximo)
            {
                Console.WriteLine("ERROR: Opción inválida. Debe estar entre " + minimo + " y " + maximo + ".");
                continue;
            }
            return numero;
        }
    }

    public static double LeerDoubleNoNegativo(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            string texto = Console.ReadLine();
            double numero;

            bool esValido = double.TryParse(texto, out numero);

            if (!esValido)
            {
                Console.WriteLine("ERROR: Opción inválida.");
                continue;
            }
            if (numero < 0)
            {
                Console.WriteLine("ERROR: No se permiten valores negativos.");
                continue;
            }

            return numero;
        }
    }

    public static string LeerTextoNoVacio(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            string texto = Console.ReadLine();

            if (texto == null || texto.Trim() == "")
            {
                Console.WriteLine("ERROR: No se permiten valores vacíos.");
                continue;
            }
            return texto.Trim();
        }
    }

    public static bool EsCorreoValido(string correo)
    {
        if (correo == null || correo.Trim() == "") return false;

        int cantidadArrobas = 0;
        int posicionArroba = -1;

        for (int i = 0; i < correo.Length; i++)
        {
            if (correo[i] == ' ') return false;
            if (correo[i] == '@')
            {
                cantidadArrobas++;
                posicionArroba = i;
            }

        }
        if (cantidadArrobas != 1 || posicionArroba <= 0) return false;

        bool tienePuntoDespuesArroba = false;
        for (int i = posicionArroba + 1; i < correo.Length; i++)
        {
            if (correo[i] == '.')
            {
                if (i == posicionArroba + 1 || i == correo.Length - 1)
                {
                    return false;
                }
                tienePuntoDespuesArroba = true;
            }

        }
        return tienePuntoDespuesArroba;
    }

    public static bool EsTelefonoValido(string telefono)
    {
        if (telefono == null || telefono.Trim() == "") return false;
        if (telefono.Length != 8) return false;

        for (int i = 0; i < telefono.Length; i++)
        {
            char c = telefono[i];
            if (c < '0' || c > '9') return false;
        }
        return true;
    }
}