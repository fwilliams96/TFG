using UnityEngine;
using System.Collections;

public class DragMagicEffect : DraggingActions {

    #region Atributos
    // reference to WhereIsTheCardOrCreature to track this object`s state in the game
    private WhereIsTheCardOrEntity dondeEstaCartaOCriatura;
    // the pointy end of the arrow, should be called "Triangle" in the Hierarchy
    // when we stop dragging, the gameObject that we were targeting will be stored in this variable.
    private GameObject Target;
    // Reference to creature manager, attached to the parent game object
    private OneCreatureManager manager;
    #endregion
    void Awake()
    {

        manager = GetComponentInParent<OneCreatureManager>();
        dondeEstaCartaOCriatura = GetComponentInParent<WhereIsTheCardOrEntity>();
    }

    public override bool SePuedeArrastrar
    {
        get
        {
            // we can drag this card if 
            // a) we can control this our player (this is checked in base.canDrag)
            // b) creature "CanAttackNow" - this info comes from logic part of our code into each creature`s manager script
            //return base.SePuedeArrastrar && manager.PuedeAtacar;
            return false;
           
        }
    }
		
    public override void OnStartDrag()
    {
        dondeEstaCartaOCriatura.EstadoVisual = VisualStates.Arrastrando;
		reset = false;
    }

    public override void OnDraggingInUpdate()
    {
            
    }
	/// <summary>
	/// Restablece el estado visual de la mágica.
	/// </summary>
    public override void OnEndDrag()
    {
        if(tag.Contains("Low"))
            dondeEstaCartaOCriatura.EstadoVisual = VisualStates.MesaJugadorAbajo;
        else
            dondeEstaCartaOCriatura.EstadoVisual = VisualStates.MesaJugadorArriba;
        dondeEstaCartaOCriatura.SetearOrdenCriatura();

    }

	public override void resetDragg(){
		reset = true;
	}

    // NOT USED IN THIS SCRIPT
    protected override bool DragSuccessful()
    {
        return true;
    }
}
