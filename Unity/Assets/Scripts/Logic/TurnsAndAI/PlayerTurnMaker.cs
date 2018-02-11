using UnityEngine;
using System.Collections;

public class PlayerTurnMaker : TurnMaker
{
    public override void OnTurnStart()
    {
        base.OnTurnStart();
        // dispay a message that it is player`s turn
        if(p == GlobalSettings.Instance.TopPlayer)
            new ShowMessageCommand("Enemy Turn!", 2.0f).AñadirAlaCola();
        else
            new ShowMessageCommand("Your Turn!", 2.0f).AñadirAlaCola();
        p.DrawACard();
    }
}
