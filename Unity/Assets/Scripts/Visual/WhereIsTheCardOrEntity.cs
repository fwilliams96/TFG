using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// an enum to store the info about where this object is
public enum VisualStates
{
    Transicion,
    ManoJugadorAbajo,
    ManoJugadorArriba,
    MesaJugadorAbajo,
    MesaJugadorArriba,
    Arrastrando
}

public class WhereIsTheCardOrEntity : MonoBehaviour
{

    #region Atributos
    // reference to a HoverPreview Component
    //private HoverPreview hover;
    private OpcionesObjeto hover;

    // reference to a canvas on this object to set sorting order
    private Canvas canvas;

    // a value for canvas sorting order when we want to show this object above everything
    private int ValorClasificacionOrden = 500;

    // PROPERTIES
    private int slot = -1;
    private VisualStates estadoVisual;
    #endregion
    #region Getters/Setters
    public int Slot
    {
        get { return slot; }

        set
        {
            slot = value;
            /*if (value != -1)
            {
                canvas.sortingOrder = HandSortingOrder(slot);
            }*/
        }
    }


    public VisualStates EstadoVisual
    {
        get { return estadoVisual; }

        set
        {
            estadoVisual = value;
            switch (estadoVisual)
            {
                case VisualStates.ManoJugadorArriba:
                    hover.PrevisualizacionActivada = true;
                    break;
                case VisualStates.ManoJugadorAbajo:
                    hover.PrevisualizacionActivada = true;
                    break;
                case VisualStates.MesaJugadorAbajo:
                    hover.PrevisualizacionActivada = true;
                    break;
                case VisualStates.MesaJugadorArriba:
                    hover.PrevisualizacionActivada = true;
                    break;
                case VisualStates.Transicion:
                    hover.PrevisualizacionActivada = false;
                    break;
                case VisualStates.Arrastrando:
                    hover.PrevisualizacionActivada = false;
                    break;
            }
        }
    }
    #endregion

    void Awake()
    {
        //hover = GetComponent<HoverPreview>();
        hover = GetComponent<OpcionesObjeto>();

        // for characters hover is attached to a child game object
        //hover = GetComponentInChildren<HoverPreview>();
        if (hover == null)
            hover = GetComponentInChildren<OpcionesObjeto>();
        canvas = GetComponentInChildren<Canvas>();
    }

    public void TraerAlFrente()
    {
        canvas.sortingOrder = ValorClasificacionOrden;
        canvas.sortingLayerName = "AboveEverything";
    }

    // not setting sorting order inside of VisualStaes property because when the card is drawn, 
    // we want to set an index first and set the sorting order only when the card arrives to hand. 
    public void SetearOrdenCarta()
    {
        if (slot != -1)
            canvas.sortingOrder = HandSortingOrder(slot);
        canvas.sortingLayerName = "Cards";
    }

    public void SetearOrdenCriatura()
    {
        canvas.sortingOrder = 0;
        canvas.sortingLayerName = "Creatures";
    }

    private int HandSortingOrder(int placeInHand)
    {
        return (-(placeInHand + 1) * 10);
    }


}
