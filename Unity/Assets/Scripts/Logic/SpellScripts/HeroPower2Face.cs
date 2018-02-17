using UnityEngine;
using System.Collections;

public class HeroPower2Face : SpellEffect 
{

    public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {
        new DealDamageCommand(ControladorTurno.Instance.jugadorActual.otroJugador.PlayerID, 2, ControladorTurno.Instance.jugadorActual.otroJugador.Vida - 2).AñadirAlaCola();
        ControladorTurno.Instance.jugadorActual.otroJugador.Vida -= 2;
    }
}
