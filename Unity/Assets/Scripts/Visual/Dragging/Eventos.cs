﻿using UnityEngine;

public class Eventos : MonoBehaviour {

    #region Atributos
    // PRIVATE FIELDS
    private static Eventos _draggingThis;
    private bool dragging;
    private DraggingActions da;
    private Plane objPlane;
    private Vector3 m0;
    #endregion

    public static Eventos DraggingThis
    {
        get { return _draggingThis; }
    }

    // MONOBEHAVIOUR METHODS
    void Awake()
    {
        da = GetComponent<DraggingActions>();
        dragging = false;
    }

	/// <summary>
	/// Click sobre el ente o carta.
	/// </summary>
    public void Click()
    {
        //Creamos un plano con la normal y la posicion del objeto intersectado
        
        objPlane = new Plane(Camera.main.transform.forward * -1, transform.position);

        //calc touch offset
        Ray mRay = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        float rayDistance;
        objPlane.Raycast(mRay, out rayDistance);
        m0 = transform.position - mRay.GetPoint(rayDistance);
    }

	/// <summary>
	/// Se mantiene clicada la carta o ente.
	/// </summary>
	public void Still(){
		if (da != null) {
			if (!da.SePuedeArrastrar && dragging) { 
				if(!da.Reset)
					da.resetDragg ();
			}
		}

	}

	/// <summary>
	/// Se mueve la carta o ente.
	/// </summary>
    public void Dragg()
    {
		if (da != null) {
			if (da.SePuedeArrastrar) { 
				if (!dragging) {
					dragging = true;
					OpcionesObjeto.PrevisualizacionesPermitidas = false;
					_draggingThis = this;
					da.OnStartDrag ();
				}
	                
				Ray mRay = Camera.main.ScreenPointToRay (Input.GetTouch (0).position);
				float rayDistance;
				if (objPlane.Raycast (mRay, out rayDistance)) {
					transform.position = mRay.GetPoint (rayDistance) + m0;
				} 
				da.OnDraggingInUpdate ();
			} else {
				if (dragging && !da.Reset) {
					da.resetDragg ();
				}

			}
		}
        //mostrar previsualizacion
    }

	/// <summary>
	/// Se suelta la carta o ente.
	/// </summary>
    public void End()
    {
		if (!dragging) {
            //Mostrar previsualizacion y menu
			if(da.SePuedeControlar)
            	GetComponent<OpcionesObjeto>().MostrarOpciones();
        }
        else
        {
            OpcionesObjeto.PrevisualizacionesPermitidas = true;
            _draggingThis = null;
			if (da.SePuedeArrastrar)
				da.OnEndDrag ();

            dragging = false;
        }
        
    }
    
}
