using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TipoItem
{
	Pocion,
	Piedra
}

public class Item
{
    public string RutaImagen;
	public int Cantidad;
	public TipoItem Tipo;
	private int idItem;

	public Item()
    {
		idItem = IDFactory.GetUniqueID();
		Cantidad = 0;
		RutaImagen = "";
    }

	public Item(TipoItem tipo, string rutaImagen, int cantidad)
	{
		idItem = IDFactory.GetUniqueID();
		Tipo = tipo;
		RutaImagen = rutaImagen;
		Cantidad = cantidad;
	}


	public Dictionary<string, System.Object> ToDictionary()
	{
		Dictionary<string, System.Object> result = new Dictionary<string, System.Object>();
		result["tipoItem"] = (int)Tipo;
		result["rutaImagen"] = RutaImagen;
		result["cantidad"] = Cantidad;
		return result;
	}

	public int ID
	{
		get { return idItem; }
	}
}

