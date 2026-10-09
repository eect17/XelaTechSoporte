namespace XelaTechSoporte;
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("XelaTech Soporte: proyecto listo");
        string nombre = Validador.LeerTextoNoVacio("Nombre: ");
        Console.WriteLine("Hola " + nombre);

    }
}
