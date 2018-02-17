using UnityEngine;
using System.Collections;

public class PlayACreatureCommand : Comanda
{
    private CardLogic cl;
    private int tablePos;
    private Player p;
    private int creatureID;
    private bool posicionAtaque;

    public PlayACreatureCommand(CardLogic cl, Player p, int tablePos, bool posicionAtaque, int creatureID)
    {
        this.p = p;
        this.cl = cl;
        this.tablePos = tablePos;
        this.posicionAtaque = posicionAtaque;
        this.creatureID = creatureID;
    }

    public override void EmpezarEjecucionComanda()
    {
        // remove and destroy the card in hand 
        HandVisual PlayerHand = p.PArea.manoVisual;
        GameObject card = IDHolder.GetGameObjectWithID(cl.idCarta);
        PlayerHand.RemoveCard(card);
        GameObject.Destroy(card);
        // enable Hover Previews Back
        HoverPreview.PrevisualizacionesPermitidas = true;
        // move this card to the spot 
        p.PArea.tableVisual.AñadirCriatura(cl.assetCarta, creatureID, tablePos, posicionAtaque);
    }
}
