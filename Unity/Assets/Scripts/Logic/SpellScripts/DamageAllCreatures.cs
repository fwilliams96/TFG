using UnityEngine;
using System.Collections;

public class DamageAllCreatures : EfectoMagica {

    public override void ActivateEffect(int specialAmount = 0, Ente target = null)
    {
        Ente[] CreaturesToDamage = Controlador.Instance.OtroJugador(Controlador.Instance.JugadorActual).EntesEnLaMesa();
        foreach (Ente cl in CreaturesToDamage)
        {
            new DealDamageCommand(cl.ID, specialAmount, health: cl.Defensa - specialAmount).AñadirAlaCola();
            cl.Defensa -= specialAmount;
        }
    }
}
