using UnityEngine;
using System.Collections;

public class DeshabilitarManaCommand : Comanda {

	private JugadorPartida p;

	public DeshabilitarManaCommand(JugadorPartida p)
    {
        this.p = p;
    }

    public override void EmpezarEjecucionComanda()
    {
        PlayerArea areaJugador = Controlador.Instance.AreaJugador(p);
		areaJugador.ManaBar.gameObject.SetActive (false);
        comandas.CompletarEjecucionComanda();
    }
}
