using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class TouchManager2 : MonoBehaviour {

	public static TouchManager2 Instance;
    GameObject gObj;
    Plane objPlane;
    Vector3 m0;
	GameObject current;
	public GameObject ObjetoActual {
		get {
			return gObj;
		}
		set{
			gObj = value;
		}
	}

	private void Awake()
	{
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		
	}
		
	Ray GenerateMouseRay(Vector3 touchPos)
	{
		Vector3 mousePosFar = new Vector3(touchPos.x, touchPos.y, Camera.main.farClipPlane);
		Vector3 mousePosNear = new Vector3(touchPos.x, touchPos.y, Camera.main.nearClipPlane);

		Vector3 mousePosF = Camera.main.ScreenToWorldPoint(mousePosFar);
		Vector3 mousePosN = Camera.main.ScreenToWorldPoint(mousePosNear);

		Ray mr = new Ray(mousePosN, mousePosF - mousePosN);
		return mr;
	}

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        {
            //Debug.Log("Toque");
            //Miramos si esta en la fase de tocar la pantalla
            if(Input.GetTouch(0).phase == TouchPhase.Began)
            {
                //A partir de la posición de pantalla del mouse generamos un rayo
                Ray mouseRay = GenerateMouseRay(Input.GetTouch(0).position);
                //Miramos con que objeto ha chocado el rayo
				var layerMask = (1 << 8);
				layerMask |= (1 << 9);
				RaycastHit2D hit = Physics2D.GetRayIntersection(mouseRay, Mathf.Infinity, layerMask);
				current = EventSystem.current.currentSelectedGameObject;
				if (hit.collider != null) {
					//TODO en funcion de la escena en la que nos encontremos haremos una cosa u otra
					//switch(Settings.Instance.EscenaActual)
					//TODO mejorar comprobación
					gObj = hit.collider.gameObject;
				}
            }
            else if(Input.GetTouch(0).phase == TouchPhase.Moved && gObj)
            {
				/*if (null != gObj) {
				}*/
				gObj = null;
            }
			else if(Input.GetTouch(0).phase == TouchPhase.Stationary)
			{
				if (null != gObj) {
				}

			}
            else if(Input.GetTouch(0).phase == TouchPhase.Ended)
            {
				if (null != gObj) {
					switch (ControladorMenu.Instance.PantallaActual) {
					case PANTALLA_MENU.INVENTARIO:
						if (gObj.transform.gameObject.tag.Equals ("CartaInventario")) {
							Acciones.Instance.MostrarAcciones (true, gObj);
						} else if (gObj.transform.gameObject.tag.Equals ("ItemInventario")) {
							//Acciones.Instance.MostrarAcciones (false);
						} else if (gObj.transform.gameObject.tag.Equals ("ItemConsumible")) {
							ControladorMenu.Instance.AgregarItemCarta (Acciones.Instance.ElementoActual.GetComponent<IDHolder> ().UniqueID, gObj.transform.gameObject.GetComponent<IDHolder> ().UniqueID);
						} else if (gObj.transform.gameObject.tag.Equals ("CartaConsumible")) {
							ControladorMenu.Instance.AgregarItemCarta (gObj.transform.gameObject.GetComponent<IDHolder> ().UniqueID, Acciones.Instance.ElementoActual.GetComponent<IDHolder> ().UniqueID);
						} else {
							ControladorMenu.Instance.CerrarAccion ();
						}
							break;
					case PANTALLA_MENU.MAZO:

							if (gObj.transform.gameObject.tag.Equals ("CartaFueraMazo") || gObj.transform.gameObject.tag.Equals ("CartaDentroMazo")) {
								ControladorMenu.Instance.MostrarAccion (gObj);
							} else {
								ControladorMenu.Instance.CerrarAccion ();
							}
							break;
						}
				} else {
					if (null != EventSystem.current.currentSelectedGameObject) {
						Debug.Log ("Event system");
					} else {
						
					}
				}
				gObj = null;

            }
        }
    }


}
