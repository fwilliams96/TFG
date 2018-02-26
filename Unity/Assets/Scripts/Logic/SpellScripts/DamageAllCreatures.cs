using UnityEngine;
using System.Collections;

public class DamageAllCreatures : SpellEffect {

    public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {
        Ente[] CreaturesToDamage = Controlador.Instance.OtroJugador(Controlador.Instance.jugadorActual).CriaturasEnLaMesa();
        foreach (Ente cl in CreaturesToDamage)
        {
            new DealDamageCommand(cl.ID, specialAmount, healthAfter: cl.Vida - specialAmount).AñadirAlaCola();
            cl.Vida -= specialAmount;
        }
    }
}
