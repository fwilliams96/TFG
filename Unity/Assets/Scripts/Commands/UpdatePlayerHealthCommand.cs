using UnityEngine;
using System.Collections;

public class UpdatePlayerHealthCommand : Comanda {

	private JugadorPartida p;
	private int vidaActual;

	public UpdatePlayerHealthCommand(JugadorPartida p,int vidaActual)
    {
        this.p = p;
		this.vidaActual = vidaActual;
    }

    public override void EmpezarEjecucionComanda()
    {
        PlayerArea areaJugador = Controlador.Instance.AreaJugador(p);
		areaJugador.Personaje.AumentarVida (vidaActual,p.Defensa);
    }


}
