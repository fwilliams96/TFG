using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class DragEntityOnTable : DraggingActions
{

    private int savedHandSlot;
    private WhereIsTheCardOrCreature whereIsCard;
    private IDHolder idScript;
    private VisualStates tempState;
    private OneCardManager manager;

    public override bool PuedeSerLanzada
    {
        get
        {
            //return true;
            //manage es si está con glow
            return base.PuedeSerLanzada && manager.PuedeSerJugada;
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
        tempState = whereIsCard.EstadoVisual;
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
            VolverALaMano();
        }
    }

    public void ColocarCartaTablero(bool resultOK)
    {
        //Se ha seleccionado ataque o defensa en el popup
        if (resultOK)
        {
            bool magica = manager.TypeText.text.Equals("Magica");

            // determine table position
            int tablePos = playerOwner.PArea.tableVisual.PosicionSlotNuevaCriatura(Camera.main.ScreenToWorldPoint(
                    new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z - Camera.main.transform.position.z)).x);
            // Debug.Log("Table Pos for new Creature: " + tablePos.ToString());
            // play this card
            if (magica)
            {
                Controlador.Instance.JugarMagicaMano(GetComponent<IDHolder>().UniqueID, tablePos);
            }
            else
            {
                bool ataque = PosicionCriaturaPopUp.Instance.Ataque;
                Debug.Log("ColocarCartaTablero ataque " + ataque);
                Controlador.Instance.JugarCartaMano(GetComponent<IDHolder>().UniqueID, tablePos, ataque);
            }

        }
        //Se ha cancelado el popup
        else
        {
            this.gameObject.SetActive(true);
            VolverALaMano();
        }

    }

    protected override bool DragSuccessful()
    {
        bool TableNotFull = (playerOwner.NumCriaturasEnLaMesa() < 8);

        return TableVisual.CursorSobreAlgunaMesa && TableNotFull;
    }

    private void VolverALaMano()
    {
        // Set old sorting order 
        whereIsCard.SetearOrdenCarta();
        whereIsCard.EstadoVisual = tempState;
        // Move this card back to its slot position
        HandVisual PlayerHand = playerOwner.PArea.manoVisual;
        Vector3 oldCardPos = PlayerHand.slots.Children[savedHandSlot].transform.localPosition;
        transform.DOLocalMove(oldCardPos, 1f);
    }
}
