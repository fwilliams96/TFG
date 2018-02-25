﻿using UnityEngine;
using System.Collections;

public class DealDamageToTarget : SpellEffect 
{
    

    public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {
        new DealDamageCommand(target.ID, specialAmount, healthAfter: target.Vida - specialAmount).AñadirAlaCola();
        target.Vida -= specialAmount;
        //TODO comprobar si target muere
    }
}
