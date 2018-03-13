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
            Debug.Log("Toque");
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
                    //Creamos un plano con la normal y la posicion del objeto intersectado
                    objPlane = new Plane(Camera.main.transform.forward * -1, gObj.transform.position);

                    //calc touch offset
                    Ray mRay = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                    float rayDistance;
                    objPlane.Raycast(mRay, out rayDistance);
                    m0 = gObj.transform.position - mRay.GetPoint(rayDistance);

                }
            }
            else if(Input.GetTouch(0).phase == TouchPhase.Moved && gObj)
            {
                Ray mRay = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                float rayDistance;
                if(objPlane.Raycast(mRay, out rayDistance))
                {
                    gObj.transform.position = mRay.GetPoint(rayDistance) + m0;
                }
            }
            else if(Input.GetTouch(0).phase == TouchPhase.Ended && gObj)
            {
                gObj = null;
            }
        }
    }


}
