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
		GameObject carta = IDHolder.GetGameObjectWithID (item.ID);
		carta.GetComponent<ProgresoVisual> ().AñadirItem (item.Tipo, item.Cantidad);

    }
}
