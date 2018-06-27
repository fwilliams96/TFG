using UnityEngine;
using System.Collections;

public class EnteDieCommand : Comanda 
{
	private JugadorPartida p;
    private int DeadCreatureID;

	public EnteDieCommand(int CreatureID, JugadorPartida p)
    {
        this.p = p;
        this.DeadCreatureID = CreatureID;
    }

	/// <summary>
	/// Elimina la criatura o mágica de la mesa de batalla.
	/// </summary>
    public override void EmpezarEjecucionComanda()
    {
        Controlador.Instance.AreaJugador(p).tableVisual.EliminarEnteID(DeadCreatureID);
    }
}
