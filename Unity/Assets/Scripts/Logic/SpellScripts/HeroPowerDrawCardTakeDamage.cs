using UnityEngine;
using System.Collections;

public class HeroPowerDrawCardTakeDamage : SpellEffect {

    public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {
        // Take 2 damage
        new DealDamageCommand(ControladorTurno.Instance.jugadorActual.PlayerID, 2, ControladorTurno.Instance.jugadorActual.Vida - 2).AñadirAlaCola();
        ControladorTurno.Instance.jugadorActual.Vida -= 2;
        // Draw a card
        ControladorTurno.Instance.jugadorActual.DibujarCartaMazo();

    }
}
