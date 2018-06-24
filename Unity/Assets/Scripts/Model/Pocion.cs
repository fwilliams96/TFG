using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pocion : Item
{
	public Pocion() : base() {}

	public Pocion(string rutaImagen, int cantidad) : base(rutaImagen,cantidad) {}

	/// <summary>
	/// Determina los elementos necesarios que se guardan para el item poción en base de datos.
	/// </summary>
	/// <returns>The dictionary.</returns>
	public override Dictionary<string, System.Object> ToDictionary()
	{
		Dictionary<string, System.Object> result = new Dictionary<string, System.Object>();
		result["tipoItem"] = 0;
		result["rutaImagen"] = rutaImagen;
		result["cantidad"] = cantidad;
		return result;
	}
}

