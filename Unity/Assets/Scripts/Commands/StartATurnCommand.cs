using UnityEngine;
using System.Collections;

public class StartATurnCommand : Comanda {

    private Jugador p;

    public StartATurnCommand(Jugador p)
    {
        this.p = p;
    }

    public override void EmpezarEjecucionComanda()
    {
        Controlador.Instance.jugadorActual = p;
        Controlador.Instance.ActualizarValoresJugador();
        // this command is completed instantly
        comandas.CompletarEjecucionComanda();
    }
}
