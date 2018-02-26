using UnityEngine;
using System.Collections;

public class PlayAEntityCommand : Comanda
{
    private Carta cl;
    private int tablePos;
    private Jugador p;
    private Ente ente;

    public PlayAEntityCommand(Carta cl, Jugador p, int tablePos, Ente ente)
    {
        this.p = p;
        this.cl = cl;
        this.tablePos = tablePos;
        this.ente = ente;
    }

    public override void EmpezarEjecucionComanda()
    {
        //Eliminar la carta de la mano del jugador y elimina el gameobject
        HandVisual PlayerHand = p.PArea.manoVisual;
        //TODO se podria hacer que el id de la carta pase a ser el id de la criatura 
        GameObject card = IDHolder.GetGameObjectWithID(cl.ID);
        PlayerHand.RemoveCard(card);
        GameObject.Destroy(card);
        //Permite la previsualizacion de cartas
        HoverPreview.PrevisualizacionesPermitidas = true;
        // Añade la carta en el tablero
        if (ente.GetType() == typeof(Magica))
            p.PArea.tableVisual.AñadirMagica(cl.assetCarta, ente.ID, tablePos);
        else
        {
            if(((Criatura)ente).PosicionCriatura.Equals(PosicionCriatura.ATAQUE))
                p.PArea.tableVisual.AñadirCriaturaAtaque(cl.assetCarta, ente.ID, tablePos);
            else
                p.PArea.tableVisual.AñadirCriaturaDefensa(cl.assetCarta, ente.ID, tablePos);
        }
            
    }
}
