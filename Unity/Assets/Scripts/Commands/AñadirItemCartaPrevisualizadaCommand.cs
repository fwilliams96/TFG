using UnityEngine;
using System.Collections;

public class AñadirItemCartaPrevisualizadaCommand : Comanda {

	private Item item;
   

	public AñadirItemCartaPrevisualizadaCommand(Item item)
    {
		this.item = item;
    }

    public override void EmpezarEjecucionComanda()
    {
		GameObject cartaPrevisualizada = GameObject.FindGameObjectWithTag ("CartaPrevisualizada");
		cartaPrevisualizada.GetComponent<ProgresoVisual> ().AñadirItem (item.Tipo, item.Cantidad);
		Comandas.Instance.CompletarEjecucionComanda();
    }
}
