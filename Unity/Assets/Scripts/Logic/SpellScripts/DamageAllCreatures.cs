using UnityEngine;
using System.Collections;

public class DamageAllCreatures : SpellEffect {

    public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {
        CreatureLogic[] CreaturesToDamage = ControladorTurno.Instance.jugadorActual.otroJugador.CriaturasEnLaMesa();
        foreach (CreatureLogic cl in CreaturesToDamage)
        {
            new DealDamageCommand(cl.ID, specialAmount, healthAfter: cl.Vida - specialAmount).AñadirAlaCola();
            cl.Vida -= specialAmount;
        }
    }
}
