using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Magica: Ente,ICharacter 
{
	private bool efectoActivado;

	public bool EfectoActivado{
		get{
			return efectoActivado;
		}set{
			efectoActivado = value;
		}
	}

    // CONSTRUCTOR
	public Magica(string area,CartaAsset ca) : base(area,ca) { this.efectoActivado = false; }

	public override void OnTurnStart()
	{
		base.OnTurnStart ();
	}

}
