using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Piedra : Item
{
	public Piedra() : base(){ }

	public Piedra (string rutaImagen, int cantidad) : base (rutaImagen, cantidad){}


	public override Dictionary<string, System.Object> ToDictionary()
	{
		Dictionary<string, System.Object> result = new Dictionary<string, System.Object>();
		result["tipoItem"] = 1;
		result["rutaImagen"] = rutaImagen;
		result["cantidad"] = cantidad;
		return result;
	}
}

