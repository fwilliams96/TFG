using UnityEngine;
using System.Collections;

public class AñadirItemCartaCommand : Comanda {

	private Carta carta;
	private Item item;
   

	public AñadirItemCartaCommand(Carta carta, Item item)
    {
		this.carta = carta;
		this.item = item;
    }

	/// <summary>
	/// Función que añade el item al script  progreso visual incoporado al gameobject de la carta.
	/// </summary>
    public override void EmpezarEjecucionComanda()
    {
		GameObject cartaG = IDHolder.GetGameObjectWithID (carta.ID);
		GameObject itemG = IDHolder.GetGameObjectWithID (item.ID);
		int tipoItem = item.GetType () == typeof(Piedra) ? 1 : 0;
		cartaG.GetComponent<ProgresoVisual> ().AñadirItem (tipoItem, item.Cantidad);
		IDHolder.EliminarElemento (itemG.GetComponent<IDHolder> ());
		GameObject.Destroy (itemG);
		Comandas.Instance.CompletarEjecucionComanda();
    }
}
