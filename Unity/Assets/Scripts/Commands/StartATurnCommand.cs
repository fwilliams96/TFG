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
        //Cambiamos el jugador actual
        Controlador.Instance.JugadorActual = p;
        Controlador.Instance.ActualizarValoresJugador();
        //OPTIONAL la primera vez que inicia el juego esto podria sobrar
        Controlador.Instance.OcultarManoJugadorAnterior();
        Controlador.Instance.MostrarManoJugadorActual();
        // this command is completed instantly
        comandas.CompletarEjecucionComanda();
    }
}
