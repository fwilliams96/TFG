using UnityEngine;
using System.Collections;

public class DealDamageToTarget : EfectoMagica 
{
    

    public override void ActivateEffect(int specialAmount = 0, Ente target = null)
    {
        new DealDamageCommand(target.ID, specialAmount, health: target.Defensa - specialAmount).AñadirAlaCola();
        target.Defensa -= specialAmount;
        //TODO comprobar si target muere
    }
}
