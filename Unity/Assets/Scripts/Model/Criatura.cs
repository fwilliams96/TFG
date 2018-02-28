using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Criatura : Ente, ICharacter
{
    #region Atributos
    private PosicionCriatura posicionCriatura;
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
    #endregion

    // CONSTRUCTOR
    public Criatura(CardAsset ca, PosicionCriatura posicionCriatura) : base(ca)
    {
        this.posicionCriatura = posicionCriatura;
    }

}
