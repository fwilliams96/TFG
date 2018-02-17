using UnityEngine;
using System.Collections;

public class StartATurnCommand : Comanda {

    private Player p;

    public StartATurnCommand(Player p)
    {
        this.p = p;
    }

    public override void EmpezarEjecucionComanda()
    {
        ControladorTurno.Instance.whoseTurn = p;
        ControladorTurno.Instance.ActualizarValoresJugador();
        // this command is completed instantly
        comandas.CompletarEjecucionComanda();
    }
}
