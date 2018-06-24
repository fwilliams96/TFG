using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Ente : ICharacter
{
    #region Atributos
    // PUBLIC FIELDS
    private CartaBase assetCarta;
	private AreaPosition area;
    private int idCriatura;
	private int defensa;
	private int ataque;
	private int attacksForOneTurn = 1;
    #endregion
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

	public AreaPosition Area
	{
		get
		{
			return area;
		}
	}

    #endregion

    // CONSTRUCTOR
	public Ente(AreaPosition area,CartaBase ca)
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
    }

	/// <summary>
	/// Restablece los ataques o veces de uso disponibles
	/// </summary>
    public virtual void OnTurnStart()
    {
        AtaquesRestantesEnTurno = attacksForOneTurn;
    }

}
