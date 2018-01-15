using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class HandVisual : MonoBehaviour
{
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
    private List<GameObject> CardsInHand = new List<GameObject>();

    public GameObject LastDealtCard{ get; set;}

    public void PlayASpellFromHand(int CardID)
    {
        GameObject card = IDHolder.GetGameObjectWithID(CardID);
        PlayASpellFromHand(card);
    }

    public void PlayASpellFromHand(GameObject CardVisual)
    {
        Command.CommandExecutionComplete();
        CardVisual.GetComponent<WhereIsTheCardOrCreature>().VisualState = VisualStates.Transition;
        RemoveCard(CardVisual);

        CardVisual.transform.SetParent(null);

        Sequence s = DOTween.Sequence();
        s.Append(CardVisual.transform.DOMove(PlayPreviewSpot.position, 1f));
        s.Insert(0f, CardVisual.transform.DORotate(Vector3.zero, 1f));
        s.AppendInterval(2f);
        s.OnComplete(()=>
            {
                //Command.CommandExecutionComplete();
                Destroy(CardVisual);
            });
    }

    public void AddCard(GameObject card)
    {
        CardsInHand.Insert(0, card);
        // set parent for this card
        card.transform.SetParent(slots.transform);
        // re-calculate the position of the hand
        PlaceCardsOnNewSlots();
        UpdatePlacementOfSlots();
    }

    public void RemoveCard(GameObject card)
    {
        CardsInHand.Remove(card);
        // re-calculate the position of the hand
        PlaceCardsOnNewSlots();
        UpdatePlacementOfSlots();
    }

    public GameObject GetCardAtIndex(int index)
    {
        return CardsInHand[index];
    }

    public void RemoveCardAtIndex(int index)
    {
        CardsInHand.RemoveAt(index);
        // re-calculate the position of the hand
        PlaceCardsOnNewSlots();
        UpdatePlacementOfSlots();
    }

    void UpdatePlacementOfSlots()
    {
        float posX;
        if (CardsInHand.Count > 0)
            posX = (slots.Children[0].transform.localPosition.x - slots.Children[CardsInHand.Count - 1].transform.localPosition.x) / 2f;
        else
            posX = 0f;
            
        slots.gameObject.transform.DOLocalMoveX(posX, 0.3f);  
    }


    void PlaceCardsOnNewSlots()
    {
        foreach (GameObject g in CardsInHand)
        {
            g.transform.DOLocalMoveX(slots.Children[CardsInHand.IndexOf(g)].transform.localPosition.x, 0.3f);
            // apply correct sorting order and HandSlot value for later 
            WhereIsTheCardOrCreature w = g.GetComponent<WhereIsTheCardOrCreature>();
            w.Slot = CardsInHand.IndexOf(g);
            w.SetHandSortingOrder();
        }
    }

    // CARD DRAW METHODS
    public void GivePlayerACard(CardAsset c, int UniqueID, bool fast = false, bool fromDeck = true)
    {
        GameObject card;
        if (fromDeck)
            card = CreateACardAtPosition(c, DeckTransform.position, new Vector3(0f, -179f, 0f));
        else
            card = CreateACardAtPosition(c, OtherCardDrawSourceTransform.position, new Vector3(0f, -179f, 0f));
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
            t.tag = owner.ToString()+"Card";
        // pass this card to HandVisual class
        AddCard(card);
        // let the card know about its place in hand.
        WhereIsTheCardOrCreature w = card.GetComponent<WhereIsTheCardOrCreature>();
        w.BringToFront();

        w.Slot = 0;
        w.VisualState = VisualStates.Transition;
        // pass a unique ID to this card.
        IDHolder id = card.AddComponent<IDHolder>();
        id.UniqueID = UniqueID;

        // save this card to change its state to "Hand" when it arrives to the hand.
        LastDealtCard = card;

        // move card to the hand;
        Sequence s = DOTween.Sequence();
        if (!fast)
        {
            Debug.Log ("Not fast!!!");
            s.Append(card.transform.DOMove(DrawPreviewSpot.position, GlobalSettings.Instance.CardTransitionTime));
            if (TakeCardsOpenly)
                s.Insert(0f, card.transform.DORotate(Vector3.zero, GlobalSettings.Instance.CardTransitionTime)); 
            else 
                s.Insert(0f, card.transform.DORotate(new Vector3(0f, 179f, 0f), GlobalSettings.Instance.CardTransitionTime)); 
            s.AppendInterval(GlobalSettings.Instance.CardPreviewTime);
            // displace the card so that we can select it in the scene easier.
            s.Append(card.transform.DOLocalMove(slots.Children[0].transform.localPosition, GlobalSettings.Instance.CardTransitionTime));
        }
        else
        {
            // displace the card so that we can select it in the scene easier.
            s.Append(card.transform.DOLocalMove(slots.Children[0].transform.localPosition, GlobalSettings.Instance.CardTransitionTimeFast));
            if (TakeCardsOpenly)    
                s.Insert(0f,card.transform.DORotate(Vector3.zero, GlobalSettings.Instance.CardTransitionTimeFast)); 
        }

        s.OnComplete(()=>ChangeLastCardStatusToInHand(card, w));
    }

    void ChangeLastCardStatusToInHand(GameObject card, WhereIsTheCardOrCreature w)
    {
        //Debug.Log("Changing state to Hand for card: " + card.gameObject.name);
        if (owner == AreaPosition.Low)
            w.VisualState = VisualStates.LowHand;
        else
            w.VisualState = VisualStates.TopHand;

        w.SetHandSortingOrder();
        Command.CommandExecutionComplete();
    }

    GameObject CreateACardAtPosition(CardAsset c, Vector3 position, Vector3 eulerAngles)
    {
        // Instantiate a card depending on its type
        GameObject card;
        if (c.Defensa > 0)
        {
            // this card is a creature card
            card = GameObject.Instantiate(GlobalSettings.Instance.CreatureCardPrefab, position, Quaternion.Euler(eulerAngles)) as GameObject;
        }
        else
        {
            // this is a spell: checking for targeted or non-targeted spell
            if (c.Targets == TargetingOptions.NoTarget)
                card = GameObject.Instantiate(GlobalSettings.Instance.NoTargetSpellCardPrefab, position, Quaternion.Euler(eulerAngles)) as GameObject;
            else
            {
                card = GameObject.Instantiate(GlobalSettings.Instance.TargetedSpellCardPrefab, position, Quaternion.Euler(eulerAngles)) as GameObject;
                // pass targeting options to DraggingActions
                DragSpellOnTarget dragSpell = card.GetComponentInChildren<DragSpellOnTarget>();
                dragSpell.Targets = c.Targets;
            }
                
        }

        // apply the look of the card based on the info from CardAsset
        OneCardManager manager = card.GetComponent<OneCardManager>();
        manager.cardAsset = c;
        manager.ReadCardFromAsset();

        return card;
    }

}
