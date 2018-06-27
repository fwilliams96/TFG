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

    /// <summary>
    /// Añade la carta a la mano visual del jugador.
    /// </summary>
    /// <param name="card">Card.</param>
    public void AñadirCarta(GameObject card)
    {
        CartasEnMano.Insert(0, card);
        // set parent for this card
        card.transform.SetParent(slots.transform);
        // re-calculate the position of the hand
        MoverSlotCartas();
        ActualizarSlots();
    }

	/// <summary>
	/// Elimina la carta de la mano visual del jugador.
	/// </summary>
	/// <param name="card">Card.</param>
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

	/// <summary>
	/// Actualiza la posición de las cartas en la mano visual.
	/// </summary>
    void ActualizarSlots()
    {
        float posX;
        if (CartasEnMano.Count > 0)
            posX = (slots.Children[0].transform.localPosition.x - slots.Children[CartasEnMano.Count - 1].transform.localPosition.x) / 2f;
        else
            posX = 0f;

        slots.gameObject.transform.DOLocalMoveX(posX, 0.3f);
    }

	/// <summary>
	/// Desplaza las cartas de la mano para que quede centrado.
	/// </summary>
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

    /// <summary>
    /// Instancia la carta visual del jugador y la reparte.
    /// </summary>
    /// <param name="c">C.</param>
    /// <param name="UniqueID">Unique I.</param>
    /// <param name="fast">If set to <c>true</c> fast.</param>
    /// <param name="fromDeck">If set to <c>true</c> from deck.</param>
    /// <param name="rotarDeCara">If set to <c>true</c> rotar de cara.</param>
	public void DarCartaJugador(CartaPartida c, int UniqueID, bool fast = false, bool fromDeck = true, bool rotarDeCara = false)
    {
		if (!rotarDeCara)
			fast = true;
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
		if(card.GetComponent<AudioSource>() != null && ConfiguracionUsuario.Instance.Musica)
			card.GetComponent<AudioSource>().Play();
        // move card to the hand;
        Sequence s = DOTween.Sequence();
        if (!fast)
        {
            Debug.Log("Not fast!!!");
			s.Append(card.transform.DOMove(DrawPreviewSpot.position, ConfiguracionUsuario.Instance.CardTransitionTime));
				
			s.Insert (0f, card.transform.DORotate (Vector3.zero, ConfiguracionUsuario.Instance.CardTransitionTime));
				
			s.AppendInterval(ConfiguracionUsuario.Instance.CardPreviewTime);
            // displace the card so that we can select it in the scene easier.
			s.Append(card.transform.DOLocalMove(slots.Children[0].transform.localPosition, ConfiguracionUsuario.Instance.CardTransitionTime));
        }
        else
        {
            // displace the card so that we can select it in the scene easier.
			if (rotarDeCara) {
				s.Insert (0f, card.transform.DORotate (Vector3.zero, ConfiguracionUsuario.Instance.CardTransitionTime));
			}
			s.Append(card.transform.DOLocalMove(slots.Children[0].transform.localPosition, ConfiguracionUsuario.Instance.CardTransitionTime));
            //if (TakeCardsOpenly)
                //s.Insert(0f, card.transform.DORotate(Vector3.zero, DatosGenerales.Instance.CardTransitionTimeFast));
        }

        s.OnComplete(() => CambiarEstadoCartaAMano(card, w));
    }

	/// <summary>
	/// Resetea el estado visual de la carta de la mano.
	/// </summary>
	/// <param name="card">Card.</param>
	/// <param name="w">The width.</param>
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

	/// <summary>
	/// Devuelve una carta a partir de la posición indexada en la mano del jugador.
	/// </summary>
	/// <returns>The carta por posicion.</returns>
	/// <param name="c">C.</param>
	/// <param name="position">Position.</param>
	/// <param name="eulerAngles">Euler angles.</param>
	GameObject CrearCartaPorPosicion(CartaPartida c, Vector3 position, Vector3 eulerAngles)
    {
        // Instantiate a card depending on its type
        GameObject card;
        // this card is a creature card
        card = GameObject.Instantiate(ObjetosGenerales.Instance.CardPrefab, position, Quaternion.Euler(eulerAngles)) as GameObject;


        // apply the look of the card based on the info from CardAsset
        OneCardManager manager = card.GetComponent<OneCardManager>();
        manager.CartaAsset = c.AssetCarta;
        manager.LeerDatos();

        return card;
    }

    public void OcultarMano()
    {
        GirarMano(180);
    }

    public void MostrarMano()
    {
        GirarMano(0);
    }

	/// <summary>
	/// Gira la mano en el eje Y, lo que permite poner toda la mano boca abajo o boca arriba (No se usa).
	/// </summary>
	/// <param name="grado">Grado.</param>
    private void GirarMano(int grado)
    {
        foreach (GameObject carta in CartasEnMano)
        {
            float x = carta.transform.eulerAngles.x;
            float y = grado;
            float z = carta.transform.eulerAngles.z;
            carta.transform.DORotate(new Vector3(x, y, z), ConfiguracionUsuario.Instance.CardTransitionTime);
        }
    }

}
