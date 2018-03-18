using UnityEngine;

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

    public void Click()
    {
        Debug.Log("Click");
        //Creamos un plano con la normal y la posicion del objeto intersectado
        
        objPlane = new Plane(Camera.main.transform.forward * -1, transform.position);

        //calc touch offset
        Ray mRay = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        float rayDistance;
        objPlane.Raycast(mRay, out rayDistance);
        m0 = transform.position - mRay.GetPoint(rayDistance);
    }

    public void Dragg()
    {
        Debug.Log("Dragg");
        if (da != null && da.SePuedeArrastrar) { 
            if (!dragging)
            {
                dragging = true;
                OpcionesObjeto.PrevisualizacionesPermitidas = false;
                _draggingThis = this;
                da.OnStartDrag();
            }
                
            Ray mRay = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            float rayDistance;
            if (objPlane.Raycast(mRay, out rayDistance))
            {
                transform.position = mRay.GetPoint(rayDistance) + m0;
            }
            da.OnDraggingInUpdate();
        }
        //mostrar previsualizacion
    }

    public void End()
    {
        Debug.Log("End");
        if (!dragging) {
            //Mostrar previsualizacion y menu
            GetComponent<OpcionesObjeto>().MostrarOpciones();
        }
        else
        {
            OpcionesObjeto.PrevisualizacionesPermitidas = true;
            _draggingThis = null;
            da.OnEndDrag();
            dragging = false;
        }
        
    }
    
}
