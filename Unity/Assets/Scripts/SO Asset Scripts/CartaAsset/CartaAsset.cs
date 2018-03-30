using System;
using UnityEngine;

public enum Familia
{
    Fuego,
    Tierra,
    Electrica,
    Agua,
    Magica,
    Fusion,
    Ancestral
}

[Serializable]
public class CartaAsset : ScriptableObject 
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
    public string NombreScriptEfecto;

    [Header("Carta no mágica")]
    public int Defensa;
    public int Ataque;
    public int AtaquesPorTurno = 1;
        
    public CartaAsset()
    {
        //TipoDeCarta = TipoCarta.Ancestral;
        //Defensa = 2;
    }
}
