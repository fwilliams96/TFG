using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class Carta: IIdentifiable
{
    // an ID of this card
    public int idCarta; 
    // a reference to the card asset that stores all the info about this card
    public CardAsset assetCarta;
    // a script of type spell effect that will be attached to this card when it`s created
    public SpellEffect efecto;

    // PROPERTIES
    public int ID
    {
        get{ return idCarta; }
    }

    public int CosteManaActual{ get; set; }

    // CONSTRUCTOR
    public Carta(CardAsset ca)
    {
        // set the CardAsset reference
        this.assetCarta = ca;
        // get unique int ID
        idCarta = IDFactory.GetUniqueID();
        //UniqueCardID = IDFactory.GetUniqueID();
        ResetCosteMana();
        // create an instance of SpellEffect with a name from our CardAsset
        // and attach it to 
        if (ca.SpellScriptName!= null && ca.SpellScriptName!= "")
        {
            efecto = System.Activator.CreateInstance(System.Type.GetType(ca.SpellScriptName)) as SpellEffect;
        }
        // add this card to a dictionary with its ID as a key
        Recursos.CartasCreadasEnElJuego.Add(idCarta, this);
    }

    // method to set or reset mana cost
    public void ResetCosteMana()
    {
        CosteManaActual = assetCarta.CosteMana;
    }

}
