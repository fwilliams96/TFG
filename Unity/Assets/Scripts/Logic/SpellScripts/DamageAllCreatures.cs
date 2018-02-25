using UnityEngine;
using System.Collections;

public class DamageAllCreatures : SpellEffect {

    public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {
        Criatura[] CreaturesToDamage = Controlador.Instance.OtroJugador(Controlador.Instance.jugadorActual).CriaturasEnLaMesa();
        foreach (Criatura cl in CreaturesToDamage)
        {
            new DealDamageCommand(cl.ID, specialAmount, healthAfter: cl.Vida - specialAmount).AñadirAlaCola();
            cl.Vida -= specialAmount;
        }
    }
}
