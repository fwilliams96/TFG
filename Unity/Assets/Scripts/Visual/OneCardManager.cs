using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// holds the refs to all the Text, Images on the card
public class OneCardManager : MonoBehaviour {

    public CartaAsset CartaAsset;
    public float PorcentajeProgresoTrebol = 0;
    public float PorcentajeProgresoPocion = 0;
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
    [Header("Progress Card References")]
    public Slider ProgresoTrebol;
    public Slider ProgresoPocion;

    void Awake()
    {
        if (CartaAsset != null)
            LeerDatos();  
    }

    private bool puedeSerJugada = false;
    public bool PuedeSerJugada
    {
        get
        {
            return puedeSerJugada;
        }

        set
        {
            puedeSerJugada = value;

            CardFaceGlowImage.enabled = value;
        }
    }

    public void LeerDatos()
    {
        LeerDatosAsset();
        LeerProgreso();
    }

    public void LeerDatosAsset()
    {
        // universal actions for any Card
        LeerDatosCarta();
        AplicarColor();
        LeerSpritesItem();

       
        if (PreviewManager != null)
        {
            // this is a card and not a preview
            // Preview GameObject will have OneCardManager as well, but PreviewManager should be null there

            PreviewManager.CartaAsset = CartaAsset;
            PreviewManager.LeerDatosAsset();
        }
    }

    public void LeerProgreso()
    {
        ProgresoTrebol.value = PorcentajeProgresoTrebol;
        ProgresoPocion.value = PorcentajeProgresoPocion;
    }

    private void LeerDatosCarta()
    {

        if (CartaAsset.Familia != Familia.Magica)
        {
            // this is a creature
            AttackText.text = CartaAsset.Ataque.ToString();
            DefenseText.text = CartaAsset.Defensa.ToString();
            //EvolutionText.text = cardAsset.Evolucion;
        }
        // 2) add card name
        NameText.text = CartaAsset.Nombre;
        // 3) add mana cost
        ManaCostText.text = CartaAsset.CosteMana.ToString();
        // 4) add description
        DescriptionText.text = CartaAsset.Descripcion;
        // 5) add type
        TypeText.text = CartaAsset.Familia.ToString();
    }

    private void AplicarColor()
    {
        CardFaceFrameImage.color = Color.white;
    }

    private void LeerSpritesItem()
    {
        // 6) Change the card graphic sprite
        CardGraphicImage.sprite = CartaAsset.ImagenCarta;
        //TODO cuando use CartaAsset en vez de CardAsset
        //if(cardAsset.Fondo != null)
            //CardBodyImage.sprite = cardAsset.Fondo;
    }
}
