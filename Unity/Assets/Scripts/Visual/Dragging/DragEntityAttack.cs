using UnityEngine;
using System.Collections;

public class DragEntityAttack : DraggingActions {

    #region Atributos
    // reference to the sprite with a round "Target" graphic
    private SpriteRenderer sr;
    // LineRenderer that is attached to a child game object to draw the arrow
    private LineRenderer lr;
    // reference to WhereIsTheCardOrCreature to track this object`s state in the game
    private WhereIsTheCardOrEntity dondeEstaCartaOCriatura;
    // the pointy end of the arrow, should be called "Triangle" in the Hierarchy
    private Transform triangle;
    // SpriteRenderer of triangle. We need this to disable the pointy end if the target is too close.
    private SpriteRenderer triangleSR;
    // when we stop dragging, the gameObject that we were targeting will be stored in this variable.
    private GameObject Target;
    // Reference to creature manager, attached to the parent game object
    private OneCreatureManager manager;
    #endregion
    void Awake()
    {
        // establish all the connections
        sr = GetComponent<SpriteRenderer>();
        lr = GetComponentInChildren<LineRenderer>();
        lr.sortingLayerName = "AboveEverything";
        triangle = transform.Find("Triangle");
        triangleSR = triangle.GetComponent<SpriteRenderer>();

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
            int idEnte = GetComponentInParent<IDHolder>().UniqueID;
            try
            {
                return base.SePuedeArrastrar && !Controlador.Instance.EsMagica(idEnte) && Controlador.Instance.EstaEnPosicionAtaque(idEnte) && manager.PuedeAtacar;
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
        // enable target graphic
        sr.enabled = true;
        // enable line renderer to start drawing the line.
        lr.enabled = true;
    }

    public override void OnDraggingInUpdate()
    {
        Vector3 notNormalized = transform.position - transform.parent.position;
        Vector3 direction = notNormalized.normalized;
        float distanceToTarget = (direction*2.3f).magnitude;
        if (notNormalized.magnitude > distanceToTarget)
        {
            // draw a line between the creature and the target
            lr.SetPositions(new Vector3[]{ transform.parent.position, transform.position - direction*2.3f });
            lr.enabled = true;

            // position the end of the arrow between near the target.
            triangleSR.enabled = true;
            triangleSR.transform.position = transform.position - 1.5f*direction;

            // proper rotarion of arrow end
            float rot_z = Mathf.Atan2(notNormalized.y, notNormalized.x) * Mathf.Rad2Deg;
            triangleSR.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        }
        else
        {
            // if the target is not far enough from creature, do not show the arrow
            lr.enabled = false;
            triangleSR.enabled = false;
        }
            
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

        if (Target != null)
        {
            int targetID = Target.GetComponent<IDHolder>().UniqueID;
            Debug.Log("Target ID: " + targetID);
            if (Recursos.EntesCreadosEnElJuego[targetID] != null)
            {
                // if targeted creature is still alive, attack creature
                Controlador.Instance.AtacarCriatura(GetComponentInParent<IDHolder>().UniqueID, targetID);
                Debug.Log("Attacking "+Target);
            }
                
        }
        // not a valid target, return
        if(tag.Contains("Low"))
            dondeEstaCartaOCriatura.EstadoVisual = VisualStates.MesaJugadorAbajo;
        else
            dondeEstaCartaOCriatura.EstadoVisual = VisualStates.MesaJugadorArriba;
        dondeEstaCartaOCriatura.SetearOrdenCriatura();

        // return target and arrow to original position
        transform.localPosition = Vector3.zero;
        sr.enabled = false;
        lr.enabled = false;
        triangleSR.enabled = false;

    }

    // NOT USED IN THIS SCRIPT
    protected override bool DragSuccessful()
    {
        return true;
    }
}
