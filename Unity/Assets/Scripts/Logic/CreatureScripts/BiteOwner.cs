using UnityEngine;
using System.Collections;

public class BiteOwner : EfectoEnte
{  
    public BiteOwner(Jugador owner, Criatura creature, int specialAmount): base(owner, creature, specialAmount)
    {}

    public override void RegisterEventEffect()
    {
        owner.EndTurnEvent += CauseEventEffect;
        Debug.Log("Registered bite effect!!!!");
    }

    public override void CauseEventEffect()
    {
        Debug.Log("InCauseEffect: owner: "+ owner + " specialAmount: "+ specialAmount);
        new DealDamageCommand(owner.PlayerID, specialAmount, owner.Defensa - specialAmount).AñadirAlaCola();
        owner.Defensa -= specialAmount;
    }
}
