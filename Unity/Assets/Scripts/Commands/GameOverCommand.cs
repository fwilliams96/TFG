using UnityEngine;
using System.Collections;

public class GameOverCommand : Comanda{

	private JugadorPartida looser;
	private int experiencia;

	public GameOverCommand(JugadorPartida looser, int experiencia)
    {
        this.looser = looser;
		this.experiencia = experiencia;
    }

    public override void EmpezarEjecucionComanda()
    {
		GameOver.Instance.MostrarGameOver(experiencia);
		Comandas.Instance.CompletarEjecucionComanda ();

    }
}
