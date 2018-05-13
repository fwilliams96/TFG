using UnityEngine;
using System.Collections;

public class GameOverCommand : Comanda{

    private Jugador looser;

    public GameOverCommand(Jugador looser)
    {
        this.looser = looser;
    }

    public override void EmpezarEjecucionComanda()
    {
		GameOver.Instance.MostrarGameOver();
		Comandas.Instance.CompletarEjecucionComanda ();

    }
}
