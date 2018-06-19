using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OneEnteManager : MonoBehaviour 
{
    public CartaBase CartaAsset;
    public OneCardManager PreviewManager;
    [Header("Text Component References")]
    public Text HealthText;
    public Text AttackText;
    [Header("Image References")]
    public Image CreatureGraphicImage;
    public Image CreatureGlowImage;

    void Awake()
    {
        if (CartaAsset != null)
            LeerDatosAsset();
    }

    private bool puedeAtacar = false;
    public bool PuedeAtacar
    {
        get
        {
            return puedeAtacar;
        }

        set
        {
            puedeAtacar = value;

            CreatureGlowImage.enabled = value;
        }
    }

	public virtual void LeerDatosAsset() {}

}
