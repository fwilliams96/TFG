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
        looser.PArea.Personaje.Explotar();
    }
}
