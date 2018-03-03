using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class HandVisual : MonoBehaviour
{
    #region Atributos
    // PUBLIC FIELDS
    public AreaPosition owner;
    public bool TakeCardsOpenly = true;
    public SameDistanceChildren slots;

    [Header("Transform References")]
    public Transform DrawPreviewSpot;
    public Transform DeckTransform;
    public Transform OtherCardDrawSourceTransform;
    public Transform PlayPreviewSpot;

    // PRIVATE : a list of all card visual representations as GameObjects
    private List<GameObject> CartasEnMano = new List<GameObject>();
    #endregion
    #region Getters/Setters
    public GameObject LastDealtCard { get; set; }
    #endregion

    //TODO nombre funciones en español
    public void AñadirCarta(GameObject card)
    {
        CartasEnMano.Insert(0, card);
        // set parent for this card
        card.transform.SetParent(slots.transform);
        // re-calculate the position of the hand
        MoverSlotCartas();
        ActualizarSlots();
    }

    public void EliminarCarta(GameObject card)
    {
        CartasEnMano.Remove(card);
        // re-calculate the position of the hand
        MoverSlotCartas();
        ActualizarSlots();
    }

    public GameObject CogerCartaPorIndice(int index)
    {
        return CartasEnMano[index];
    }

    public void EliminarCartaEnIndice(int index)
    {
        CartasEnMano.RemoveAt(index);
        // re-calculate the position of the hand
        MoverSlotCartas();
        ActualizarSlots();
    }

    void ActualizarSlots()
    {
        float posX;
        if (CartasEnMano.Count > 0)
            posX = (slots.Children[0].transform.localPosition.x - slots.Children[CartasEnMano.Count - 1].transform.localPosition.x) / 2f;
        else
            posX = 0f;

        slots.gameObject.transform.DOLocalMoveX(posX, 0.3f);
    }


    void MoverSlotCartas()
    {
        foreach (GameObject g in CartasEnMano)
        {
            g.transform.DOLocalMoveX(slots.Children[CartasEnMano.IndexOf(g)].transform.localPosition.x, 0.3f);
            // apply correct sorting order and HandSlot value for later 
            WhereIsTheCardOrEntity w = g.GetComponent<WhereIsTheCardOrEntity>();
            w.Slot = CartasEnMano.IndexOf(g);
            w.SetearOrdenCarta();
        }
    }

    // CARD DRAW METHODS
    public void DarCartaJugador(CardAsset c, int UniqueID, bool fast = false, bool fromDeck = true)
    {
        GameObject card;
        if (fromDeck)
            card = CrearCartaPorPosicion(c, DeckTransform.position, new Vector3(0f, -179f, 0f));
        else
            card = CrearCartaPorPosicion(c, OtherCardDrawSourceTransform.position, new Vector3(0f, -179f, 0f));
        // save this as visual representation in CardLogic
        // Player ownerPlayer = GlobalSettings.Instance.Players[owner];
        //Debug.Log(ownerPlayer);
        //Debug.Log(ownerPlayer.hand);
        //Debug.Log("CArdsInHand.Count: "+ ownerPlayer.hand.CardsInHand.Count);
        //Debug.Log("Attempted placeInHand: " +placeInHand);
        // ownerPlayer.hand.CardsInHand[0].VisualRepresentation = card;
        //Debug.Log(ownerPlayer.hand);
        // Set a tag to reflect where this card is
        foreach (Transform t in card.GetComponentsInChildren<Transform>())
            t.tag = owner.ToString() + "Card";
        // pass this card to HandVisual class
        AñadirCarta(card);
        // let the card know about its place in hand.
        WhereIsTheCardOrEntity w = card.GetComponent<WhereIsTheCardOrEntity>();
        w.TraerAlFrente();

        w.Slot = 0;
        w.EstadoVisual = VisualStates.Transicion;
        // pass a unique ID to this card.
        IDHolder id = card.AddComponent<IDHolder>();
        id.UniqueID = UniqueID;

        // save this card to change its state to "Hand" when it arrives to the hand.
        LastDealtCard = card;

        // move card to the hand;
        Sequence s = DOTween.Sequence();
        if (!fast)
        {
            Debug.Log("Not fast!!!");
            s.Append(card.transform.DOMove(DrawPreviewSpot.position, DatosGenerales.Instance.CardTransitionTime));
            if (TakeCardsOpenly)
                s.Insert(0f, card.transform.DORotate(Vector3.zero, DatosGenerales.Instance.CardTransitionTime));
            else
                s.Insert(0f, card.transform.DORotate(new Vector3(0f, 179f, 0f), DatosGenerales.Instance.CardTransitionTime));
            s.AppendInterval(DatosGenerales.Instance.CardPreviewTime);
            // displace the card so that we can select it in the scene easier.
            s.Append(card.transform.DOLocalMove(slots.Children[0].transform.localPosition, DatosGenerales.Instance.CardTransitionTime));
        }
        else
        {
            // displace the card so that we can select it in the scene easier.
            s.Append(card.transform.DOLocalMove(slots.Children[0].transform.localPosition, DatosGenerales.Instance.CardTransitionTimeFast));
            if (TakeCardsOpenly)
                s.Insert(0f, card.transform.DORotate(Vector3.zero, DatosGenerales.Instance.CardTransitionTimeFast));
        }

        s.OnComplete(() => CambiarEstadoCartaAMano(card, w));
    }

    void CambiarEstadoCartaAMano(GameObject card, WhereIsTheCardOrEntity w)
    {
        //Debug.Log("Changing state to Hand for card: " + card.gameObject.name);
        if (owner == AreaPosition.Low)
            w.EstadoVisual = VisualStates.ManoJugadorAbajo;
        else
            w.EstadoVisual = VisualStates.ManoJugadorArriba;

        w.SetearOrdenCarta();
        Comandas.Instance.CompletarEjecucionComanda();
    }

    GameObject CrearCartaPorPosicion(CardAsset c, Vector3 position, Vector3 eulerAngles)
    {
        // Instantiate a card depending on its type
        GameObject card;
        // this card is a creature card
        card = GameObject.Instantiate(DatosGenerales.Instance.CardPrefab, position, Quaternion.Euler(eulerAngles)) as GameObject;


        // apply the look of the card based on the info from CardAsset
        OneCardManager manager = card.GetComponent<OneCardManager>();
        manager.cardAsset = c;
        manager.LeerDatosAsset();

        return card;
    }

}
