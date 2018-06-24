using UnityEngine;
using System.Collections;

public class DragCreatureAttack : DraggingActions {

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

	/// <summary>
	/// Determina si la carta puede controlarse por el jugador, si está disponible para moverse y si no se encuentra en defensa.
	/// </summary>
	/// <value><c>true</c> if se puede arrastrar; otherwise, <c>false</c>.</value>
    public override bool SePuedeArrastrar
    {
        get
        {
            try
            {

				return base.SePuedeControlar && manager.PuedeAtacar && Controlador.Instance.EstaEnPosicionAtaque(GetComponentInParent<IDHolder
                    >().UniqueID);
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
                return false;
            }
           
        }
    }
	/// <summary>
	/// Empieza el dragg de la criatura.
	/// </summary>
    public override void OnStartDrag()
    {
        dondeEstaCartaOCriatura.EstadoVisual = VisualStates.Arrastrando;
        // enable target graphic
        sr.enabled = true;
        // enable line renderer to start drawing the line.
        lr.enabled = true;
		reset = false;
    }

	/// <summary>
	/// Se muestra la flecha roja que permite apuntar al objetivo.
	/// </summary>
    public override void OnDraggingInUpdate()
    {
       
        Vector3 notNormalized = transform.position - transform.parent.position;
        Vector3 direction = notNormalized.normalized;
        float distanceToTarget = (direction * 2.3f).magnitude;
        if (notNormalized.magnitude > distanceToTarget)
        {
            // draw a line between the creature and the target
            lr.SetPositions(new Vector3[] { transform.parent.position, transform.position - direction * 2.3f });
            lr.enabled = true;

            // position the end of the arrow between near the target.
            triangleSR.enabled = true;
            triangleSR.transform.position = transform.position - 1.5f * direction;

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

	/// <summary>
	/// Determina si se ha apuntado a un enemigo correcto.
	/// </summary>
    public override void OnEndDrag()
    {
        Target = FindTarget();

        if (Target != null)
        {
            int targetID = Target.GetComponent<IDHolder>().UniqueID;
			if (targetID == Controlador.Instance.Local.ID || targetID == Controlador.Instance.Enemigo.ID) {
				if (Controlador.Instance.SePuedeAtacarJugadorDeCara (targetID)) {
					Controlador.Instance.AtacarJugador (GetComponentInParent<IDHolder> ().UniqueID, targetID);
				} else {
					new ShowMessageCommand ("Todavía tienes enemigos cerca...", 2.0f).AñadirAlaCola ();
				}
			} else {
				//Debug.Log("Target ID: " + targetID);
				if (Recursos.EntesCreadosEnElJuego[targetID] != null)
				{
					Controlador.Instance.AtacarEnte(GetComponentInParent<IDHolder>().UniqueID, targetID);
				}
			}
        }
		resetDragg ();
    }

	/// <summary>
	/// Retorna la flecha a su origen.
	/// </summary>
	public override void resetDragg(){
		if (tag.Contains("Low"))
			dondeEstaCartaOCriatura.EstadoVisual = VisualStates.MesaJugadorAbajo;
		else
			dondeEstaCartaOCriatura.EstadoVisual = VisualStates.MesaJugadorArriba;
		dondeEstaCartaOCriatura.SetearOrdenCriatura();

		// return target and arrow to original position
		transform.localPosition = Vector3.zero;
		sr.enabled = false;
		lr.enabled = false;
		triangleSR.enabled = false;
		reset = true;
	}

    //Se devuelve true porque no se usa.
    protected override bool DragSuccessful()
    {
        return true;
    }
}
