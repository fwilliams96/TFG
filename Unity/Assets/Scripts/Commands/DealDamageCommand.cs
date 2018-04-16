using UnityEngine;
using System.Collections;

public class DealDamageCommand : Comanda {

    private int targetID;
    private int amount;
    private int healthAfter;

    public DealDamageCommand( int targetID, int amount, int healthAfter)
    {
        this.targetID = targetID;
        this.amount = amount;
        this.healthAfter = healthAfter;
    }

	public override void EmpezarEjecucionComanda()
	{
		Debug.Log("In deal damage command!");

		GameObject target = IDHolder.GetGameObjectWithID(targetID);
		if (targetID == Controlador.Instance.Local.ID || targetID == Controlador.Instance.Enemigo.ID)
		{
			target.GetComponent<PlayerPortraitVisual>().HacerDaño(amount,healthAfter);
		}
		else
		{
			target.GetComponent<OneCreatureManager>().HacerDaño(amount, healthAfter);
		}
		comandas.CompletarEjecucionComanda();
	}
}
