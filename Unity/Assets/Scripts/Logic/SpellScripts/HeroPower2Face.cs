using UnityEngine;
using System.Collections;

public class HeroPower2Face : SpellEffect 
{

    public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {
        new DealDamageCommand(ControladorTurno.Instance.whoseTurn.otherPlayer.PlayerID, 2, ControladorTurno.Instance.whoseTurn.otherPlayer.Health - 2).AñadirAlaCola();
        ControladorTurno.Instance.whoseTurn.otherPlayer.Health -= 2;
    }
}
