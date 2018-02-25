using UnityEngine;
using System.Collections;

public class CreatureDieCommand : Comanda 
{
    private Jugador p;
    private int DeadCreatureID;

    public CreatureDieCommand(int CreatureID, Jugador p)
    {
        this.p = p;
        this.DeadCreatureID = CreatureID;
    }

    public override void EmpezarEjecucionComanda()
    {
        p.PArea.tableVisual.EliminarCriaturaID(DeadCreatureID);
    }
}
