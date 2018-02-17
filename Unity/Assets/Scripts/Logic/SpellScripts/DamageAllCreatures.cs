﻿using UnityEngine;
using System.Collections;

public class DamageAllCreatures : SpellEffect {

    public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {
        CreatureLogic[] CreaturesToDamage = ControladorTurno.Instance.whoseTurn.otherPlayer.table.CreaturesOnTable.ToArray();
        foreach (CreatureLogic cl in CreaturesToDamage)
        {
            new DealDamageCommand(cl.ID, specialAmount, healthAfter: cl.Health - specialAmount).AñadirAlaCola();
            cl.Health -= specialAmount;
        }
    }
}
