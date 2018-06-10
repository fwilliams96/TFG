using UnityEngine;
using System.Collections;

public class StartATurnCommand : Comanda {

	private JugadorPartida p;

	public StartATurnCommand(JugadorPartida p)
    {
        this.p = p;
    }

    public override void EmpezarEjecucionComanda()
    {
        //Cambiamos el jugador actual
        Controlador.Instance.JugadorActual = p;
		Controlador.Instance.ActualizarValoresJugador(p);
        /*Controlador.Instance.OcultarManoJugadorAnterior();
        Controlador.Instance.MostrarManoJugadorActual();*/
        // this command is completed instantly
        comandas.CompletarEjecucionComanda();
    }
}
