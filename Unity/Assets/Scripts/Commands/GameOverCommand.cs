using UnityEngine;
using System.Collections;

public class GameOverCommand : Comanda{

    private Player looser;

    public GameOverCommand(Player looser)
    {
        this.looser = looser;
    }

    public override void EmpezarEjecucionComanda()
    {
        looser.PArea.Personaje.Explotar();
    }
}
