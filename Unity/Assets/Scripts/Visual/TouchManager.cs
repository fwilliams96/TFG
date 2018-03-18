using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour {

    GameObject gObj;
    Plane objPlane;
    Vector3 m0;

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
                if(Physics.Raycast(mouseRay.origin,mouseRay.direction,out hit))
                {
                    //Cogemos el objeto
                    gObj = hit.transform.gameObject;
                    if(gObj.GetComponent<Eventos>() != null)
                    {
                        gObj.GetComponent<Eventos>().Click();
                    }
                    else
                    {
                       
                        Debug.Log("Ningun gameobject con evento tocado "+gObj.name);
                        gObj = null;
                        //if (OpcionesObjeto.PrevisualizandoAlgunaCarta())
                            //OpcionesObjeto.PararTodasPrevisualizaciones();
                    }
                    

                }
            }
            else if(Input.GetTouch(0).phase == TouchPhase.Moved && gObj)
            {
                /*if (gObj.GetComponent<Eventos>() != null)
                {
                    gObj.GetComponent<Eventos>().Click();
                }*/
                gObj.GetComponent<Eventos>().Dragg();
            }
            else if(Input.GetTouch(0).phase == TouchPhase.Ended && gObj)
            {
                gObj.GetComponent<Eventos>().End();
                gObj = null;
            }
        }
    }


}
