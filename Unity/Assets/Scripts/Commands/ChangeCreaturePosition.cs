using UnityEngine;
using System.Collections;

public class ChangeCreaturePosition : Comanda
{ 
    private int AttackerUniqueID;
    private PosicionCriatura pos;

    public ChangeCreaturePosition(int AttackerUniqueID, PosicionCriatura pos)
    {
        this.AttackerUniqueID = AttackerUniqueID;
        this.pos = pos;
    }

    public override void EmpezarEjecucionComanda()
    {
        GameObject Attacker = IDHolder.GetGameObjectWithID(AttackerUniqueID);

        //Debug.Log(TargetUniqueID);
        Attacker.GetComponent<CreatureAttackVisual>().ChangePosition(pos);
    }
}
