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

    public override bool SePuedeArrastrar
    {
        get
        {
            //return true;
            //manage es si está con glow
            return base.SePuedeArrastrar && manager.PuedeSerJugada;
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
        tempState = whereIsCard.EstadoVisual;
        whereIsCard.EstadoVisual = VisualStates.Arrastrando;
        whereIsCard.TraerAlFrente();
		reset = false;

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
			resetDragg ();
        }
        //whereIsCard.SetearOrdenCarta();
       // whereIsCard.EstadoVisual = VisualStates.ManoJugadorAbajo;
    }

    public void ColocarCartaTablero(bool resultOK)
    {
        //Se ha seleccionado ataque o defensa en el popup
        if (resultOK)
        {
            bool magica = manager.TypeText.text.Equals("Magica");

            // determine table position
            int tablePos = Controlador.Instance.AreaJugador(playerOwner).tableVisual.PosicionSlotNuevaCriatura(Camera.main.ScreenToWorldPoint(
                    new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, transform.position.z - Camera.main.transform.position.z)).x);
            //new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z - Camera.main.transform.position.z)).x);
            // Debug.Log("Table Pos for new Creature: " + tablePos.ToString());
            // play this card
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

    protected override bool DragSuccessful()
    {
        bool TableNotFull = (playerOwner.NumEntesEnLaMesa() < 6);

        return TableVisual.CursorSobreAlgunaMesa && TableNotFull;
    }

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
