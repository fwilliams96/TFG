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

    public override void EmpezarEjecucionComanda()
    {
		GameObject cartaG = IDHolder.GetGameObjectWithID (carta.ID);
		GameObject itemG = IDHolder.GetGameObjectWithID (item.ID);
		cartaG.GetComponent<ProgresoVisual> ().AñadirItem (item.Tipo, item.Cantidad);
		IDHolder.EliminarElemento (itemG.GetComponent<IDHolder> ());
		GameObject.Destroy (itemG);

    }
}
