using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// holds the refs to all the Text, Images on the card
public class OneCardManager : MonoBehaviour {

    public CartaAsset CartaAsset;
    public OneCardManager PreviewManager;
    [Header("Text Component References")]
    public Text NameText;
    public Text ManaCostText;
    public Text DescriptionText;
    public Text TypeText;
    public Text AttackText;
    public Text DefenseText;
    [Header("Image References")]
	public Image EvolutionImage;
	public Image AncestralImage;
    public Image CardGraphicImage;
    public Image CardBodyImage;
    public Image CardFaceFrameImage;
    public Image CardFaceGlowImage;

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
    }

    public void LeerDatosAsset()
    {
        // universal actions for any Card
        LeerDatosCarta();
        AplicarColor();
		LeerSprites();

       
        if (PreviewManager != null)
        {
            // this is a card and not a preview
            // Preview GameObject will have OneCardManager as well, but PreviewManager should be null there

            PreviewManager.CartaAsset = CartaAsset;
            PreviewManager.LeerDatosAsset();
        }
    }

    private void LeerDatosCarta()
    {

		if (!CartaAsset.Familia.Equals (Familia.Magica)) {
			// this is a creature
			AttackText.text = CartaAsset.Ataque.ToString ();
			DefenseText.text = CartaAsset.Defensa.ToString ();
		} else {
			//Ponemos ataque y defensa oculto
			AttackText.transform.parent.gameObject.SetActive (false);
			DefenseText.transform.parent.gameObject.SetActive (false);
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

    private void LeerSprites()
    {
		CardGraphicImage.sprite = Resources.Load<Sprite>(CartaAsset.RutaImagenCarta);
		switch (CartaAsset.Evolucion) {
			case 1:
				EvolutionImage.sprite = Resources.Load<Sprite> ("Sprites/Recursos/Componentes/IconoEvolucion/evo1");
				break;
			case 2:
				EvolutionImage.sprite = Resources.Load<Sprite> ("Sprites/Recursos/Componentes/IconoEvolucion/evo2");
				break;
			default:
				break;
		}
		if (CartaAsset.Familia.Equals (Familia.Ancestral))
			AncestralImage.enabled = true;
    }
}
