using UnityEngine;
using System.Collections;

public abstract class DamageOpponentBattleCry : CreatureEffect
{

    public DamageOpponentBattleCry(Jugador owner, Criatura creature, int specialAmount) : base(owner, creature, specialAmount)
    { }

    //BattleCry
    public override void WhenACreatureIsPlayed()
    {
        Jugador oponente = Controlador.Instance.OtroJugador(owner);
        new DealDamageCommand(oponente.PlayerID, specialAmount, oponente.Vida - specialAmount).AñadirAlaCola();
        //TODO aqui se resta la vida del oponente, vigilar que hacer
        oponente.Vida = oponente.Vida - specialAmount;
    }
}
