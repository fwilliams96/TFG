using UnityEngine;
using System.Collections;

public class MuerteJugadorCommand : Comanda{

    private Jugador looser;

	public MuerteJugadorCommand(Jugador looser)
    {
        this.looser = looser;
    }

    public override void EmpezarEjecucionComanda()
    {
		Controlador.Instance.AreaJugador (looser).Personaje.Explotar ();

    }
}
