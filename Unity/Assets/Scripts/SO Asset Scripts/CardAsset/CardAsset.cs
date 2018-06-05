using UnityEngine;

public class CardAsset : ScriptableObject 
{
    // this object will hold the info about the most general card
    [Header("General info")]
	public string Nombre;
	[TextArea(2,3)]
	public string Descripcion;
	public Familia Familia;
	public string RutaImagenCarta;
	public int CosteMana;
	//Excepto si es ancestral
	public int Evolucion;
	public int IDEvolucion;
	public Efecto Efecto;

	public int Defensa;
	public int Ataque;

    public CardAsset()
    {
    }
}
