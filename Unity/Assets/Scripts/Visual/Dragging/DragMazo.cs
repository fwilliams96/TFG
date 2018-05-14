using UnityEngine;
using System.Collections;
using DG.Tweening;

public class DragMazo : DraggingActions {

    #region Atributos
	private Vector3 oldCardPos;
	private TablaCartas tablaActual;

	private Plane objPlane;
	private Vector3 m0;
    #endregion
    void Awake()
    {
        
    }

    public override void OnStartDrag()
    {
		objPlane = new Plane(Camera.main.transform.forward * -1, transform.position);

		//calc touch offset
		Ray mRay = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
		float rayDistance;
		objPlane.Raycast(mRay, out rayDistance);
		m0 = transform.position - mRay.GetPoint(rayDistance);
		oldCardPos = transform.position;
		reset = false;
    }

    public override void OnDraggingInUpdate()
    {
		Ray mRay = Camera.main.ScreenPointToRay (Input.GetTouch (0).position);
		float rayDistance;
		if (objPlane.Raycast (mRay, out rayDistance)) {
			transform.position = mRay.GetPoint (rayDistance) + m0;
		} 
    }

    public override void OnEndDrag()
    {
		if (DragSuccessful ()) {
			this.tablaActual.AñadirCarta (this.gameObject);

		} else {
			resetDragg ();
		}

    }

	public override void resetDragg(){
		transform.DOLocalMove(oldCardPos, 1f);
		reset = true;
	}

	protected override bool DragSuccessful()
	{
		TablaCartas tablaActual = TablaCartas.tablaActual ();
		bool success = false;
		if (tablaActual != null) {
			this.tablaActual = tablaActual;
			if (tablaActual.gameObject.tag.Equals ("TablaMazo")) {
				if (tablaActual.NumElementos() < 8)
					success = true;
			} else {
				success = true;
			}
		}
		return success;

	}
}
