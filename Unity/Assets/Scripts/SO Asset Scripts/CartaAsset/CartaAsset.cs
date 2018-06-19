using UnityEngine;

public class CartaAsset : ScriptableObject 
{
    // this object will hold the info about the most general card
    [Header("General info")]
	public Familia Tipo;
	public string Título;
	public Efecto Efecto;
	public string NombreImagen;
	[TextArea(2,3)]
	public string Descripcion;
	public string InfoCarta;
	public int Evolucion;
	public int IDEvolucion;
	public int Mana;
	public int Ataque;
	public int Defensa;

    public CartaAsset()
    {
    }
}
