using UnityEngine;
using System.Collections;

public class DrawACardCommand : Comanda {

    private Jugador p;
    private Carta cl;
    private bool fast;
    private bool fromDeck;

    public DrawACardCommand(Carta cl, Jugador p, bool fast, bool fromDeck)
    {        
        this.cl = cl;
        this.p = p;
        this.fast = fast;
        this.fromDeck = fromDeck;
    }

    public override void EmpezarEjecucionComanda()
    {
        PlayerArea areaJugador = Controlador.Instance.AreaJugador(p);
        areaJugador.mazoVisual.CartasEnMazo--;
        areaJugador.manoVisual.DarCartaJugador(cl, cl.ID, fast, fromDeck);
        //TODO descomentar en caso de que MostrarCartasJugablesJugador de la funcion ActualizarValoresJugador falle
        Controlador.Instance.MostrarCartasJugablesJugador(p);
    }
}
