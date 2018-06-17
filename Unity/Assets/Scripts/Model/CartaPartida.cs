﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CartaPartida : IIdentifiable
{
    #region Atributos
    // an ID of this card
    private int idCarta;
    // a reference to the card asset that stores all the info about this card
	private CartaAsset assetCarta;
    #endregion

	public CartaPartida(CartaAsset ca)
    {
        this.assetCarta = ca;
        // get unique int ID
		ResetCosteMana();
        idCarta = IDFactory.GetUniqueID();
		Recursos.CartasCreadasEnElJuego.Add(idCarta, this);
    }

    // method to set or reset mana cost
    public void ResetCosteMana()
    {
        CosteManaActual = assetCarta.CosteMana;
    }

    #region Getters/Setters

	public CartaAsset AssetCarta{
		get{
			return assetCarta;
		}
		set{
			assetCarta = value;
		}
	}
    public int ID
    {
        get { return idCarta; }
    }

    public int CosteManaActual { get; set; }

    #endregion

}
