using System;

namespace XelaTechSoporte;

public static class Validador
{
    public static bool EsTelefonoValido(string telefono)
    {
        return !string.IsNullOrWhiteSpace(telefono) && telefono.Trim().Length >= 8;
    }

    public static bool EsCorreoValido(string correo)
    {
        return !string.IsNullOrWhiteSpace(correo) && correo.Contains("@") && correo.Contains(".");
    }

    public static int LeerEntero(string mensaje, int min, int max)
    {
        int numero;
        do
        {
            Console.Write(mensaje);
            string? entrada = Console.ReadLine();
            if (int.TryParse(entrada, out numero) && numero >= min && numero <= max)
            {
                return numero;
            }
            Console.WriteLine($"[Error] Ingrese un número válido entre {min} y {max}.");
        } while (true);
    }
}