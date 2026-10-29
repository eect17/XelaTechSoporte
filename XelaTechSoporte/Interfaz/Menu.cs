using System;
using System.Collections.Generic;

namespace XelaTechSoporte;

class Menu
{
    private CentroServicio centro;
    private Ordenador ordenador;
    private Buscador buscador;
    private Reportes reportes;
    private Random azar;

    public Menu(CentroServicio centro)
    {
        this.centro = centro;
        ordenador = new Ordenador();
        buscador = new Buscador(centro);
        reportes = new Reportes(centro);
        azar = new Random();
    }

    public void Iniciar()
    {
        int opcion;
        do
        {
            Console.Clear();
            MostrarMenuPrincipal();
            opcion = LeerEntero("Seleccione una opción: ", 1, 11);
            switch (opcion)
            {
                case 1: MenuClientes(); break;
                case 2: MenuTecnicos(); break;
                case 3: MenuEquipos(); break;
                case 4: MenuSolicitudes(); break;
                case 5: MenuRepuestos(); break;
                case 6: MenuColas(); break;
                case 7: MenuOrdenamientos(); break;
                case 8: MenuBusquedas(); break;
                case 9: MenuReportes(); break;
                case 10: GenerarDatosPrueba(); break;
                case 11: Console.WriteLine("Gracias por utilizar XelaTech Soporte."); break;
            }
            if (opcion != 11) Pausa();
        } while (opcion != 11);
    }

    private void MostrarMenuPrincipal()
    {
        Console.WriteLine("================================================");
        Console.WriteLine("              XELATECH SOPORTE");
        Console.WriteLine("================================================");
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
        Console.WriteLine("------------------------------------------------");
    }

    private void MenuClientes()
    {
        int op;
        do
        {
            Console.Clear(); Console.WriteLine("--- GESTIÓN DE CLIENTES ---");
            Console.WriteLine("1. Registrar cliente\n2. Consultar todos\n3. Buscar por código\n4. Actualizar datos\n5. Volver");
            op = LeerEntero("Opción: ", 1, 5);
            if (op == 1)
            {
                try
                {
                    string codigo = LeerTexto("Código: ");
                    string nombre = LeerTexto("Nombre completo: ");
                    string telefono = LeerTexto("Teléfono (mínimo 8 caracteres): ");
                    string correo = LeerTexto("Correo: ");
                    string tipoCliente = ElegirTipoCliente();
                    Cliente c = new Cliente(codigo, nombre, telefono, correo, tipoCliente);
                    Console.WriteLine(centro.RegistrarCliente(c) ? "Cliente registrado." : "Ya existe un cliente con ese código.");
                }
                catch (Exception ex) { Console.WriteLine("No se pudo registrar: " + ex.Message); }
            }
            else if (op == 2) MostrarClientes();
            else if (op == 3)
            {
                Cliente? c = buscador.BuscarCliente(LeerTexto("Código del cliente: "));
                if (c == null) Console.WriteLine("No se encontró el cliente."); else c.MostrarInformacion();
            }
            else if (op == 4)
            {
                Cliente? c = buscador.BuscarCliente(LeerTexto("Código del cliente a actualizar: "));
                if (c == null) Console.WriteLine("No se encontró el cliente.");
                else
                {
                    try
                    {
                        string telefono = LeerTexto("Nuevo teléfono: ");
                        string correo = LeerTexto("Nuevo correo: ");
                        string tipoCliente = ElegirTipoCliente();
                        c.ActualizarDatos(telefono, correo, tipoCliente);
                        Console.WriteLine("Datos actualizados.");
                    }
                    catch (Exception ex) { Console.WriteLine("No se actualizaron los datos: " + ex.Message); }
                }
            }
            if (op != 5) Pausa();
        } while (op != 5);
    }

    private void MenuTecnicos()
    {
        int op;
        do
        {
            Console.Clear(); Console.WriteLine("--- GESTIÓN DE TÉCNICOS ---");
            Console.WriteLine("1. Registrar técnico\n2. Consultar técnicos\n3. Buscar técnico por código\n4. Volver");
            op = LeerEntero("Opción: ", 1, 4);
            if (op == 1)
            {
                try
                {
                    string codigo = LeerTexto("Código: "); string nombre = LeerTexto("Nombre completo: ");
                    Console.WriteLine("Especialidad: 1.Portátil  2.Escritorio  3.Impresora");
                    int esp = LeerEntero("Especialidad: ", 1, 3);
                    int disp = LeerEntero("Disponibilidad (1.Disponible, 2.Carga completa, 3.Fuera de servicio): ", 1, 3);
                    Tecnico t = new Tecnico(codigo, nombre, esp, disp);
                    Console.WriteLine(centro.RegistrarTecnico(t) ? "Técnico registrado." : "Ya existe un técnico con ese código.");
                }
                catch (Exception ex) { Console.WriteLine("No se pudo registrar: " + ex.Message); }
            }
            else if (op == 2)
            {
                if (centro.Tecnicos.Count == 0) Console.WriteLine("No hay técnicos registrados.");
                foreach (Tecnico t in centro.Tecnicos) t.MostrarInformacion();
            }
            else if (op == 3)
            {
                Tecnico? t = buscador.BuscarTecnico(LeerTexto("Código del técnico: "));
                if (t == null) Console.WriteLine("No se encontró el técnico."); else t.MostrarInformacion();
            }
            if (op != 4) Pausa();
        } while (op != 4);
    }

    private void MenuEquipos()
    {
        int op;
        do
        {
            Console.Clear(); Console.WriteLine("--- GESTIÓN DE EQUIPOS ---");
            Console.WriteLine("1. Registrar equipo\n2. Consultar equipos\n3. Volver");
            op = LeerEntero("Opción: ", 1, 3);
            if (op == 1)
            {
                if (centro.Clientes.Count == 0) Console.WriteLine("Primero registre un cliente.");
                else
                {
                    try
                    {
                        string codigo = LeerTexto("Código interno: "); string serie = LeerTexto("Número de serie: ");
                        string marca = LeerTexto("Marca: "); string modelo = LeerTexto("Modelo: ");
                        string codCli = LeerTexto("Código del propietario: ");
                        Cliente? cli = buscador.BuscarCliente(codCli);
                        if (cli == null) Console.WriteLine("El cliente no existe.");
                        else
                        {
                            string estado = LeerTexto("Estado físico recibido: "); string accesorios = LeerTexto("Accesorios entregados (puede escribir Ninguno): ");
                            Console.WriteLine("Tipo: 1.Portátil 2.Escritorio 3.Impresora"); int tipo = LeerEntero("Tipo: ", 1, 3);
                            Equipo equipo;
                            if (tipo == 1) equipo = new ComputadoraPortatil(codigo, serie, marca, modelo, cli, estado, accesorios, LeerSiNo("¿Incluye cargador? (s/n): "));
                            else if (tipo == 2) equipo = new ComputadoraEscritorio(codigo, serie, marca, modelo, cli, estado, accesorios, LeerSiNo("¿Incluye monitor? (s/n): "));
                            else equipo = new Impresora(codigo, serie, marca, modelo, cli, estado, accesorios, LeerSiNo("¿Es multifuncional? (s/n): "));
                            Console.WriteLine(centro.RegistrarEquipo(equipo) ? "Equipo registrado." : "Código interno o número de serie duplicado.");
                        }
                    }
                    catch (Exception ex) { Console.WriteLine("No se pudo registrar: " + ex.Message); }
                }
            }
            else if (op == 2)
            {
                if (centro.Equipos.Count == 0) Console.WriteLine("No hay equipos registrados.");
                foreach (Equipo e in centro.Equipos) e.MostrarInformacion();
            }
            if (op != 3) Pausa();
        } while (op != 3);
    }

    private void MenuSolicitudes()
    {
        int op;
        do
        {
            Console.Clear(); Console.WriteLine("--- GESTIÓN DE SOLICITUDES ---");
            Console.WriteLine("1. Crear solicitud\n2. Consultar solicitudes\n3. Cambiar estado\n4. Registrar diagnóstico\n5. Agregar repuesto a reparación\n6. Registrar mano de obra\n7. Ver historial de estados\n8. Deshacer último cambio de estado\n9. Cancelar solicitud\n10. Calificar servicio entregado\n11. Volver");
            op = LeerEntero("Opción: ", 1, 11);
            if (op == 1) CrearSolicitud();
            else if (op == 2) MostrarSolicitudes();
            else if (op >= 3 && op <= 10) GestionarSolicitud(op);
            if (op != 11) Pausa();
        } while (op != 11);
    }

    private void CrearSolicitud()
    {
        if (centro.Clientes.Count == 0 || centro.Equipos.Count == 0) { Console.WriteLine("Debe registrar al menos un cliente y un equipo primero."); return; }
        try
        {
            string codigo = LeerTexto("Código de solicitud: ");
            Cliente? cli = buscador.BuscarCliente(LeerTexto("Código del cliente: "));
            if (cli == null) { Console.WriteLine("El cliente no existe."); return; }
            string codigoEquipo = LeerTexto("Código interno del equipo: ");
            Equipo? equipo = centro.Equipos.Find(e => e.CodigoInterno.Equals(codigoEquipo, StringComparison.OrdinalIgnoreCase));
            if (equipo == null || equipo.Propietario.Codigo != cli.Codigo) { Console.WriteLine("El equipo no existe o no pertenece a ese cliente."); return; }
            string motivo = LeerTexto("Descripción del problema: ");
            int prioridad = LeerEntero("Prioridad (1.Normal, 2.Urgente): ", 1, 2);
            Solicitud s = new Solicitud(codigo, cli, equipo, motivo, prioridad, DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            Console.WriteLine(centro.RegistrarSolicitud(s) ? "Solicitud registrada y colocada en la cola correspondiente." : "Código duplicado o el equipo ya tiene una solicitud activa.");
        }
        catch (Exception ex) { Console.WriteLine("No se pudo crear la solicitud: " + ex.Message); }
    }

    private void GestionarSolicitud(int op)
    {
        string codigo = LeerTexto("Código de solicitud: ");
        Solicitud? s = centro.Solicitudes.Find(x => x.NumeroSolicitud.Equals(codigo, StringComparison.OrdinalIgnoreCase));
        if (s == null) { Console.WriteLine("No se encontró la solicitud."); return; }
        if (op == 3)
        {
            Console.WriteLine("Estado actual: " + s.Estado + ". " + Solicitud.NombreEstado(s.Estado));
            Console.WriteLine("Estados: 1.Recibida 2.En espera 3.Asignada 4.En diagnóstico 5.En reparación 6.Lista para entregar 7.Entregada 8.Cancelada");
            int nuevo = LeerEntero("Nuevo estado: ", 1, 8);
            if (nuevo == 3 && s.TecnicoAsignado == null)
            {
                Tecnico? tec = ElegirTecnico(s.Equipo.ObtenerEspecialidadRequerida());
                if (tec == null) { Console.WriteLine("No hay técnico disponible con la especialidad requerida."); return; }
                try
                {
                    Console.WriteLine(centro.AsignarSolicitud(s, tec, -1) ? "Técnico asignado y estado actualizado." : "No se pudo asignar; revise especialidad, disponibilidad y estado.");
                }
                catch (Exception ex) { Console.WriteLine("No se pudo asignar: " + ex.Message); }
            }
            else
            {
                int estadoAnterior = s.Estado;
                if (s.CambiarEstado(nuevo))
                {
                    if (nuevo == 7 && s.TecnicoAsignado != null) s.TecnicoAsignado.FinalizarTrabajo();
                    Console.WriteLine("Estado actualizado a: " + Solicitud.NombreEstado(nuevo));
                }
                else Console.WriteLine("Cambio rechazado. Avance de uno en uno, registre técnico/diagnóstico y no modifique estados finales.");
            }
        }
        else if (op == 4)
        {
            if (s.Estado != 4) Console.WriteLine("Primero cambie la solicitud a En diagnóstico.");
            else Console.WriteLine(s.RegistrarDiagnostico(LeerTexto("Diagnóstico: ")) ? "Diagnóstico registrado." : "El diagnóstico no puede estar vacío.");
        }
        else if (op == 5)
        {
            if (s.Estado != 5) { Console.WriteLine("La solicitud debe estar En reparación."); return; }
            foreach (Repuesto r in centro.Repuestos) r.MostrarInformacion();
            Repuesto? rep = buscador.BuscarRepuesto(LeerTexto("Código del repuesto: "));
            if (rep == null) { Console.WriteLine("No existe el repuesto."); return; }
            int cantidad = LeerEntero("Cantidad a utilizar: ", 1, Math.Max(1, rep.Existencia));
            try { Console.WriteLine(s.AgregarRepuesto(rep, cantidad) ? "Repuesto agregado y existencia descontada." : "No se pudo utilizar el repuesto."); }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
        else if (op == 6)
        {
            double adicional = LeerDouble("Costo adicional de mano de obra (Q): ", 0);
            Console.WriteLine(s.RegistrarManoObra(adicional) ? "Mano de obra registrada. Total: Q" + s.CalcularTotal().ToString("F2") : "Debe estar En reparación y el costo no puede ser negativo.");
        }
        else if (op == 7) s.ConsultarHistorial();
        else if (op == 8) Console.WriteLine(s.DeshacerEstado() ? "Se deshizo el último cambio." : "No se puede deshacer: historial vacío o solicitud finalizada/cancelada.");
        else if (op == 9)
        {
            bool cancelada = s.Cancelar();
            if (cancelada && s.TecnicoAsignado != null && s.Estado == 8) s.TecnicoAsignado.LiberarTrabajo();
            Console.WriteLine(cancelada ? "Solicitud cancelada." : "No se pudo cancelar una solicitud entregada o cancelada.");
        }
        else if (op == 10)
        {
            int calificacion = LeerEntero("Calificación del servicio (1 a 5): ", 1, 5);
            bool ok = s.RegistrarCalificacion(calificacion);
            if (ok && s.TecnicoAsignado != null) s.TecnicoAsignado.RegistrarCalificacion(calificacion);
            Console.WriteLine(ok ? "Calificación registrada." : "Solo puede calificar una solicitud entregada y una sola vez.");
        }
    }

    private Tecnico? ElegirTecnico(int especialidad)
    {
        foreach (Tecnico t in centro.Tecnicos)
            if (t.PuedeRecibir(especialidad)) { Console.WriteLine("Técnico sugerido: " + t.Codigo + " - " + t.NombreCompleto); return t; }
        return null;
    }

    private void MenuRepuestos()
    {
        int op;
        do
        {
            Console.Clear(); Console.WriteLine("--- GESTIÓN DE REPUESTOS ---");
            Console.WriteLine("1. Registrar repuesto\n2. Consultar inventario\n3. Reponer existencia\n4. Alertas de existencia mínima\n5. Volver");
            op = LeerEntero("Opción: ", 1, 5);
            if (op == 1)
            {
                try
                {
                    Repuesto r = new Repuesto(LeerTexto("Código: "), LeerTexto("Nombre: "), LeerTexto("Categoría: "), LeerEntero("Existencia: ", 0, 100000), LeerEntero("Existencia mínima: ", 0, 100000), LeerDouble("Costo unitario Q: ", 0), LeerDouble("Precio de venta Q: ", 0));
                    Console.WriteLine(centro.RegistrarRepuesto(r) ? "Repuesto registrado." : "Código de repuesto duplicado.");
                }
                catch (Exception ex) { Console.WriteLine("No se pudo registrar: " + ex.Message); }
            }
            else if (op == 2) { if (centro.Repuestos.Count == 0) Console.WriteLine("No hay repuestos."); foreach (Repuesto r in centro.Repuestos) r.MostrarInformacion(); }
            else if (op == 3)
            {
                Repuesto? r = buscador.BuscarRepuesto(LeerTexto("Código del repuesto: "));
                if (r == null) Console.WriteLine("No existe el repuesto."); else { int cantidad = LeerEntero("Cantidad a reponer: ", 1, 100000); r.Reponer(cantidad); Console.WriteLine("Existencia actualizada: " + r.Existencia); }
            }
            else if (op == 4) reportes.MostrarRepuestosMinimos();
            if (op != 5) Pausa();
        } while (op != 5);
    }

    private void MenuColas()
    {
        int op;
        do
        {
            Console.Clear(); Console.WriteLine("--- ATENCIÓN DE COLAS ---");
            Console.WriteLine("1. Ver colas\n2. Atender siguiente (urgentes antes que normales)\n3. Asignar solicitud en espera a técnico\n4. Volver");
            op = LeerEntero("Opción: ", 1, 4);
            if (op == 1) { reportes.Reporte1_SolicitudesEnEspera(); }
            else if (op == 2) { if (!centro.AtenderSiguiente()) Console.WriteLine("No fue posible atender una solicitud."); }
            else if (op == 3)
            {
                MostrarSolicitudes();
                string codigoSolicitud = LeerTexto("Código de solicitud: ");
                Solicitud? s = centro.Solicitudes.Find(x => x.NumeroSolicitud.Equals(codigoSolicitud, StringComparison.OrdinalIgnoreCase));
                if (s == null || (s.Estado != 1 && s.Estado != 2)) Console.WriteLine("La solicitud no existe o ya no está en espera.");
                else
                {
                    Tecnico? t = ElegirTecnico(s.Equipo.ObtenerEspecialidadRequerida());
                    if (t == null) Console.WriteLine("No hay técnico disponible para esa especialidad.");
                    else Console.WriteLine(centro.AsignarSolicitud(s, t, -1) ? "Solicitud asignada." : "No se pudo asignar.");
                }
            }
            if (op != 4) Pausa();
        } while (op != 4);
    }

    private void MenuOrdenamientos()
    {
        int op;
        do
        {
            Console.Clear(); Console.WriteLine("--- ORDENAMIENTOS MANUALES ---");
            Console.WriteLine("1. Solicitudes por costo total (Bubble Sort ascendente)\n2. Técnicos por servicios finalizados (Selection Sort descendente)\n3. Repuestos por existencia (Bubble Sort ascendente)\n4. Volver");
            op = LeerEntero("Opción: ", 1, 4);
            if (op == 1)
            {
                if (centro.Solicitudes.Count < 2) Console.WriteLine("Se necesitan al menos dos solicitudes.");
                else { List<Solicitud> copia = new List<Solicitud>(centro.Solicitudes); ordenador.OrdenarSolicitudesPorCostoTotal(copia); Console.WriteLine("Bubble Sort | costo total | ascendente"); foreach (Solicitud s in copia) Console.WriteLine(s.NumeroSolicitud + " - Q" + s.CalcularTotal().ToString("F2")); }
            }
            else if (op == 2)
            {
                if (centro.Tecnicos.Count < 2) Console.WriteLine("Se necesitan al menos dos técnicos.");
                else { List<Tecnico> copia = new List<Tecnico>(centro.Tecnicos); ordenador.OrdenarTecnicosPorServicios(copia); Console.WriteLine("Selection Sort | servicios finalizados | descendente"); foreach (Tecnico t in copia) t.MostrarInformacion(); }
            }
            else if (op == 3)
            {
                if (centro.Repuestos.Count < 2) Console.WriteLine("Se necesitan al menos dos repuestos.");
                else { List<Repuesto> copia = new List<Repuesto>(centro.Repuestos); ordenador.OrdenarRepuestosPorExistencia(copia); Console.WriteLine("Bubble Sort | existencia | ascendente"); foreach (Repuesto r in copia) r.MostrarInformacion(); }
            }
            if (op != 4) Pausa();
        } while (op != 4);
    }

    private void MenuBusquedas()
    {
        int op;
        do
        {
            Console.Clear(); Console.WriteLine("--- BÚSQUEDAS ---");
            Console.WriteLine("1. Secuencial: clientes por nombre parcial\n2. Secuencial: solicitudes por estado\n3. Secuencial: equipos por marca\n4. Binaria: solicitud por código\n5. Volver");
            op = LeerEntero("Opción: ", 1, 5);
            if (op == 1)
            {
                string nombre = LeerTexto("Nombre o parte del nombre: "); int encontrados = 0;
                foreach (Cliente c in centro.Clientes) if (c.NombreCompleto.IndexOf(nombre, StringComparison.OrdinalIgnoreCase) >= 0) { c.MostrarInformacion(); encontrados++; }
                Console.WriteLine("Coincidencias: " + encontrados);
            }
            else if (op == 2)
            {
                int estado = LeerEntero("Estado (1 a 8): ", 1, 8); List<Solicitud> lista = buscador.BuscarSolicitudesPorEstado(estado);
                foreach (Solicitud s in lista) s.MostrarInformacion(); Console.WriteLine("Coincidencias: " + lista.Count);
            }
            else if (op == 3)
            {
                string marca = LeerTexto("Marca o parte de la marca: "); int encontrados = 0;
                foreach (Equipo e in centro.Equipos) if (e.Marca.IndexOf(marca, StringComparison.OrdinalIgnoreCase) >= 0) { e.MostrarInformacion(); encontrados++; }
                Console.WriteLine("Coincidencias: " + encontrados);
            }
            else if (op == 4)
            {
                if (centro.Solicitudes.Count == 0) Console.WriteLine("No hay solicitudes.");
                else { string codigo = LeerTexto("Código de solicitud: "); Solicitud? s = buscador.BuscarSolicitudBinaria(codigo); if (s == null) Console.WriteLine("No se encontró la solicitud."); else s.MostrarInformacion(); }
            }
            if (op != 5) Pausa();
        } while (op != 5);
    }

    private void MenuReportes()
    {
        int op;
        do
        {
            Console.Clear(); Console.WriteLine("--- REPORTES ---");
            Console.WriteLine("1. Solicitudes en espera por prioridad\n2. Solicitudes activas, entregadas y canceladas\n3. Estado de técnicos\n4. Técnico con más servicios finalizados\n5. Equipos atendidos por tipo\n6. Solicitudes por costo\n7. Solicitudes por tiempo\n8. Repuestos por existencia\n9. Repuestos en mínimo o agotados\n10. Ingresos por servicios entregados\n11. Resultados de búsquedas\n12. Matriz semanal de carga\n13. Volver");
            op = LeerEntero("Opción: ", 1, 13);
            switch (op)
            {
                case 1: reportes.Reporte1_SolicitudesEnEspera(); break;
                case 2: reportes.Reporte2_EstadoSolicitudes(); break;
                case 3: reportes.Reporte3_EstadoTecnicos(); break;
                case 4: reportes.Reporte4_TecnicoTopServicios(); break;
                case 5: reportes.Reporte5_EquiposPorTipo(); break;
                case 6: reportes.Reporte6_OrdenadasPorCosto(); break;
                case 7: reportes.Reporte7_OrdenadasPorTiempo(); break;
                case 8: reportes.Reporte8_RepuestosPorExistencia(); break;
                case 9: reportes.MostrarRepuestosMinimos(); break;
                case 10: reportes.Reporte10_IngresosTotales(); break;
                case 11: reportes.Reporte11_ResultadosBusquedas(); break;
                case 12: reportes.MostrarMatrizCarga(); break;
            }
            if (op != 13) Pausa();
        } while (op != 13);
    }

    private void GenerarDatosPrueba()
    {
        int antes = centro.Clientes.Count;
        for (int i = 1; i <= 3; i++)
        {
            string cod = "CLI-R" + azar.Next(1000, 9999);
            if (buscador.BuscarCliente(cod) == null)
                centro.RegistrarCliente(new Cliente(cod, "Cliente Prueba " + i, "5555" + azar.Next(1000, 9999), "cliente" + cod.ToLower() + "@prueba.com", "Particular"));
        }
        for (int i = 1; i <= 3; i++)
        {
            string cod = "TEC-R" + azar.Next(1000, 9999);
            if (buscador.BuscarTecnico(cod) == null) centro.RegistrarTecnico(new Tecnico(cod, "Técnico Prueba " + i, i, 1));
        }
        for (int i = 1; i <= 3; i++)
        {
            string cod = "REP-R" + azar.Next(1000, 9999);
            if (buscador.BuscarRepuesto(cod) == null) centro.RegistrarRepuesto(new Repuesto(cod, "Repuesto Prueba " + i, "General", azar.Next(5, 20), 3, 10, 15));
        }
        if (centro.Clientes.Count > 0)
        {
            Cliente clientePrueba = centro.Clientes[centro.Clientes.Count - 1];
            string codEquipo = CodigoDisponible("EQ-R", centro.Equipos.ConvertAll(e => e.CodigoInterno));
            string serie = CodigoDisponible("SER-R", centro.Equipos.ConvertAll(e => e.NumeroSerie));
            Equipo equipoPrueba = new ComputadoraPortatil(codEquipo, serie, "Marca prueba", "Modelo prueba", clientePrueba, "Bueno", "Cargador", true);
            if (centro.RegistrarEquipo(equipoPrueba))
            {
                string codSolicitud = CodigoDisponible("SOL-R", centro.Solicitudes.ConvertAll(s => s.NumeroSolicitud));
                Solicitud solicitudPrueba = new Solicitud(codSolicitud, clientePrueba, equipoPrueba, "Falla generada para pruebas", azar.Next(1, 3), DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                centro.RegistrarSolicitud(solicitudPrueba);
            }
        }
        Console.WriteLine("Datos de prueba generados con Random. Consulte clientes, técnicos, equipos, repuestos y solicitudes en sus módulos.");
        Console.WriteLine("Clientes actuales: " + centro.Clientes.Count + " | Técnicos: " + centro.Tecnicos.Count + " | Equipos: " + centro.Equipos.Count + " | Solicitudes: " + centro.Solicitudes.Count + " | Repuestos: " + centro.Repuestos.Count + " | Clientes agregados en esta ejecución: " + (centro.Clientes.Count - antes));
    }

    private string CodigoDisponible(string prefijo, List<string> existentes)
    {
        string codigo;
        do { codigo = prefijo + azar.Next(1000, 9999); }
        while (existentes.Exists(x => x.Equals(codigo, StringComparison.OrdinalIgnoreCase)));
        return codigo;
    }

    private void MostrarClientes()
    {
        if (centro.Clientes.Count == 0) Console.WriteLine("No hay clientes registrados.");
        foreach (Cliente c in centro.Clientes) c.MostrarInformacion();
    }

    private void MostrarSolicitudes()
    {
        if (centro.Solicitudes.Count == 0) Console.WriteLine("No hay solicitudes registradas.");
        foreach (Solicitud s in centro.Solicitudes) s.MostrarInformacion();
    }

    private string LeerTexto(string mensaje)
    {
        string valor;
        do { Console.Write(mensaje); valor = Console.ReadLine() ?? ""; if (!string.IsNullOrWhiteSpace(valor)) return valor.Trim(); Console.WriteLine("El campo es obligatorio."); } while (true);
    }

    private int LeerEntero(string mensaje, int min, int max)
    {
        int valor;
        do { Console.Write(mensaje); if (int.TryParse(Console.ReadLine(), out valor) && valor >= min && valor <= max) return valor; Console.WriteLine("Ingrese un número entre " + min + " y " + max + "."); } while (true);
    }

    private double LeerDouble(string mensaje, double min)
    {
        double valor;
        do { Console.Write(mensaje); string entrada = (Console.ReadLine() ?? "").Replace(',', '.'); if (double.TryParse(entrada, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out valor) && valor >= min) return valor; Console.WriteLine("Ingrese un valor válido mayor o igual a " + min + "."); } while (true);
    }

    private bool LeerSiNo(string mensaje)
    {
        string respuesta;
        do { Console.Write(mensaje); respuesta = (Console.ReadLine() ?? "").Trim().ToLower(); if (respuesta == "s" || respuesta == "si" || respuesta == "sí") return true; if (respuesta == "n" || respuesta == "no") return false; Console.WriteLine("Responda s o n."); } while (true);
    }

    private string ElegirTipoCliente()
    {
        Console.WriteLine("Tipo de cliente:");
        Console.WriteLine("1. Particular");
        Console.WriteLine("2. Estudiante");
        Console.WriteLine("3. Empresa");
        Console.WriteLine("4. Institución educativa");
        int opcion = LeerEntero("Seleccione el tipo de cliente (1-4): ", 1, 4);

        string tipoSeleccionado;
        switch (opcion)
        {
            case 1: tipoSeleccionado = "Particular"; break;
            case 2: tipoSeleccionado = "Estudiante"; break;
            case 3: tipoSeleccionado = "Empresa"; break;
            default: tipoSeleccionado = "Institución educativa"; break;
        }
        Console.WriteLine("Tipo seleccionado: " + tipoSeleccionado);
        return tipoSeleccionado;
    }

    private void Pausa()
    {
        // Guardamos los cambios después de cada operación del menú.
        try
        {
            AlmacenamientoTxt.Guardar(centro);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Advertencia: no se pudieron guardar los datos: " + ex.Message);
        }
        Console.WriteLine("\nPresione ENTER para continuar...");
        Console.ReadLine();
    }
}
