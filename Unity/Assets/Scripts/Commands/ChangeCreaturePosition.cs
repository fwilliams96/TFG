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

	/// <summary>
	/// Función que cambia la posición de la criatura (defensa o ataque)
	/// </summary>
    public override void EmpezarEjecucionComanda()
    {
        GameObject Attacker = IDHolder.GetGameObjectWithID(AttackerUniqueID);

        //Debug.Log(TargetUniqueID);
        Attacker.GetComponent<CreatureAttackVisual>().ChangePosition(pos);
    }
}
