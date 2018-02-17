using UnityEngine;
using System.Collections;

public class BiteOwner : CreatureEffect
{  
    public BiteOwner(Player owner, CreatureLogic creature, int specialAmount): base(owner, creature, specialAmount)
    {}

    public override void RegisterEventEffect()
    {
        owner.EndTurnEvent += CauseEventEffect;
        Debug.Log("Registered bite effect!!!!");
    }

    public override void CauseEventEffect()
    {
        Debug.Log("InCauseEffect: owner: "+ owner + " specialAmount: "+ specialAmount);
        new DealDamageCommand(owner.PlayerID, specialAmount, owner.Vida - specialAmount).AñadirAlaCola();
        owner.Vida -= specialAmount;
    }
}
