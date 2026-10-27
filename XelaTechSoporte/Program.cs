using System;
using System.Collections.Generic;
using System.IO;

namespace XelaTechSoporte;

class Program
{
    static void Main(string[] args)
    {
        CentroServicio centro = new CentroServicio();
        Buscador buscador = new Buscador(centro);
        Ordenador ordenador = new Ordenador();

        // ========================================================
        // CARGA AUTOMÁTICA DESDE "datos.txt" AL INICIAR
        // ========================================================
        string rutaArchivo = "datos.txt";
        if (File.Exists(rutaArchivo))
        {
            try
            {
                string[] lineas = File.ReadAllLines(rutaArchivo);
                string seccionActual = "";

                foreach (string linea in lineas)
                {
                    string l = linea.Trim();
                    if (string.IsNullOrEmpty(l)) continue;

                    if (l.StartsWith("["))
                    {
                        seccionActual = l;
                        continue;
                    }

                    string[] partes = l.Split('|');

                    if (seccionActual == "[CLIENTES]" && partes.Length >= 4)
                    {
                        Cliente cli = new Cliente(partes[0], partes[1], partes[2], partes[3]);
                        centro.RegistrarCliente(cli);
                    }
                    else if (seccionActual == "[TECNICOS]" && partes.Length >= 3)
                    {
                        int.TryParse(partes[2], out int nivel);
                        Tecnico tec = new Tecnico(partes[0], partes[1], nivel);
                        centro.RegistrarTecnico(tec);
                    }
                    else if (seccionActual == "[REPUESTOS]" && partes.Length >= 7)
                    {
                        int.TryParse(partes[3], out int ex);
                        int.TryParse(partes[4], out int exMin);
                        double.TryParse(partes[5], out double costo);
                        double.TryParse(partes[6], out double precio);

                        Repuesto rep = new Repuesto(partes[0], partes[1], partes[2], ex, exMin, costo, precio);
                        centro.RegistrarRepuesto(rep);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Aviso al leer datos.txt: {ex.Message}");
            }
        }
        // ========================================================

        int opcion = 0;

        do
        {
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine("      SISTEMA DE SOPORTE - XELATECH     ");
            Console.WriteLine("========================================");
            Console.WriteLine("1. Registrar Cliente");
            Console.WriteLine("2. Registrar Técnico");
            Console.WriteLine("3. Registrar Repuesto");
            Console.WriteLine("4. Registrar Equipo y Solicitud");
            Console.WriteLine("5. Ver Colas de Atención");
            Console.WriteLine("6. Ordenar Repuestos por Precio de Venta");
            Console.WriteLine("7. Salir");
            Console.WriteLine("========================================");
            Console.Write("Seleccione una opción: ");

            string? entrada = Console.ReadLine();

            if (int.TryParse(entrada, out opcion))
            {
                switch (opcion)
                {
                    case 1:
                        Console.Clear();
                        Console.WriteLine("--- Módulo de Registro de Clientes ---");
                        Console.Write("Ingrese Código del cliente: ");
                        string codCliente = Console.ReadLine() ?? "";
                        Console.Write("Ingrese Nombre completo: ");
                        string nombreCliente = Console.ReadLine() ?? "";
                        Console.Write("Ingrese Teléfono: ");
                        string telCliente = Console.ReadLine() ?? "";
                        Console.Write("Ingrese Correo electrónico: ");
                        string correoCliente = Console.ReadLine() ?? "";

                        Cliente nuevoCli = new Cliente(codCliente, nombreCliente, telCliente, correoCliente);
                        centro.RegistrarCliente(nuevoCli);

                        File.AppendAllText("datos.txt", $"[CLIENTES]\n{codCliente}|{nombreCliente}|{telCliente}|{correoCliente}\n");

                        Console.WriteLine("\n¡Cliente registrado y guardado con éxito!");
                        break;

                    case 2:
                        Console.Clear();
                        Console.WriteLine("--- Módulo de Registro de Técnicos ---");
                        Console.Write("Ingrese Código del técnico: ");
                        string codTec = Console.ReadLine() ?? "";
                        Console.Write("Ingrese Nombre completo: ");
                        string nomTec = Console.ReadLine() ?? "";
                        Console.Write("Ingrese Nivel / Años de experiencia (número): ");
                        int.TryParse(Console.ReadLine(), out int nivelTec);

                        Tecnico nuevoTec = new Tecnico(codTec, nomTec, nivelTec);
                        centro.RegistrarTecnico(nuevoTec);

                        File.AppendAllText("datos.txt", $"[TECNICOS]\n{codTec}|{nomTec}|{nivelTec}\n");

                        Console.WriteLine("\n¡Técnico registrado y guardado con éxito!");
                        break;

                    case 3:
                        Console.Clear();
                        Console.WriteLine("--- Módulo de Registro de Repuestos ---");
                        Console.Write("Ingrese Código del repuesto: ");
                        string codRep = Console.ReadLine() ?? "";
                        Console.Write("Ingrese Nombre del repuesto: ");
                        string nomRep = Console.ReadLine() ?? "";
                        Console.Write("Ingrese Categoría: ");
                        string catRep = Console.ReadLine() ?? "";
                        Console.Write("Ingrese Existencia (cantidad): ");
                        int.TryParse(Console.ReadLine(), out int exRep);
                        Console.Write("Ingrese Existencia Mínima: ");
                        int.TryParse(Console.ReadLine(), out int exMinRep);
                        Console.Write("Ingrese Costo Unitario: ");
                        double.TryParse(Console.ReadLine(), out double costoRep);
                        Console.Write("Ingrese Precio de Venta: ");
                        double.TryParse(Console.ReadLine(), out double precioRep);

                        Repuesto nuevoRep = new Repuesto(codRep, nomRep, catRep, exRep, exMinRep, costoRep, precioRep);
                        centro.RegistrarRepuesto(nuevoRep);

                        File.AppendAllText("datos.txt", $"[REPUESTOS]\n{codRep}|{nomRep}|{catRep}|{exRep}|{exMinRep}|{costoRep}|{precioRep}\n");

                        Console.WriteLine("\n¡Repuesto registrado y guardado con éxito!");
                        break;

                    case 4:
                        Console.Clear();
                        Console.WriteLine("--- Módulo de Registro de Equipos y Solicitudes ---");

                        Console.Write("Ingrese Código de la solicitud: ");
                        string codSol = Console.ReadLine() ?? "";
                        Console.Write("Ingrese Código del cliente solicitante: ");
                        string codCliSol = Console.ReadLine() ?? "";

                        Cliente? clienteEncontrado = null;
                        for (int i = 0; i < centro.Clientes.Count; i++)
                        {
                            if (centro.Clientes[i].Codigo == codCliSol)
                            {
                                clienteEncontrado = centro.Clientes[i];
                                break;
                            }
                        }

                        if (clienteEncontrado == null)
                        {
                            Console.WriteLine("\n[Error] Cliente no encontrado. Por favor regístrese primero.");
                            break;
                        }

                        Console.WriteLine("\n--- Tipo de Equipo ---");
                        Console.WriteLine("1. Computadora Portátil");
                        Console.WriteLine("2. Computadora de Escritorio");
                        Console.WriteLine("3. Impresora");
                        Console.Write("Seleccione el tipo de equipo: ");
                        string tipoEq = Console.ReadLine() ?? "";

                        Console.Write("Ingrese Código del equipo: ");
                        string codEq = Console.ReadLine() ?? "";
                        Console.Write("Ingrese Marca: ");
                        string marcaEq = Console.ReadLine() ?? "";
                        Console.Write("Ingrese Modelo: ");
                        string modeloEq = Console.ReadLine() ?? "";
                        Console.Write("Ingrese Estado físico (ej. Bueno, Regular): ");
                        string estadoFisico = Console.ReadLine() ?? "";
                        Console.Write("Ingrese Descripción del problema: ");
                        string descProblema = Console.ReadLine() ?? "";

                        Equipo equipoNuevo;
                        if (tipoEq == "1")
                        {
                            equipoNuevo = new ComputadoraPortatil(codEq, marcaEq, modeloEq, estadoFisico, clienteEncontrado, "", "", false);
                            Solicitud nuevaSol = new Solicitud(codSol, clienteEncontrado, equipoNuevo, descProblema);
                            centro.RegistrarSolicitud(nuevaSol);
                            Console.WriteLine("\n¡Solicitud de Portátil registrada y encolada con éxito!");
                        }
                        else if (tipoEq == "2")
                        {
                            equipoNuevo = new ComputadoraEscritorio(codEq, marcaEq, modeloEq, estadoFisico, clienteEncontrado, "", "", false);
                            Solicitud nuevaSol = new Solicitud(codSol, clienteEncontrado, equipoNuevo, descProblema);
                            centro.RegistrarSolicitud(nuevaSol);
                            Console.WriteLine("\n¡Solicitud de Escritorio registrada y encolada con éxito!");
                        }
                        else if (tipoEq == "3")
                        {
                            equipoNuevo = new Impresora(codEq, marcaEq, modeloEq, estadoFisico, clienteEncontrado, "", "", false);
                            Solicitud nuevaSol = new Solicitud(codSol, clienteEncontrado, equipoNuevo, descProblema);
                            centro.RegistrarSolicitud(nuevaSol);
                            Console.WriteLine("\n¡Solicitud de Impresora registrada y encolada con éxito!");
                        }
                        else
                        {
                            Console.WriteLine("\n[Error] Tipo de equipo no válido.");
                        }
                        break;

                    case 5:
                        Console.Clear();
                        Console.WriteLine("--- Visualización de Colas de Atención ---");
                        Console.WriteLine($"Portátiles en cola: {centro.ColaPortatiles.Count}");
                        Console.WriteLine($"Escritorio en cola: {centro.ColaEscritorio.Count}");
                        Console.WriteLine($"Impresoras en cola: {centro.ColaImpresoras.Count}");
                        break;

                    case 6:
                        Console.Clear();
                        Console.WriteLine("--- Ordenamiento de Repuestos por Precio ---");
                        if (centro.Repuestos.Count == 0)
                        {
                            Console.WriteLine("No hay repuestos registrados para ordenar.");
                        }
                        else
                        {
                            ordenador.OrdenarRepuestosPorPrecio(centro.Repuestos);
                            Console.WriteLine("¡Repuestos ordenados de menor a mayor precio con éxito!\n");
                            for (int i = 0; i < centro.Repuestos.Count; i++)
                            {
                                Console.WriteLine($"- {centro.Repuestos[i].Nombre}: Q{centro.Repuestos[i].PrecioVenta}");
                            }
                        }
                        break;

                    case 7:
                        Console.WriteLine("\n¡Gracias por usar XelaTech Soporte!");
                        break;

                    default:
                        Console.WriteLine("\nOpción no válida. Intente de nuevo.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("\nPor favor, ingrese un número válido.");
            }

            if (opcion != 7)
            {
                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
            }

        } while (opcion != 7);
    }
}