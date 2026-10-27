using System;
using System.Collections.Generic;

namespace XelaTechSoporte;

class Reportes
{
    private CentroServicio centro;

    public Reportes(CentroServicio centro)
    {
        this.centro = centro;
    }

    public void MostrarMatrizCarga()
    {
        Console.WriteLine("--- Matriz Semanal de Carga de Trabajo ---");
        Console.WriteLine("Técnico          | Lun | Mar | Mié | Jue | Vie ");
        Console.WriteLine("--------------------------------------------------");

        int[,] matriz = centro.MatrizCargaSemanal;
        for (int i = 0; i < centro.Tecnicos.Count && i < 10; i++)
        {
            string nombreTec = centro.Tecnicos[i].NombreCompleto;
            if (nombreTec.Length > 15) nombreTec = nombreTec.Substring(0, 15);

            Console.Write($"{nombreTec,-16} | ");
            for (int j = 0; j < 5; j++)
            {
                Console.Write($"{matriz[i, j],3} | ");
            }
            Console.WriteLine();
        }
    }

    public void MostrarRepuestosMinimos()
    {
        Console.WriteLine("--- Repuestos con Existencia Mínima o Agotados ---");
        bool hayAlertas = false;
        for (int i = 0; i < centro.Repuestos.Count; i++)
        {
            Repuesto r = centro.Repuestos[i];
            if (r.EstaEnMinimo() || r.EstaAgotado())
            {
                Console.WriteLine($"- [{r.Codigo}] {r.Nombre} | Stock: {r.Existencia} | Mínimo: {r.ExistenciaMinima}");
                hayAlertas = true;
            }
        }
        if (!hayAlertas)
        {
            Console.WriteLine("No hay repuestos en nivel crítico.");
        }
    }
}