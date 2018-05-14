using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DamageAllCreatures : EfectoMagica {

    public override void ActivateEffect(int specialAmount = 0, Ente target = null)
    {
		List<Ente> CreaturesToDamage = Controlador.Instance.OtroJugador(Controlador.Instance.JugadorActual).EntesEnLaMesa();
        foreach (Ente cl in CreaturesToDamage)
        {
            new DealDamageCommand(cl.ID, specialAmount, health: cl.Defensa - specialAmount).AñadirAlaCola();
            cl.Defensa -= specialAmount;
        }
    }
}
