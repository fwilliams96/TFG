using UnityEngine;
using System.Collections;

public class CreatureDieCommand : Comanda 
{
    private Player p;
    private int DeadCreatureID;

    public CreatureDieCommand(int CreatureID, Player p)
    {
        this.p = p;
        this.DeadCreatureID = CreatureID;
    }

    public override void EmpezarEjecucionComanda()
    {
        p.PArea.tableVisual.EliminarCriaturaID(DeadCreatureID);
    }
}
