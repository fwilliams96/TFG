using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// holds the refs to all the Text, Images on the card
public class OneCardManager : MonoBehaviour {

    public CardAsset cardAsset;
    public OneCardManager PreviewManager;
    [Header("Text Component References")]
    public Text NameText;
    public Text ManaCostText;
    public Text EvolutionText;
    public Text DescriptionText;
    public Text TypeText;
    public Text AttackText;
    public Text DefenseText;
    [Header("Image References")]
    public Image CardTopRibbonImage;
    public Image CardLowRibbonImage;
    public Image CardGraphicImage;
    public Image CardBodyImage;
    public Image CardFaceFrameImage;
    public Image CardFaceGlowImage;
    public Image CardBackGlowImage;

    void Awake()
    {
        if (cardAsset != null)
            ReadCardFromAsset();
    }

    private bool canBePlayedNow = false;
    public bool CanBePlayedNow
    {
        get
        {
            return canBePlayedNow;
        }

        set
        {
            canBePlayedNow = value;

            CardFaceGlowImage.enabled = value;
        }
    }

    public void ReadCardFromAsset()
    {
        // universal actions for any Card
        // 1) apply tint
        if (cardAsset.AssetPersonaje != null)
        {
            CardBodyImage.color = cardAsset.AssetPersonaje.ClassCardTint;
            CardFaceFrameImage.color = cardAsset.AssetPersonaje.ClassCardTint;
            CardTopRibbonImage.color = cardAsset.AssetPersonaje.ClassRibbonsTint;
            CardLowRibbonImage.color = cardAsset.AssetPersonaje.ClassRibbonsTint;
        }
        else
        {
            //CardBodyImage.color = GlobalSettings.Instance.CardBodyStandardColor;
            CardFaceFrameImage.color = Color.white;
            //CardTopRibbonImage.color = GlobalSettings.Instance.CardRibbonsStandardColor;
            //CardLowRibbonImage.color = GlobalSettings.Instance.CardRibbonsStandardColor;
        }
        // 2) add card name
        NameText.text = cardAsset.name;
        // 3) add mana cost
        ManaCostText.text = cardAsset.CosteMana.ToString();
        // 4) add description
        DescriptionText.text = cardAsset.Descripcion;
        // 5) add type
        TypeText.text = cardAsset.TipoDeCarta.ToString();        
        // 6) Change the card graphic sprite
        CardGraphicImage.sprite = cardAsset.ImagenCarta;

        if (cardAsset.TipoDeCarta != TipoCarta.Magica)
        {
            // this is a creature
            AttackText.text = cardAsset.Ataque.ToString();
            DefenseText.text = cardAsset.Defensa.ToString();
            //EvolutionText.text = cardAsset.Evolucion;
        }

        if (PreviewManager != null)
        {
            // this is a card and not a preview
            // Preview GameObject will have OneCardManager as well, but PreviewManager should be null there

            PreviewManager.cardAsset = cardAsset;
            PreviewManager.ReadCardFromAsset();
        }
    }
}
