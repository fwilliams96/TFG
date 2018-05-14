using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInventario : MonoBehaviour {

	public GameObject elementos;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void MostrarSeccionCartas(){
		elementos.GetComponent<Elementos> ().tipoElementos = Elementos.TIPO_ELEMENTOS.CARTAS;
		elementos.GetComponent<Elementos> ().MostrarElementos ();
	}

	public void MostrarSeccionItems(){
		elementos.GetComponent<Elementos> ().tipoElementos = Elementos.TIPO_ELEMENTOS.ITEMS;
		elementos.GetComponent<Elementos> ().MostrarElementos ();
	}
}
