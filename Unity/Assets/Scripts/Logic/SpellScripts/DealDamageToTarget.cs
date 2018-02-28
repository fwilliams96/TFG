using UnityEngine;
using System.Collections;

public class DealDamageToTarget : SpellEffect 
{
    

    public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {
        new DealDamageCommand(target.ID, specialAmount, healthAfter: target.Defensa - specialAmount).AñadirAlaCola();
        target.Defensa -= specialAmount;
        //TODO comprobar si target muere
    }
}
