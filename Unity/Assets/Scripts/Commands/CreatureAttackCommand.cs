using UnityEngine;
using System.Collections;

public class CreatureAttackCommand : Comanda
{
    // position of creature on enemy`s table that will be attacked
    // if enemyindex == -1 , attack an enemy character 
    private int TargetUniqueID;
    private int AttackerUniqueID;
	private int TargetHealth;
    private int DamageTaken;

    public CreatureAttackCommand(int targetID, int attackerID,int damageTaken, int targetHealth)
    {
        this.TargetUniqueID = targetID;
        this.AttackerUniqueID = attackerID;
        this.TargetHealth = targetHealth;
		this.DamageTaken = damageTaken;
    }

    public override void EmpezarEjecucionComanda()
    {
        GameObject Attacker = IDHolder.GetGameObjectWithID(AttackerUniqueID);

		Attacker.GetComponent<CreatureAttackVisual>().AttackTarget(TargetUniqueID, DamageTaken, TargetHealth);
    }
}
