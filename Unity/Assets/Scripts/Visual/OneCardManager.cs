﻿using UnityEngine;
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
            LeerDatosAsset();
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

            PreviewManager.cardAsset = cardAsset;
            PreviewManager.LeerDatosAsset();
        }
    }

    private void LeerDatosCarta()
    {

        if (cardAsset.TipoDeCarta != TipoCarta.Magica && cardAsset.TipoDeCarta != TipoCarta.Spell)
        {
            // this is a creature
            AttackText.text = cardAsset.Ataque.ToString();
            DefenseText.text = cardAsset.Defensa.ToString();
            //EvolutionText.text = cardAsset.Evolucion;
        }
        // 2) add card name
        NameText.text = cardAsset.name;
        // 3) add mana cost
        ManaCostText.text = cardAsset.CosteMana.ToString();
        // 4) add description
        DescriptionText.text = cardAsset.Descripcion;
        // 5) add type
        TypeText.text = cardAsset.TipoDeCarta.ToString();
    }

    private void AplicarColor()
    {
        CardFaceFrameImage.color = Color.white;
    }

    private void LeerSpritesItem()
    {
        // 6) Change the card graphic sprite
        CardGraphicImage.sprite = cardAsset.ImagenCarta;
        //TODO cuando use CartaAsset en vez de CardAsset
        //if(cardAsset.Fondo != null)
            //CardBodyImage.sprite = cardAsset.Fondo;
    }
}
