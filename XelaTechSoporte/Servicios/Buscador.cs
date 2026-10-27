using System;
using System.Collections.Generic;

namespace XelaTechSoporte;

class Buscador
{
    private CentroServicio centro;

    public Buscador(CentroServicio centro)
    {
        this.centro = centro;
    }

    public List<Solicitud> BuscarSolicitudesPorEstado(int estadoBuscado)
    {
        List<Solicitud> resultados = new List<Solicitud>();
        List<Solicitud> todas = centro.Solicitudes;

        for (int i = 0; i < todas.Count; i++)
        {
            if (todas[i].Estado == estadoBuscado)
            {
                resultados.Add(todas[i]);
            }
        }
        return resultados;
    }

    public List<Solicitud> BuscarSolicitudesPorTecnico(string codigoTecnico)
    {
        List<Solicitud> resultados = new List<Solicitud>();
        if (string.IsNullOrWhiteSpace(codigoTecnico)) return resultados;

        List<Solicitud> todas = centro.Solicitudes;
        for (int i = 0; i < todas.Count; i++)
        {
            if (todas[i].TecnicoAsignado != null && todas[i].TecnicoAsignado.Codigo == codigoTecnico.Trim())
            {
                resultados.Add(todas[i]);
            }
        }
        return resultados;
    }

    public Solicitud BusquedaBinariaSolicitudPorNumero(List<Solicitud> listaOrdenada, string numeroBuscado)
    {
        if (listaOrdenada == null || listaOrdenada.Count == 0 || string.IsNullOrWhiteSpace(numeroBuscado))
            return null;

        int inicio = 0;
        int fin = listaOrdenada.Count - 1;
        string busqueda = numeroBuscado.Trim();

        while (inicio <= fin)
        {
            int medio = inicio + (fin - inicio) / 2;
            string actual = listaOrdenada[medio].NumeroSolicitud;

            int comparacion = string.Compare(actual, busqueda, StringComparison.OrdinalIgnoreCase);

            if (comparacion == 0)
            {
                return listaOrdenada[medio];
            }
            else if (comparacion < 0)
            {
                inicio = medio + 1;
            }
            else
            {
                fin = medio - 1;
            }
        }
        return null;
    }
}