using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pocion : Item
{
	public Pocion() : base() {}

	public Pocion(string rutaImagen, int cantidad) : base(rutaImagen,cantidad) {}


	public override Dictionary<string, System.Object> ToDictionary()
	{
		Dictionary<string, System.Object> result = new Dictionary<string, System.Object>();
		result["tipoItem"] = 0;
		result["rutaImagen"] = rutaImagen;
		result["cantidad"] = cantidad;
		return result;
	}
}

