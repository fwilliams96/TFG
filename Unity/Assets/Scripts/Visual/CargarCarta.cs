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

	void OnEnable(){
		Debug.Log ("On enable");
		elemento = Instantiate (TouchManager2.Instance.ObjetoActual, transform);
		elemento.transform.parent = gridLayoutGroup.gameObject.transform;
		elemento.tag = "CartaPrevisualizada";
		Destroy (elemento.GetComponent<IDHolder> ());
		elemento.GetComponent<BoxCollider2D> ().enabled = false; 
		if (posicion.Equals (Posicion.DELANTE)) {
			elemento.GetComponent<PosicionVisual> ().MostrarCara ();
		} else {
			elemento.GetComponent<PosicionVisual> ().MostrarAtras ();
		}
			
	}

	void OnDisable(){
		Debug.Log ("On disable");
		Destroy (elemento);
	}

	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
