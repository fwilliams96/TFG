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
            try
            {
                return base.SePuedeArrastrar && manager.PuedeAtacar;
            }
            /*catch(EnteException e)
            {
                Debug.Log(e.GetMessage());
                return false;
            }*/
            catch (System.Exception e)
            {
                Debug.Log(e);
                return false;
            }
           
        }
    }

    public override void OnStartDrag()
    {
        dondeEstaCartaOCriatura.EstadoVisual = VisualStates.Arrastrando;
    }

    public override void OnDraggingInUpdate()
    {
            
    }

    public override void OnEndDrag()
    {
        Target = null;
        RaycastHit[] hits;
        // TODO: raycast here anyway, store the results in 
        hits = Physics.RaycastAll(origin: Camera.main.transform.position, 
            direction: (-Camera.main.transform.position + this.transform.position).normalized, 
            maxDistance: 30f) ;

        foreach (RaycastHit h in hits)
        {
            //TODO El tag this.tag == "LowEnte" o "TopEnte" debe cambiar a "LowCreature/TopCreature" nuevamente porque una magica no atacara de esta manera
            if ((h.transform.tag == "TopEnte" && this.tag == "LowEnte") ||
                    (h.transform.tag == "LowEnte" && this.tag == "TopEnte"))
            {
                // hit a creature, save parent transform
                Target = h.transform.parent.gameObject;
            }
               
        }
        int magicID = GetComponentInParent<IDHolder>().UniqueID;
        if (Target != null)
        {
            int targetID = Target.GetComponent<IDHolder>().UniqueID;
            
            Debug.Log("Target ID: " + targetID);
            if (magicID != targetID && Recursos.EntesCreadosEnElJuego[targetID] != null)
            {
                // if targeted creature is still alive, attack creature
                Controlador.Instance.ActivarEfecto(magicID);
                Debug.Log("Attacking "+Target);
            }

        }
        else
        {
            // if targeted creature is still alive, attack creature
            Controlador.Instance.ActivarEfecto(magicID);
            Debug.Log("Attacking " + Target);
        }
        // not a valid target, return
        if(tag.Contains("Low"))
            dondeEstaCartaOCriatura.EstadoVisual = VisualStates.MesaJugadorAbajo;
        else
            dondeEstaCartaOCriatura.EstadoVisual = VisualStates.MesaJugadorArriba;
        dondeEstaCartaOCriatura.SetearOrdenCriatura();

        // return target and arrow to original position
        //transform.localPosition = Vector3.zero;

    }

    // NOT USED IN THIS SCRIPT
    protected override bool DragSuccessful()
    {
        return true;
    }
}
