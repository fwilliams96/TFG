using UnityEngine;
using System.Collections;

public class DealDamageCommand : Comanda {

    private int targetID;
    private int amount;
	private int health;

    public DealDamageCommand( int targetID, int amount, int health)
    {
        this.targetID = targetID;
        this.amount = amount;
        this.health = health;
    }

	/// <summary>
	/// Función que permite hacer daño a un jugador o una criatura.
	/// </summary>
	public override void EmpezarEjecucionComanda()
	{
		GameObject target = IDHolder.GetGameObjectWithID(targetID);
		if (targetID == Controlador.Instance.Local.ID || targetID == Controlador.Instance.Enemigo.ID)
		{
			target.GetComponent<PlayerPortraitVisual>().HacerDaño(amount,health);
		}
		else
		{
			target.GetComponent<OneCreatureManager>().HacerDaño(amount, health);
		}
		comandas.CompletarEjecucionComanda();
	}
}
