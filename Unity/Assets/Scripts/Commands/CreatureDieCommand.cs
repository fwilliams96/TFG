using UnityEngine;
using System.Collections;

public class CreatureDieCommand : Comanda 
{
	private JugadorPartida p;
    private int DeadCreatureID;

	public CreatureDieCommand(int CreatureID, JugadorPartida p)
    {
        this.p = p;
        this.DeadCreatureID = CreatureID;
    }

    public override void EmpezarEjecucionComanda()
    {
        Controlador.Instance.AreaJugador(p).tableVisual.EliminarCriaturaID(DeadCreatureID);
    }
}
