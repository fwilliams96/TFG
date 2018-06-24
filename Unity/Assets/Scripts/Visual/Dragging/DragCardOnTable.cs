using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class DragCardOnTable : DraggingActions
{
    #region Atributos
    private int savedHandSlot;
    private WhereIsTheCardOrEntity whereIsCard;
    private IDHolder idScript;
    private VisualStates tempState;
    private OneCardManager manager;
    #endregion
	/// <summary>
	/// Identifica si una carta se puede controlar (segun el jugador) y si la carta esta disponible para ser usada.
	/// </summary>
	/// <value><c>true</c> if se puede arrastrar; otherwise, <c>false</c>.</value>
    public override bool SePuedeArrastrar
    {
        get
        {
            //return true;
            //manage es si está con glow
			return base.SePuedeControlar && manager.PuedeSerJugada;
        }
    }

    void Awake()
    {
        whereIsCard = GetComponent<WhereIsTheCardOrEntity>();
        manager = GetComponent<OneCardManager>();
    }

	/// <summary>
	/// Comienza el drag de la carta.
	/// </summary>
    public override void OnStartDrag()
    {
        savedHandSlot = whereIsCard.Slot;
        tempState = whereIsCard.EstadoVisual;
        whereIsCard.EstadoVisual = VisualStates.Arrastrando;
        whereIsCard.TraerAlFrente();
		reset = false;

    }

    public override void OnDraggingInUpdate()
    {

    }

	/// <summary>
	/// Acaba el dragg de la carta, determinando si se ha soltado en el sitio correspondiente.
	/// </summary>
    public override void OnEndDrag()
    {

        // 1) Check if we are holding a card over the table
        if (DragSuccessful())
        {
			bool magica = manager.CartaAsset.Familia.Equals (Familia.Magica);
            //Activar pop-up de tipo de posicion
			if (!magica)
            {
                this.gameObject.SetActive(false);
                PosicionCriaturaPopUp.Instance.MostrarPopupEleccionPosicion();
                PosicionCriaturaPopUp.Instance.RegistrarCallBack(ColocarCartaTablero);
            }
            //En caso de una carta magica, no existe posicion de defensa, la colocamos directamente
            else
            {
                ColocarCartaTablero(true);
            }

        }
        else
        {
			bool TableFull = (playerOwner.NumEntesEnLaMesa() == 5);
			if (TableFull) {
				new ShowMessageCommand ("¡Tu campo de batalla está lleno!", 2.0f).AñadirAlaCola ();
			} else {
				new ShowMessageCommand ("No te alejes del campo de batalla...", 2.0f).AñadirAlaCola ();
			}
			resetDragg ();
        }
    }

	/// <summary>
	/// Coloca una carta en el territorio.
	/// </summary>
	/// <param name="resultOK">If set to <c>true</c> result O.</param>
    public void ColocarCartaTablero(bool resultOK)
    {
        //Se ha seleccionado ataque o defensa en el popup
        if (resultOK)
        {
			bool magica = manager.CartaAsset.Familia.Equals (Familia.Magica);
            // determine table position
            int tablePos = Controlador.Instance.AreaJugador(playerOwner).tableVisual.PosicionSlotNuevaEnte(Camera.main.ScreenToWorldPoint(
                    new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, transform.position.z - Camera.main.transform.position.z)).x);
            if (magica)
            {
				Controlador.Instance.JugarMagicaMano(playerOwner,GetComponent<IDHolder>().UniqueID, tablePos);
            }
            else
            {
                bool ataque = PosicionCriaturaPopUp.Instance.Ataque;
                Debug.Log("ColocarCartaTablero ataque " + ataque);
				Controlador.Instance.JugarCartaMano(playerOwner,GetComponent<IDHolder>().UniqueID, tablePos, ataque);
            }

        }
        //Se ha cancelado el popup
        else
        {
            this.gameObject.SetActive(true);
			resetDragg ();
        }

    }

	/// <summary>
	/// Determina si la carta se ha soltado en el sitio correcto y que no se encuentre lleno.
	/// </summary>
	/// <returns><c>true</c>, if successful was draged, <c>false</c> otherwise.</returns>
    protected override bool DragSuccessful()
    {
        bool TableNotFull = (playerOwner.NumEntesEnLaMesa() < 5);

		PlayerArea area = Controlador.Instance.AreaJugador (playerOwner);

		return area.tableVisual.CursorSobreEstaMesa && TableNotFull;
    }

	/// <summary>
	/// Mueve la carta a su sitio de origen.
	/// </summary>
	public override void resetDragg(){
        // Set old sorting order 
        whereIsCard.SetearOrdenCarta();
        whereIsCard.EstadoVisual = tempState;
        // Move this card back to its slot position
        HandVisual PlayerHand = Controlador.Instance.AreaJugador(playerOwner).manoVisual;
        Vector3 oldCardPos = PlayerHand.slots.Children[savedHandSlot].transform.localPosition;
        //Se usa local move porque a veces puede estar este script en target y alli si pillamos transform.position pillariamos la dl padre
        transform.DOLocalMove(oldCardPos, 1f);
		reset = true;
    }
}
