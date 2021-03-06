﻿using UnityEngine;
using System.Collections;

public class DrawACardCommand : Comanda {

	private JugadorPartida p;
	private CartaPartida cl;
    private bool fast;
    private bool fromDeck;

	public DrawACardCommand(CartaPartida cl, JugadorPartida p, bool fast, bool fromDeck)
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
		bool rotarDeCara = false;
		if (p.GetType () == typeof(JugadorHumano))
			rotarDeCara = true;
		areaJugador.manoVisual.DarCartaJugador(cl, cl.ID, fast, fromDeck,rotarDeCara);
		if(p.GetType() == typeof(JugadorHumano))
			Controlador.Instance.MostrarCartasJugablesJugador (p);
    }
}
