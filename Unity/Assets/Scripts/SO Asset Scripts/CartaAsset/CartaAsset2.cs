using System;
using UnityEngine;

[Serializable]
public class CartaAsset2 : ScriptableObject 
{
    [Header("Información general")]
    [TextArea(2,3)]
    public string Descripcion; 
    public Familia Familia;
    public Sprite ImagenCarta;
    public int CosteMana;
    public Sprite Fondo;
    //Excepto si es ancestral
    public int Evolucion;

    [Header("Carta no mágica")]
    public int Defensa;
    public int Ataque;
        
    public CartaAsset2()
    {
        //TipoDeCarta = TipoCarta.Ancestral;
        //Defensa = 2;
    }
}
