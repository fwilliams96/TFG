using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PosicionCriatura { ATAQUE, DEFENSA };

[System.Serializable]
public class Criatura : Ente, ICharacter
{
    #region Atributos
    private PosicionCriatura posicionCriatura;
	private bool haAtacado;
    #endregion
    #region Getters/Setters
    public PosicionCriatura PosicionCriatura
    {
        get
        {
            return posicionCriatura;
        }
        set
        {
            posicionCriatura = value;
        }
    }
	public bool HaAtacado
	{
		get
		{
			return haAtacado;
		}
		set
		{
			haAtacado = value;
		}
	}
    #endregion

    // CONSTRUCTOR
	public Criatura(AreaPosition area,CartaBase ca, PosicionCriatura posicionCriatura) : base(area,ca)
    {
        this.posicionCriatura = posicionCriatura;
		this.haAtacado = false;
    }

	public override void OnTurnStart()
	{
		base.OnTurnStart ();
		haAtacado = false;
	}

}
