using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInventario : MonoBehaviour {

	public GameObject SeccionCartas;
	public GameObject SeccionItems;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void MostrarSeccionCartas(){
		SeccionCartas.SetActive (true);
		SeccionItems.SetActive (false);
	}

	public void MostrarSeccionItems(){
		SeccionCartas.SetActive (false);
		SeccionItems.SetActive (true);
	}
}
