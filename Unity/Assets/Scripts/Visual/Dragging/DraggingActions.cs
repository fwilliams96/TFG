using UnityEngine;
using System.Collections;

public abstract class DraggingActions : MonoBehaviour {

    public abstract void OnStartDrag();

    public abstract void OnEndDrag();

    public abstract void OnDraggingInUpdate();

	public abstract void resetDragg();

	protected bool reset;

	public bool Reset {
		get {
			return reset;
		}
	}

	public virtual bool SePuedeArrastrar{
		get{
			return SePuedeControlar;
		}
	}

	/// <summary>
	/// Determina si el jugador puede controlar el ente o carta.
	/// </summary>
	/// <value><c>true</c> if se puede controlar; otherwise, <c>false</c>.</value>
	public virtual bool SePuedeControlar
    {
        get
        {
			if(playerOwner != null)
				return Controlador.Instance.SePermiteControlarElJugador(playerOwner);
			return false;
        }
    }

	/// <summary>
	/// Devuelve el dueño del ente o carta.
	/// </summary>
	/// <value>The player owner.</value>
    protected virtual JugadorPartida playerOwner
    {
        get{
			try{
				
	            if (tag.Contains("Low"))
	                return Controlador.Instance.Local;
	            else if (tag.Contains("Top"))
	                return Controlador.Instance.Enemigo;
	            else
	            {
	                Debug.LogError("Untagged Card or creature " + transform.parent.name);
	                return null;
	            }
			}catch{
				return null;
			}
        }
    }

    protected abstract bool DragSuccessful();

	/// <summary>
	/// Retorna el objetivo del ente.s
	/// </summary>
	/// <returns>The target.</returns>
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
			if ((h.transform.tag.Equals ("TopCriatura") || h.transform.tag.Equals ("TopMagica")) && (this.tag.Equals ("LowCriatura") || this.tag.Equals ("LowMagica")) ||
			    ((h.transform.tag.Equals ("LowCriatura") || h.transform.tag.Equals ("LowMagica")) && (this.tag.Equals ("TopCriatura") || this.tag.Equals ("TopMagica")))) {
				// hit a creature or magica, save parent transform
				if (h.transform.tag.Contains ("Criatura"))
					Target = h.transform.parent.gameObject;
				else
					Target = h.transform.parent.gameObject.transform.parent.gameObject;
			} else if((h.transform.tag.Equals("TopPlayer") && (this.tag.Equals("LowCriatura"))) || 
				(h.transform.tag.Equals("LowPlayer") && (this.tag.Equals("TopCriatura")))){
				Target = h.transform.gameObject;
			}

        }
        return Target;
    }
}
