using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Ente : ICharacter
{
    #region Atributos
    // PUBLIC FIELDS
    private CartaAsset assetCarta;
	private string area;
    public EfectoEnte efecto;
    private int idCriatura;
	private int defensa;
	private int ataque;
	private int attacksForOneTurn = 1;
    #endregion
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
        get { return idCriatura; }
    }
   
    public int Defensa
    {
        get { return defensa; }

        set
        {
            defensa = value;
        }
    }

    
    public int Ataque
    {
        get { 
			return ataque; 
		}set{
			ataque = value;
		}
    }

   
    public int AtaquesRestantesEnTurno
    {
        get;
        set;
    }

	public string Area
	{
		get
		{
			return area;
		}
	}

    #endregion

    // CONSTRUCTOR
    public Ente(string area,CartaAsset ca)
    {
		this.area = area;
        this.assetCarta = ca;
        defensa = ca.Defensa;
        ataque = ca.Ataque;
        idCriatura = IDFactory.GetUniqueID();
        Recursos.EntesCreadosEnElJuego.Add(idCriatura, this);
    }

    public virtual void Morir()
    {
        if (efecto != null)
            efecto.WhenACreatureDies();
    }

    public virtual void OnTurnStart()
    {
        AtaquesRestantesEnTurno = attacksForOneTurn;
    }

}
