using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Carta : IIdentifiable
{
    #region Atributos
    // an ID of this card
    private int idCarta;
    // a reference to the card asset that stores all the info about this card
	private string idAsset;
	private CartaBase assetCarta;
    private Progreso progreso;
    #endregion

    public Carta(string idAsset,CartaBase ca)
    {
        // set the CardAsset reference
        this.idAsset = idAsset;
        this.assetCarta = ca;
        // get unique int ID
        idCarta = IDFactory.GetUniqueID();
        progreso = new Progreso();
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

	public void AñadirPiedra(int cantidad){
		progreso.Piedra += cantidad;
	}
	public void AñadirPocion(int cantidad){
		progreso.Pocion += cantidad;
	}

    #endregion

}
