using UnityEngine;
using System.Collections;

public class AñadirItemCartaPrevisualizadaCommand : Comanda {

	private Item item;
   

	public AñadirItemCartaPrevisualizadaCommand(Item item)
    {
		this.item = item;
    }

	/// <summary>
	/// Se añade el item a la carta previsualizada en el menú.
	/// </summary>
    public override void EmpezarEjecucionComanda()
    {
		GameObject cartaPrevisualizada = GameObject.FindGameObjectWithTag ("CartaPrevisualizada");
		int tipoItem = item.GetType () == typeof(Piedra) ? 1 : 0;
		cartaPrevisualizada.GetComponent<ProgresoVisual> ().AñadirItem (tipoItem, item.Cantidad);
		Comandas.Instance.CompletarEjecucionComanda();
    }
}
