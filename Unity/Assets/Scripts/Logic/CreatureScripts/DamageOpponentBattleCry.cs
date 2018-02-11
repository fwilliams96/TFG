using UnityEngine;
using System.Collections;

public abstract class DamageOpponentBattleCry : CreatureEffect
{

    public DamageOpponentBattleCry(Player owner, CreatureLogic creature, int specialAmount) : base(owner, creature, specialAmount)
    { }

    //BattleCry
    public override void WhenACreatureIsPlayed()
    {
        new DealDamageCommand(owner.otherPlayer.PlayerID, specialAmount, owner.otherPlayer.Health - specialAmount).AñadirAlaCola();
        owner.otherPlayer.Health = owner.otherPlayer.Health - specialAmount;
    }
}
