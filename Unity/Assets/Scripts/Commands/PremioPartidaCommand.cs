using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PremioPartidaCommand : Comanda{

	private Jugador ganador;
	private Carta carta;
	private int exp;
	private List<Item> items;

	public PremioPartidaCommand(Jugador ganador, Carta carta, List<Item> items,int exp)
    {
		this.ganador = ganador;
		this.carta = carta;
		this.items = items;
		this.exp = exp;
    }

    public override void EmpezarEjecucionComanda()
    {
		PremioPartida.Instance.MostrarPremioPartida(carta,items,exp);
		Comandas.Instance.CompletarEjecucionComanda ();
    }
}
