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
   
    public static int LeerEntero(string mensaje, int minimo, int maximo)
    {
        while (true)
        {
            Console.Write(mensaje);
            string texto = Console.ReadLine();
            int numero;
            try
            {
                numero = int.Parse(texto);
            }
            catch (FormatException)
            {
                Console.WriteLine("ERROR: Escriba solo números enteros.");
                continue;
            }
            catch (OverflowException)
            {
                Console.WriteLine("ERROR: El número es demasiado grande.");
                continue;
            }

            // Validación de rango [minimo, maximo]
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
            try
            {
                numero = double.Parse(texto);
            }
            catch (FormatException)
            {
                Console.WriteLine("ERROR: Escriba un número válido (ejemplo: 12.50).");
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
  
    public static bool EsCorreoValido(string correo)
    {
        if (string.IsNullOrWhiteSpace(correo)) return false;       
        if (correo.Contains(" ")) return false;                    
        int arroba = correo.IndexOf('@');
        if (arroba <= 0) return false;                             
        if (arroba != correo.LastIndexOf('@')) return false;      
        int punto = correo.IndexOf('.', arroba);
        if (punto == -1 || punto == arroba + 1 || punto == correo.Length - 1) return false;
        return true;
    }
   
    public static bool EsTelefonoValido(string telefono)
    {
        if (string.IsNullOrWhiteSpace(telefono)) return false;
        if (telefono.Length != 8) return false;                    
        foreach (char c in telefono)
        {
            if (!char.IsDigit(c)) return false;
        }
        return true;
    }
}