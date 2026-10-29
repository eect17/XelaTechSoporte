using System.Collections.Generic;

namespace XelaTechSoporte;

class Ordenador
{
    public void OrdenarSolicitudesPorCostoTotal(List<Solicitud> lista)
    {
        for (int i = 0; i < lista.Count - 1; i++)
            for (int j = 0; j < lista.Count - i - 1; j++)
                if (lista[j].CalcularTotal() > lista[j + 1].CalcularTotal())
                {
                    Solicitud temp = lista[j]; lista[j] = lista[j + 1]; lista[j + 1] = temp;
                }
    }
    public void OrdenarTecnicosPorServicios(List<Tecnico> lista)
    {
        for (int i = 0; i < lista.Count - 1; i++)
        {
            int mayor = i;
            for (int j = i + 1; j < lista.Count; j++)
                if (lista[j].ServiciosFinalizados > lista[mayor].ServiciosFinalizados) mayor = j;
            if (mayor != i) { Tecnico temp = lista[i]; lista[i] = lista[mayor]; lista[mayor] = temp; }
        }
    }
    public void OrdenarRepuestosPorExistencia(List<Repuesto> lista)
    {
        for (int i = 0; i < lista.Count - 1; i++)
            for (int j = 0; j < lista.Count - i - 1; j++)
                if (lista[j].Existencia > lista[j + 1].Existencia)
                { Repuesto temp = lista[j]; lista[j] = lista[j + 1]; lista[j + 1] = temp; }
    }
}
