using UnityEngine;
using System.Collections;

public enum Familia
{
    Fuego,
    Tierra,
    Electrica,
	Aire,
    Agua,
    Magica,
    Fusion,
    Ancestral
}

public enum Efecto
{
	Destructor,
	Espejo,
	Mana,
	Vida
}

public class CartaAsset
{
    public string Nombre;
    public string Descripcion;
    public Familia Familia;
    public string RutaImagenCarta;
    public int CosteMana;
    //Excepto si es ancestral
    public int Evolucion;
	public Efecto Efecto;

    public int Defensa;
    public int Ataque;

    public CartaAsset()
    {
    }
}

