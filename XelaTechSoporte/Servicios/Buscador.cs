using System;
using System.Collections.Generic;

namespace XelaTechSoporte;

class Buscador
{
    private CentroServicio centro;

    public Buscador(CentroServicio centro) { this.centro = centro; }

    public Cliente? BuscarCliente(string codigo)
    {
        return centro.Clientes.Find(c => c.Codigo.Equals(codigo, StringComparison.OrdinalIgnoreCase));
    }

    public Tecnico? BuscarTecnico(string codigo)
    {
        return centro.Tecnicos.Find(t => t.Codigo.Equals(codigo, StringComparison.OrdinalIgnoreCase));
    }

    public Repuesto? BuscarRepuesto(string codigo)
    {
        return centro.Repuestos.Find(r => r.Codigo.Equals(codigo, StringComparison.OrdinalIgnoreCase));
    }

    public List<Solicitud> BuscarSolicitudesPorEstado(int estado)
    {
        List<Solicitud> encontradas = new List<Solicitud>();
        foreach (Solicitud s in centro.Solicitudes) if (s.Estado == estado) encontradas.Add(s);
        return encontradas;
    }

    public Solicitud? BuscarSolicitudBinaria(string codigo)
    {
        List<Solicitud> lista = new List<Solicitud>(centro.Solicitudes);
        for (int i = 0; i < lista.Count - 1; i++)
        {
            for (int j = 0; j < lista.Count - i - 1; j++)
            {
                if (string.Compare(lista[j].NumeroSolicitud, lista[j + 1].NumeroSolicitud, StringComparison.OrdinalIgnoreCase) > 0)
                {
                    Solicitud temporal = lista[j]; lista[j] = lista[j + 1]; lista[j + 1] = temporal;
                }
            }
        }
        int inicio = 0, fin = lista.Count - 1;
        while (inicio <= fin)
        {
            int medio = inicio + (fin - inicio) / 2;
            int comparacion = string.Compare(lista[medio].NumeroSolicitud, codigo, StringComparison.OrdinalIgnoreCase);
            if (comparacion == 0) return lista[medio];
            if (comparacion < 0) inicio = medio + 1; else fin = medio - 1;
        }
        return null;
    }
}
