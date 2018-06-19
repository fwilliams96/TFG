using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CargarCarta : MonoBehaviour {
	public UnityEngine.UI.GridLayoutGroup gridLayoutGroup;
	GameObject elemento;

	public enum Posicion {
		ATRAS,
		DELANTE
	}
	public Posicion posicion;

	// Use this for initialization

	void Awake(){
	}

	public void MostrarCarta(){
		Debug.Log ("On enable");
		elemento = Instantiate (Acciones.Instance.ElementoActual, transform);
		elemento.GetComponent<OneCardManager> ().PuedeSerJugada = false;
		elemento.transform.SetParent (gridLayoutGroup.gameObject.transform);
		elemento.tag = "CartaPrevisualizada";
		IDHolder.EliminarElemento (elemento.GetComponent<IDHolder>());
		Destroy (elemento.GetComponent<IDHolder> ());
		elemento.GetComponent<BoxCollider2D> ().enabled = false; 
		if (posicion.Equals (Posicion.DELANTE)) {
			elemento.GetComponent<PosicionVisual> ().MostrarCara ();
		} else {
			elemento.GetComponent<PosicionVisual> ().MostrarAtras ();
		}
			
	}

	public void EliminarCarta(){
		Debug.Log ("On disable");
		Destroy (elemento);
	}

	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
