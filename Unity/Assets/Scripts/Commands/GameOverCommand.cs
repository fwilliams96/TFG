using UnityEngine;
using System.Collections;

public class GameOverCommand : Comanda{

	private JugadorPartida looser;

	public GameOverCommand(JugadorPartida looser)
    {
        this.looser = looser;
    }

    public override void EmpezarEjecucionComanda()
    {
		GameOver.Instance.MostrarGameOver();
		Comandas.Instance.CompletarEjecucionComanda ();

    }
}
