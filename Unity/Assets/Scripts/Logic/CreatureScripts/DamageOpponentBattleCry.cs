using UnityEngine;
using System.Collections;

public abstract class DamageOpponentBattleCry : CreatureEffect
{

    public DamageOpponentBattleCry(Player owner, CreatureLogic creature, int specialAmount) : base(owner, creature, specialAmount)
    { }

    //BattleCry
    public override void WhenACreatureIsPlayed()
    {
        new DealDamageCommand(owner.otroJugador.PlayerID, specialAmount, owner.otroJugador.Vida - specialAmount).AñadirAlaCola();
        owner.otroJugador.Vida = owner.otroJugador.Vida - specialAmount;
    }
}
