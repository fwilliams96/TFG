using UnityEngine;
using System.Collections;
using DG.Tweening;

public class OpcionesObjeto : MonoBehaviour
{
    #region Atributos
    // PUBLIC FIELDS
    public GameObject TurnThisOffWhenPreviewing;  
    public Vector3 TargetPosition;
    public float TargetScale;
    public GameObject objetoPrevisualizado;
    public bool ActivateInAwake = false;

    // PRIVATE FIELDS
    private static OpcionesObjeto previsualizacionActual = null;

    private static bool _PrevisualizacionesPermitidas = true;
    #endregion
    #region Getters/Setters
    public static bool PrevisualizacionesPermitidas
    {
        get { return _PrevisualizacionesPermitidas; }

        set
        {
            //Debug.Log("Hover Previews Allowed is now: " + value);
            _PrevisualizacionesPermitidas = value;
            if (!_PrevisualizacionesPermitidas)
                PararTodasPrevisualizaciones();
        }
    }

    private bool _previsualizacionActivada = false;
    public bool PrevisualizacionActivada
    {
        get { return _previsualizacionActivada; }

        set
        {
            _previsualizacionActivada = value;
            if (!_previsualizacionActivada)
                StopThisPreview();
        }
    }

    public bool OverCollider { get; set; }
    #endregion

    void Awake()
    {
        PrevisualizacionActivada = ActivateInAwake;
    }

    public virtual void MostrarOpciones()
    {
        MostrarPrevisualizacion();
        MostrarAccion();
    }

	/// <summary>
	/// Muestra la accion del ente.
	/// </summary>
    public virtual void MostrarAccion()
    {
        //Se ha de mirar si es magica o criatura. En caso de criatura se debe mirar si esta en ataque o defensa
        if (PrevisualizacionesPermitidas && PrevisualizacionActivada && Controlador.Instance.CartaOCriaturaDelJugador(gameObject.tag))
            Controlador.Instance.MostrarAccion(GetComponentInParent<IDHolder>().UniqueID);
    }

	/// <summary>
	/// Muestra la previsualizacion de lacarta.
	/// </summary>
    protected void MostrarPrevisualizacion()
    {
        if(PrevisualizandoAlgunaCarta())
            PararTodasPrevisualizaciones();
        if (PrevisualizacionesPermitidas && PrevisualizacionActivada && Controlador.Instance.CartaOCriaturaDelJugador(gameObject.tag))
            PrevisualizarObjeto();
    }

    void QuitarPrevisualizacion()
    {
        if (!PrevisualizandoAlgunaCarta())
            PararTodasPrevisualizaciones();
    }

    void PrevisualizarObjeto()
    {
        previsualizacionActual = this;
        objetoPrevisualizado.SetActive(true);
        if (TurnThisOffWhenPreviewing != null)
            TurnThisOffWhenPreviewing.SetActive(false);
        objetoPrevisualizado.transform.localPosition = Vector3.zero;
        objetoPrevisualizado.transform.localScale = Vector3.one;
		if (gameObject.tag.Contains ("Card")) {
			if (gameObject.tag.Contains ("Low")) {
				TargetPosition.y = 2.0f;
			} else {
				TargetPosition.y = -2.0f;
			}
		}

        objetoPrevisualizado.transform.DOLocalMove(TargetPosition, 1f).SetEase(Ease.OutQuint);
        objetoPrevisualizado.transform.DOScale(TargetScale, 1f).SetEase(Ease.OutQuint);
    }

    void StopThisPreview()
    {
        objetoPrevisualizado.SetActive(false);
        objetoPrevisualizado.transform.localScale = Vector3.one;
        objetoPrevisualizado.transform.localPosition = Vector3.zero;
        if (TurnThisOffWhenPreviewing != null)
            TurnThisOffWhenPreviewing.SetActive(true);
    }

    // STATIC METHODS
    public static void PararTodasPrevisualizaciones()
    {
        if (previsualizacionActual != null)
        {
            AccionesPopUp.Instance.OcultarPopup();
            previsualizacionActual.objetoPrevisualizado.SetActive(false);
            previsualizacionActual.objetoPrevisualizado.transform.localScale = Vector3.one;
            previsualizacionActual.objetoPrevisualizado.transform.localPosition = Vector3.zero;
            if (previsualizacionActual.TurnThisOffWhenPreviewing != null)
                previsualizacionActual.TurnThisOffWhenPreviewing.SetActive(true);
        }

    }

    public static bool PrevisualizandoAlgunaCarta()
    {
        if (!PrevisualizacionesPermitidas)
            return false;

        OpcionesObjeto[] allHoverBlowups = GameObject.FindObjectsOfType<OpcionesObjeto>();

        foreach (OpcionesObjeto hb in allHoverBlowups)
        {
            if (hb.PrevisualizacionActivada)
                return true;
        }

        return false;
    }


}
