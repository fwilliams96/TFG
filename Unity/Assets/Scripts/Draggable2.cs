using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Draggable2 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform parentToReturnTo = null;
    public Transform placerHolderParent = null;
    /*public enum Slot { WEAPON,HEAD,CHEST,LEGS,FEET, INVENTORY};
    public Slot typeOfItem = Slot.WEAPON;*/
    GameObject placeHolder = null;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag ");

        placeHolder = new GameObject();
        placeHolder.transform.SetParent(this.transform.parent);
        LayoutElement le = placeHolder.AddComponent<LayoutElement>();
        le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
        le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
        le.flexibleHeight = 0;
        le.flexibleWidth = 0;
        
        placeHolder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());
        parentToReturnTo = this.transform.parent;
        placerHolderParent = parentToReturnTo;
        this.transform.SetParent(this.transform.parent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        //GameObject.FindObjectOfType<DropZone>();

    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        this.transform.position = eventData.position;
        if (placeHolder.transform.parent != placerHolderParent)
        {
            placeHolder.transform.SetParent(placerHolderParent);
        }
           

        int newSiblingIndex = placerHolderParent.childCount;
        for(int i=0; i < placerHolderParent.childCount; i++)
        {
            if(this.transform.position.x < placerHolderParent.GetChild(i).position.x)
            {
                newSiblingIndex = i;
                if(placeHolder.transform.GetSiblingIndex() < newSiblingIndex) //
                    newSiblingIndex--;
                
                break;
            }
        }
        placeHolder.transform.SetSiblingIndex(newSiblingIndex);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        this.transform.SetParent(parentToReturnTo);
        this.transform.SetSiblingIndex(placeHolder.transform.GetSiblingIndex());
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        //EventSystem.current.RaycastAll(eventData);
        Destroy(placeHolder);
    }
}
