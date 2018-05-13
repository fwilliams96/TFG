using UnityEngine;
using System.Collections;

public class GiveManaBonus: EfectoMagica 
{
	public override void ActivateEffect(int specialAmount = 0, Ente target = null)
    {
        Controlador.Instance.JugadorActual.ConseguirManaExtra(specialAmount);
    }
}
