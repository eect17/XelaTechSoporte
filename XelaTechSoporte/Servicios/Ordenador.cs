using System;
using System.Collections.Generic;

namespace XelaTechSoporte;

class Ordenador
{
    public void OrdenarRepuestosPorPrecio(List<Repuesto> repuestos)
    {
        if (repuestos == null || repuestos.Count <= 1) return;

        for (int i = 0; i < repuestos.Count - 1; i++)
        {
            for (int j = 0; j < repuestos.Count - 1 - i; j++)
            {
                if (repuestos[j].PrecioVenta > repuestos[j + 1].PrecioVenta)
                {
                    Repuesto temp = repuestos[j];
                    repuestos[j] = repuestos[j + 1];
                    repuestos[j + 1] = temp;
                }
            }
        }
    }

    public void OrdenarTecnicosPorServicios(List<Tecnico> tecnicos)
    {
        if (tecnicos == null || tecnicos.Count <= 1) return;

        for (int i = 0; i < tecnicos.Count - 1; i++)
        {
            int maxIdx = i;
            for (int j = i + 1; j < tecnicos.Count; j++)
            {
                if (tecnicos[j].ServiciosFinalizados > tecnicos[maxIdx].ServiciosFinalizados)
                {
                    maxIdx = j;
                }
            }
            if (maxIdx != i)
            {
                Tecnico temp = tecnicos[i];
                tecnicos[i] = tecnicos[maxIdx];
                tecnicos[maxIdx] = temp;
            }
        }
    }
}