﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    //public Draggable.Slot typeOfItem = Draggable.Slot.INVENTORY;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;
        Debug.Log("OnPointerEnter");

        Draggable2 d = eventData.pointerDrag.GetComponent<Draggable2>();
        if (d != null)
        {
            //if(typeOfItem == d.typeOfItem || typeOfItem == Draggable.Slot.INVENTORY)
            d.placerHolderParent = this.transform;


        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;
        Debug.Log("OnPointerExit");
        Draggable2 d = eventData.pointerDrag.GetComponent<Draggable2>();
        if (d != null && d.placerHolderParent == this.transform)
        {
            //if(typeOfItem == d.typeOfItem || typeOfItem == Draggable.Slot.INVENTORY)
            d.placerHolderParent = d.parentToReturnTo;


        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag.name + " was dropped on" + gameObject.name);

        Draggable2 d = eventData.pointerDrag.GetComponent<Draggable2>();
        if (d != null)
        {
            //if(typeOfItem == d.typeOfItem || typeOfItem == Draggable.Slot.INVENTORY)
            d.parentToReturnTo = this.transform;


        }
    }
}
