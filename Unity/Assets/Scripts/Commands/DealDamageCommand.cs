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
        //TODO esta comprobacion se quitara puesto que no se podrá ir de cara
        if (targetID == DatosGenerales.Instance.LowPlayer.PlayerID || targetID == DatosGenerales.Instance.TopPlayer.PlayerID)
        {
            // target is a hero
            target.GetComponent<PlayerPortraitVisual>().TakeDamage(amount,healthAfter);
        }
        else
        {
            // target is a creature
            target.GetComponent<OneCreatureManager>().HacerDaño(amount, healthAfter);
        }
        comandas.CompletarEjecucionComanda();
    }
}
