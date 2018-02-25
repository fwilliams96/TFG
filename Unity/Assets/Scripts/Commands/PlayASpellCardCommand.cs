using UnityEngine;
using System.Collections;

public class PlayASpellCardCommand: Comanda
{
    private Carta card;
    private Jugador p;
    //private ICharacter target;

    public PlayASpellCardCommand(Jugador p, Carta card)
    {
        this.card = card;
        this.p = p;
    }

    public override void EmpezarEjecucionComanda()
    {
        // move this card to the spot
        p.PArea.manoVisual.PlayASpellFromHand(card.idCarta);
        // do all the visual stuff (for each spell separately????)
    }
}
