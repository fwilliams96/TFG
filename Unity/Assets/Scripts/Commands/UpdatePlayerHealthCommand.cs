using UnityEngine;
using System.Collections;

public class UpdatePlayerHealthCommand : Comanda {

	private JugadorPartida p;

	public UpdatePlayerHealthCommand(JugadorPartida p)
    {
        this.p = p;
    }

    public override void EmpezarEjecucionComanda()
    {
        PlayerArea areaJugador = Controlador.Instance.AreaJugador(p);
		areaJugador.Personaje.AumentarVida (p.Defensa);
    }


}
