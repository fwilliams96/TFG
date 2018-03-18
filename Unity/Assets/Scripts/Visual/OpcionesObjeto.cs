using UnityEngine;
using System.Collections;
using DG.Tweening;

public class OpcionesObjeto : MonoBehaviour
{
    #region Atributos
    // PUBLIC FIELDS
    public GameObject TurnThisOffWhenPreviewing;  // if this is null, will not turn off anything 
    public Vector3 TargetPosition;
    public float TargetScale;
    public GameObject objetoPrevisualizado;
    public bool ActivateInAwake = false;

    // PRIVATE FIELDS
    private static OpcionesObjeto previsualizacionActual = null;

    // PROPERTIES WITH UNDERLYING PRIVATE FIELDS
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

    // MONOBEHVIOUR METHODS
    void Awake()
    {
        PrevisualizacionActivada = ActivateInAwake;
    }

    public virtual void MostrarOpciones()
    {
        MostrarPrevisualizacion();
        MostrarAccion();
    }

    public virtual void MostrarAccion()
    {
        //Se ha de mirar si es magica o criatura. En caso de criatura se debe mirar si esta en ataque o defensa
        Controlador.Instance.MostrarAccion(GetComponentInParent<IDHolder>().UniqueID);
    }

    protected void MostrarPrevisualizacion()
    {
        if(PrevisualizandoAlgunaCarta())
            PararTodasPrevisualizaciones();
        OverCollider = true;
        if (PrevisualizacionesPermitidas && PrevisualizacionActivada && Controlador.Instance.CartaOCriaturaDelJugador(gameObject.tag))
            PrevisualizarObjeto();
    }

    void QuitarPrevisualizacion()
    {
        OverCollider = false;

        if (!PrevisualizandoAlgunaCarta())
            PararTodasPrevisualizaciones();
    }

    // OTHER METHODS
    void PrevisualizarObjeto()
    {
        // 1) clone this card 
        // first disable the previous preview if there is one already
        //PararTodasPrevisualizaciones();
        // 2) save this HoverPreview as curent
        previsualizacionActual = this;
        // 3) enable Preview game object
        objetoPrevisualizado.SetActive(true);
        // 4) disable if we have what to disable
        if (TurnThisOffWhenPreviewing != null)
            TurnThisOffWhenPreviewing.SetActive(false);
        // 5) tween to target position
        objetoPrevisualizado.transform.localPosition = Vector3.zero;
        objetoPrevisualizado.transform.localScale = Vector3.one;

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
            previsualizacionActual.OverCollider = false;
            //TODO quitar visibilidad del menu si es una criatura o magica
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
            if (hb.OverCollider && hb.PrevisualizacionActivada)
                return true;
        }

        return false;
    }


}
