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

	public override void EmpezarEjecucionComanda()
	{
		Debug.Log("In deal damage command!");

		GameObject target = IDHolder.GetGameObjectWithID(targetID);
		if (targetID == Controlador.Instance.Local.ID || targetID == Controlador.Instance.Enemigo.ID)
		{
			target.GetComponent<PlayerPortraitVisual>().HacerDaño(amount,health);
		}
		else
		{
			//TODO ver si se elimina porque supuestamente solo se está usando DealDamageCommand para atacar jugadores y no criaturas (criaturas ya lo hace creature attack)
			target.GetComponent<OneCreatureManager>().HacerDaño(amount, health);
		}
		comandas.CompletarEjecucionComanda();
	}
}
