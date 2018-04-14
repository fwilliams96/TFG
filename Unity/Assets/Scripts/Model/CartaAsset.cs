using UnityEngine;
using System.Collections;

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

public class CartaAsset
{
    public string Nombre;
    public string Descripcion;
    public Familia Familia;
    public string RutaImagenCarta;
    public int CosteMana;
    public Sprite Fondo;
    //Excepto si es ancestral
    public int Evolucion;

    public int Defensa;
    public int Ataque;

    public CartaAsset()
    {
        //TipoDeCarta = TipoCarta.Ancestral;
        //Defensa = 2;
    }
}

