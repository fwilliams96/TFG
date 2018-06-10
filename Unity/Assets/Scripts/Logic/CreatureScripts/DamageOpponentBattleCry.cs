using UnityEngine;
using System.Collections;

public abstract class DamageOpponentBattleCry : EfectoEnte
{

	public DamageOpponentBattleCry(JugadorPartida owner, Criatura creature, int specialAmount) : base(owner, creature, specialAmount)
    { }

    //BattleCry
    public override void WhenACreatureIsPlayed()
    {
		JugadorPartida oponente = Controlador.Instance.OtroJugador(owner);
		new DealDamageCommand(oponente.ID, specialAmount, oponente.Defensa - specialAmount).AñadirAlaCola();
        //TODO aqui se resta la vida del oponente, vigilar que hacer
        oponente.Defensa = oponente.Defensa - specialAmount;
    }
}
