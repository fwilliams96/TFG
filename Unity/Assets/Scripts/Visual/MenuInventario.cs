using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInventario : MonoBehaviour {

	public GameObject Elementos;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void MostrarSeccionCartas(){
		Elementos.GetComponent<Elementos> ().cartas = true;
		Elementos.GetComponent<Elementos> ().MostrarElementos ();
	}

	public void MostrarSeccionItems(){
		Elementos.GetComponent<Elementos> ().cartas = false;
		Elementos.GetComponent<Elementos> ().MostrarElementos ();
	}
}
