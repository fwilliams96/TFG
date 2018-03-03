using UnityEngine;
using System.Collections;
using DG.Tweening;

public class DragSpellNoTarget: DraggingActions{

    private int savedHandSlot;
    private WhereIsTheCardOrEntity whereIsCard;
    private OneCardManager manager;

    public override bool PuedeSerLanzada
    {
        get
        {
            return base.PuedeSerLanzada && manager.PuedeSerJugada;
        }
    }

    void Awake()
    {
        whereIsCard = GetComponent<WhereIsTheCardOrEntity>();
        manager = GetComponent<OneCardManager>();
    }

    public override void OnStartDrag()
    {
        savedHandSlot = whereIsCard.Slot;

        whereIsCard.EstadoVisual = VisualStates.Arrastrando;
        whereIsCard.TraerAlFrente();

    }

    public override void OnDraggingInUpdate()
    {
        
    }

    public override void OnEndDrag()
    {
        // 1) Check if we are holding a card over the table
        if (DragSuccessful())
        {
            // play this card
            Controlador.Instance.JugarMagicaMano(GetComponent<IDHolder>().UniqueID, -1);
        }
        else
        {
            // Set old sorting order 
            whereIsCard.Slot = savedHandSlot;
            whereIsCard.EstadoVisual = VisualStates.ManoJugadorAbajo;
            // Move this card back to its slot position
            HandVisual PlayerHand = Controlador.Instance.AreaJugador(Controlador.Instance.JugadorActual).manoVisual;
            Vector3 oldCardPos = PlayerHand.slots.Children[savedHandSlot].transform.localPosition;
            transform.DOLocalMove(oldCardPos, 1f);
        } 
    }

    protected override bool DragSuccessful()
    {
        //bool TableNotFull = (TurnManager.Instance.whoseTurn.table.CreaturesOnTable.Count < 8);

        return TableVisual.CursorSobreAlgunaMesa; //&& TableNotFull;
    }


}
