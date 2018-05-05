using UnityEngine;
using System.Collections;

public class CreatureAttackCommand : Comanda
{
    // position of creature on enemy`s table that will be attacked
    // if enemyindex == -1 , attack an enemy character 
    private int TargetUniqueID;
    private int AttackerUniqueID;
    private int AttackerHealthAfter;
	private int TargetHealth;
    private int DamageTakenByAttacker;
    private int DamageTakenByTarget;

    public CreatureAttackCommand(int targetID, int attackerID, int damageTakenByAttacker, int damageTakenByTarget, int attackerHealthAfter, int targetHealth)
    {
        this.TargetUniqueID = targetID;
        this.AttackerUniqueID = attackerID;
        this.AttackerHealthAfter = attackerHealthAfter;
        this.TargetHealth = targetHealth;
        this.DamageTakenByTarget = damageTakenByTarget;
        this.DamageTakenByAttacker = damageTakenByAttacker;
    }

    public override void EmpezarEjecucionComanda()
    {
        GameObject Attacker = IDHolder.GetGameObjectWithID(AttackerUniqueID);

        Attacker.GetComponent<CreatureAttackVisual>().AttackTarget(TargetUniqueID, DamageTakenByTarget, DamageTakenByAttacker, AttackerHealthAfter, TargetHealth);
    }
}
