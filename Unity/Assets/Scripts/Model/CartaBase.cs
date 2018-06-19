using UnityEngine;
using System.Collections;

public enum Familia
{
    Fuego,
    Tierra,
	Aire,
    Agua,
    Magica,
    Ancestral
}

public enum Efecto
{
	Ninguno,
	Destructor,
	Espejo,
	Mana,
	Vida
}

public class CartaBase
{
    public string Nombre;
    public string Descripcion;
	public string InfoCarta;
    public Familia Familia;
    public string RutaImagenCarta;
    public int CosteMana;
    public int Evolucion;
	public int IDEvolucion;
	public Efecto Efecto;

    public int Defensa;
    public int Ataque;

    public CartaBase()
    {
    }
}

