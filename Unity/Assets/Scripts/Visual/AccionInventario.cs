using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccionInventario : MonoBehaviour {

	public GameObject panelCarta;

	void OnEnable(){
		panelCarta.GetComponent<CargarCarta> ().MostrarCarta ();
	}
	void OnDisable(){
		panelCarta.GetComponent<CargarCarta> ().EliminarCarta ();	
	}
}
