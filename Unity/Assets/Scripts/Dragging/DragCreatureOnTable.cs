﻿using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class DragCreatureOnTable : DraggingActions {

    private int savedHandSlot;
    private WhereIsTheCardOrCreature whereIsCard;
    private IDHolder idScript;
    private VisualStates tempState;
    private OneCardManager manager;

    public override bool CanDrag
    {
        get
        {
            // TODO : include full field check
            //return true;
            //manage es si está con glow
            return base.CanDrag && manager.CanBePlayedNow;
        }
    }

    void Awake()
    {
        whereIsCard = GetComponent<WhereIsTheCardOrCreature>();
        manager = GetComponent<OneCardManager>();
    }

    public override void OnStartDrag()
    {
        savedHandSlot = whereIsCard.Slot;
        tempState = whereIsCard.VisualState;
        whereIsCard.VisualState = VisualStates.Dragging;
        whereIsCard.BringToFront();

    }

    public override void OnDraggingInUpdate()
    {

    }

    public override void OnEndDrag()
    {
        
        // 1) Check if we are holding a card over the table
        if (DragSuccessful())
        {
            
            //Activar pop-up de tipo de posicion
            if (!manager.TypeText.text.Equals("Magica"))
            {
                this.gameObject.SetActive(false);
                PosicionCriatura.Instance.MostrarPopupEleccionPosicion();
                PosicionCriatura.Instance.RegistrarCallBack(ColocarCartaTablero);
            }
            else
            {
                ColocarCartaTablero(true);
            }
            
            //while (!PosicionCriatura.Instance.Closed);
            //ColocarCartaTablero(PosicionCriatura.Instance.Ataque);

        }
        else
        {
            // Set old sorting order 
            whereIsCard.SetHandSortingOrder();
            whereIsCard.VisualState = tempState;
            // Move this card back to its slot position
            HandVisual PlayerHand = playerOwner.PArea.handVisual;
            Vector3 oldCardPos = PlayerHand.slots.Children[savedHandSlot].transform.localPosition;
            transform.DOLocalMove(oldCardPos, 1f);
        } 
    }

    public void ColocarCartaTablero(bool ataque)
    {
        Debug.Log("ColocarCartaTablero ataque " + ataque);
        // determine table position
        int tablePos = playerOwner.PArea.tableVisual.TablePosForNewCreature(Camera.main.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z - Camera.main.transform.position.z)).x);
        // Debug.Log("Table Pos for new Creature: " + tablePos.ToString());
        // play this card
        playerOwner.PlayACreatureFromHand(GetComponent<IDHolder>().UniqueID, tablePos,ataque);
    }

    protected override bool DragSuccessful()
    {
        bool TableNotFull = (playerOwner.table.CreaturesOnTable.Count < 8);

        return TableVisual.CursorOverSomeTable && TableNotFull;
    }
}
