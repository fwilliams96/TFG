using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OneCreatureManager : MonoBehaviour 
{
    public CartaAsset CartaAsset;
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

    public void LeerDatosAsset()
    {
        // Change the card graphic sprite
		CreatureGraphicImage.sprite = Resources.Load<Sprite>(CartaAsset.RutaImagenCarta);

        AttackText.text = CartaAsset.Ataque.ToString();
        HealthText.text = CartaAsset.Defensa.ToString();

        if (PreviewManager != null)
        {
            PreviewManager.CartaAsset = CartaAsset;
            PreviewManager.LeerDatosAsset();
        }
    }	

    public void HacerDaño(int daño, int healthAfter)
    {
        if (daño > 0)
        {
            DamageEffect.CreateDamageEffect(transform.position, daño);
            HealthText.text = healthAfter.ToString();
        }
    }
}
