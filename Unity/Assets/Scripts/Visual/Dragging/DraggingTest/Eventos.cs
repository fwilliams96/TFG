using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Eventos : MonoBehaviour {

    #region Atributos
    // PRIVATE FIELDS

    // a flag to know if we are currently dragging this GameObject
    private bool dragging;
    private float t0;

    // distance from the center of this Game Object to the point where we clicked to start dragging 
    private Vector3 pointerDisplacement;

    // distance from camera to mouse on Z axis 
    private float zDisplacement;

    // reference to DraggingActions script. Dragging Actions should be attached to the same GameObject.
    private DraggingActions da;

    private OpcionesObjeto opciones;

    // STATIC property that returns the instance of Draggable that is currently being dragged
    private static Eventos _draggingThis;

    public Vector2 startPos;
    public Vector2 direction;

    public Text m_Text;
    string message;

    #endregion

    public static Eventos DraggingThis
    {
        get { return _draggingThis; }
    }

    // MONOBEHAVIOUR METHODS
    void Awake()
    {
        da = GetComponent<DraggingActions>();
        opciones = GetComponent<OpcionesObjeto>();
        dragging = false;
}

    void OnMouseDown()
    {
        Debug.Log("OnMouseDown");
        /*if (da != null)
        {
            zDisplacement = -Camera.main.transform.position.z + transform.position.z;
            pointerDisplacement = -transform.position + MouseInWorldCoords();
        }*/
        
    }

    // Update is called once per frame
    /*void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            t0 = Time.time;
            Debug.Log("Mouse button down");
        }
        if (dragging)
        {
            Desplazamiento();
        }
        else
        {
            //OPTIONAL el tiempo hace que tarde un poquito en ser arrastrado
            if (da.SePuedeArrastrar && Input.GetMouseButton(0) && (Time.time - t0) > 0.0f)
            {
                dragging = true;
                OpcionesObjeto.PrevisualizacionesPermitidas = false;
                da.OnStartDrag();
                _draggingThis = this;
                //zDisplacement = -Camera.main.transform.position.z + transform.position.z;
                //pointerDisplacement = -transform.position + MouseInWorldCoords();
                Desplazamiento();
            }
        }
        
        
        //if (Input.GetMouseButtonUp(0))
        //{
          //  Debug.Log("Mouse button up");
        //}
    }*/

    private void OnMouseDrag()
    {
        Debug.Log("OnMouseDrag");
    }

    void OnMouseUp()
    {
        Debug.Log("OnMouseUp");
        /*if (dragging)
        {
            Debug.Log("Dragging");
            dragging = false;
            OpcionesObjeto.PrevisualizacionesPermitidas = true;
            da.OnEndDrag();
        }
        else
        {
            Debug.Log("Click normal");
            //opciones.MostrarOpciones();
            //Mostrar popup
        }*/
        
    }   

    // returns mouse position in World coordinates for our GameObject to follow. 
    private Vector3 MouseInWorldCoords()
    {
        var screenMousePos = Input.mousePosition;
        //Debug.Log(screenMousePos);
        screenMousePos.z = zDisplacement;
        return Camera.main.ScreenToWorldPoint(screenMousePos);
    }

    private void Desplazamiento()
    {
        Debug.Log("Manteniendo pulsado");
        Vector3 mousePos = MouseInWorldCoords();
        //Debug.Log(mousePos);
        transform.position = new Vector3(mousePos.x - pointerDisplacement.x, mousePos.y - pointerDisplacement.y, transform.position.z);
        da.OnDraggingInUpdate();
    }

    void Update()
    {
        //Update the Text on the screen depending on current TouchPhase, and the current direction vector
        m_Text.text = "Touch : " + message + "in direction" + direction;

        // Track a single touch as a direction control.
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Handle finger movements based on TouchPhase
            switch (touch.phase)
            {
                //When a touch has first been detected, change the message and record the starting position
                case TouchPhase.Began:
                    // Record initial touch position.
                    startPos = touch.position;
                    message = "Begun ";
                    break;

                //Determine if the touch is a moving touch
                case TouchPhase.Moved:
                    // Determine direction by comparing the current touch position with the initial one
                    direction = touch.position - startPos;
                    message = "Moving ";
                    break;

                case TouchPhase.Ended:
                    // Report that the touch has ended when it ends
                    message = "Ending ";
                    break;
            }
        }
    }
}
