using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CartaPartida : IIdentifiable
{
    #region Atributos
    // an ID of this card
    private int idCarta;
    // a reference to the card asset that stores all the info about this card
	private CartaBase assetCarta;
    #endregion

	public CartaPartida(CartaBase ca)
    {
        this.assetCarta = ca;
        // get unique int ID
		ResetCosteMana();
		idCarta = IDFactory.GetBattleUniqueID();
		Recursos.CartasCreadasEnElJuego.Add(idCarta, this);
    }

    /// <summary>
    /// Resetea el coste de mana de la carta.
    /// </summary>
    public void ResetCosteMana()
    {
        CosteManaActual = assetCarta.CosteMana;
    }

    #region Getters/Setters

	public CartaBase AssetCarta{
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
