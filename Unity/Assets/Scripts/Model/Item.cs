using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Item : IIdentifiable
{
    protected string rutaImagen;
	protected int cantidad;
	protected int idItem;

	public Item(string rutaImagen, int cantidad)
	{
		idItem = IDFactory.GetMenuUniqueID();
		this.rutaImagen = rutaImagen;
		this.cantidad = cantidad;
	}

	public abstract Dictionary<string, System.Object> ToDictionary ();

	public int ID
	{
		get { return idItem; }
	}

	public string RutaImagen {
		get {
			return rutaImagen;
		}set { 
			rutaImagen = value;
		}
	}

	public int Cantidad{
		get{ 
			return cantidad;
		}set{ 
			cantidad = cantidad;
		}
	}
}

