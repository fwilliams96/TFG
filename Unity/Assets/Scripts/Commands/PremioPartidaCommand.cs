using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PremioPartidaCommand : Comanda{

	private JugadorPartida ganador;
	private Carta carta;
	private int exp;
	private List<Item> items;

	public PremioPartidaCommand(JugadorPartida ganador, Carta carta, List<Item> items,int exp)
    {
		this.ganador = ganador;
		this.carta = carta;
		this.items = items;
		this.exp = exp;
    }

	/// <summary>
	/// Función que muestra el premio obtenido en la partida al jugador usuario.
	/// </summary>
    public override void EmpezarEjecucionComanda()
    {
		PremioPartida.Instance.MostrarPremioPartida(carta,items,exp);
		Comandas.Instance.CompletarEjecucionComanda ();
    }
}
