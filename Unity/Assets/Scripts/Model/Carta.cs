using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class Carta : IIdentifiable
{
    #region Atributos
    // an ID of this card
    private int idCarta;
    // a reference to the card asset that stores all the info about this card
	private string idAsset;
	private CartaAsset assetCarta;
    private Progreso progreso;
    #endregion

    public Carta()
    {
        idCarta = IDFactory.GetUniqueID();
        progreso = new Progreso();
    }

    public Carta(CartaAsset ca)
    {
        // set the CardAsset reference
        this.assetCarta = ca;
        // get unique int ID
        idCarta = IDFactory.GetUniqueID();
        //UniqueCardID = IDFactory.GetUniqueID();
        ResetCosteMana();
        // add this card to a dictionary with its ID as a key
        //Recursos.CartasCreadasEnElJuego.Add(idCarta, this);
        progreso = new Progreso();
    }

    public Carta(string idAsset,CartaAsset ca)
    {
        // set the CardAsset reference
        this.idAsset = idAsset;
        this.assetCarta = ca;
        // get unique int ID
        idCarta = IDFactory.GetUniqueID();
        ResetCosteMana();
        progreso = new Progreso();
    }

    // method to set or reset mana cost
    public void ResetCosteMana()
    {
        CosteManaActual = assetCarta.CosteMana;
    }

    public Dictionary<string, System.Object> ToDictionary()
    {
        Dictionary<string, System.Object> result = new Dictionary<string, System.Object>();
        result["asset"] = idAsset;
        result["progreso"] = progreso.ToDictionary();
        return result;
    }

    #region Getters/Setters

    // PROPERTIES
	public string IdAsset{
		get{
			return idAsset;
		}
		set{
			idAsset = value;
		}
	}

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

    public Progreso Progreso
    {
        get
        {
            return progreso;
        }

        set
        {
            progreso = value;
        }
    }

	public void AñadirMaterial(int cantidad){
		progreso.Material += cantidad;
	}
	public void AñadirPocion(int cantidad){
		progreso.Pocion += cantidad;
	}

    #endregion

}
