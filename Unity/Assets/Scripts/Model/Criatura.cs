using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    public Criatura(CartaAsset ca, PosicionCriatura posicionCriatura) : base(ca)
    {
        this.posicionCriatura = posicionCriatura;
		this.haAtacado = false;
    }

}
