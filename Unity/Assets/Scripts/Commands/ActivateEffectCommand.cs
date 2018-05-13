using UnityEngine;
using System.Collections;

public class ActivateEffectCommand : Comanda
{
    // position of creature on enemy`s table that will be attacked
    // if enemyindex == -1 , attack an enemy character 
    private int AttackerUniqueID;

    public ActivateEffectCommand(int attackerID)
    {
        this.AttackerUniqueID = attackerID;
    }

    public override void EmpezarEjecucionComanda()
    {
        GameObject Attacker = IDHolder.GetGameObjectWithID(AttackerUniqueID);
        Attacker.GetComponent<MagicEffectVisual>().ColocarMagicaBocaArriba();
		Comandas.Instance.CompletarEjecucionComanda();
    }
}
