using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class TouchManager2 : MonoBehaviour {

	public static TouchManager2 Instance;
	public GameObject acciones;
    GameObject gObj;
    Plane objPlane;
    Vector3 m0;

	public GameObject ObjetoActual {
		get {
			return gObj;
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
                RaycastHit hit;
                //Miramos con que objeto ha chocado el rayo
				RaycastHit2D hit2 = Physics2D.GetRayIntersection(mouseRay);
				//bool hayHit = Physics2D.Raycast(mouseRay.origin,mouseRay.direction,out hit);
                
				if (hit2.collider != null)
                {
                    //TODO en funcion de la escena en la que nos encontremos haremos una cosa u otra
					//switch(Settings.Instance.EscenaActual)
					if (!acciones.activeSelf) {
						gObj = hit2.transform.gameObject;
						if (gObj.tag.Equals("CartaInventario")) {
							if(acciones != null)
								acciones.SetActive (true);
						}
					}
                    
                    
                }
            }
            else if(Input.GetTouch(0).phase == TouchPhase.Moved && gObj)
            {

            }
            else if(Input.GetTouch(0).phase == TouchPhase.Ended && gObj)
            {
            }
        }
    }


}
