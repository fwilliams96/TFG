using UnityEngine;
using System.Collections;

public class ActivateEffectCommand : Comanda
{
    private int AttackerUniqueID;

    public ActivateEffectCommand(int attackerID)
    {
        this.AttackerUniqueID = attackerID;
    }

	/// <summary>
	/// Función que básicamente voltea la carta mágica
	/// </summary>
    public override void EmpezarEjecucionComanda()
    {
        GameObject Attacker = IDHolder.GetGameObjectWithID(AttackerUniqueID);
        Attacker.GetComponent<MagicEffectVisual>().ColocarMagicaBocaArriba();
		Comandas.Instance.CompletarEjecucionComanda();
    }
}
