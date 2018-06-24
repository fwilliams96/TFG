using UnityEngine;
using System.Collections;

public class MuerteJugadorCommand : Comanda{

	private JugadorPartida looser;

	public MuerteJugadorCommand(JugadorPartida looser)
    {
        this.looser = looser;
    }

	/// <summary>
	/// Animación de explosión cuando muere un jugador.
	/// </summary>
    public override void EmpezarEjecucionComanda()
    {
		Controlador.Instance.AreaJugador (looser).Personaje.Explotar ();

    }
}
