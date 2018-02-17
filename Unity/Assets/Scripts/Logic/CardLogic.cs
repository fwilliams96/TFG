using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class CardLogic: IIdentifiable
{
    // reference to a player who holds this card in his hand
    public Player jugador;
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

    public bool PuedeSerJugada
    {
        get
        {
            bool nuestroTurno = (ControladorTurno.Instance.jugadorActual == jugador);
            // for spells the amount of characters on the field does not matter
            bool fieldNotFull = true;
            // but if this is a creature, we have to check if there is room on board (table)
            //TODO Esto de momento sobrará porque todas las cartas ocuparan sitio en la mesa
            if (assetCarta.Defensa > 0)
                fieldNotFull = (jugador.NumCriaturasEnLaMesa() < DatosGenerales.Instance.NumMaximoCriaturasMesa);
            //Debug.Log("Card: " + ca.name + " has params: ownersTurn=" + ownersTurn + "fieldNotFull=" + fieldNotFull + " hasMana=" + (CurrentManaCost <= owner.ManaLeft));
            return nuestroTurno && fieldNotFull && (CosteManaActual <= jugador.ManaRestante);
        }
    }

    // CONSTRUCTOR
    public CardLogic(CardAsset ca)
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
