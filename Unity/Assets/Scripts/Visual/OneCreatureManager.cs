using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OneCreatureManager : MonoBehaviour 
{
    public CardAsset cardAsset;
    public OneCardManager PreviewManager;
    [Header("Text Component References")]
    public Text HealthText;
    public Text AttackText;
    [Header("Image References")]
    public Image CreatureGraphicImage;
    public Image CreatureGlowImage;

    void Awake()
    {
        if (cardAsset != null)
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
        CreatureGraphicImage.sprite = cardAsset.ImagenCarta;

        AttackText.text = cardAsset.Ataque.ToString();
        HealthText.text = cardAsset.Defensa.ToString();

        if (PreviewManager != null)
        {
            PreviewManager.cardAsset = cardAsset;
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
