using UnityEngine;
using System.Collections;

public abstract class DraggingActions : MonoBehaviour {

    public abstract void OnStartDrag();

    public abstract void OnEndDrag();

    public abstract void OnDraggingInUpdate();

    public virtual bool SePuedeArrastrar
    {
        get
        {
            if (tag.Contains("Card"))
            {
                return Controlador.Instance.SePermiteControlarElJugador(playerOwner);
            }
            else
            {
                return Controlador.Instance.SePermiteControlarElJugador(playerOwner) && !Controlador.Instance.EsMagica(GetComponent<IDHolder>().UniqueID);
            }
            
        }
    }

    protected virtual Jugador playerOwner
    {
        get{
            
            if (tag.Contains("Low"))
                return DatosGenerales.Instance.LowPlayer;
            else if (tag.Contains("Top"))
                return DatosGenerales.Instance.TopPlayer;
            else
            {
                Debug.LogError("Untagged Card or creature " + transform.parent.name);
                return null;
            }
        }
    }

    protected abstract bool DragSuccessful();

    public virtual GameObject FindTarget()
    {
        GameObject Target = null;
        RaycastHit[] hits;
        // TODO: raycast here anyway, store the results in 
        hits = Physics.RaycastAll(origin: Camera.main.transform.position,
            direction: (-Camera.main.transform.position + this.transform.position).normalized,
            maxDistance: 30f);

        //OPTIONAL quitar el for y hacer una busqueda
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
        return Target;
    }
}
