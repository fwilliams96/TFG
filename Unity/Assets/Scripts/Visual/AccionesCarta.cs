using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccionesCarta : MonoBehaviour {

	public GameObject opciones;
	public GameObject visualizar;
	public GameObject añadirItem;
	public GameObject evolucionar;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void MostrarAccionesCarta(){
		opciones.SetActive (true);
	}

	public void OcultarAccionesCarta(){
		if (visualizar.activeSelf) {
			visualizar.SetActive (false);
		}

		if (añadirItem.activeSelf) {
			añadirItem.SetActive (false);
		}
			
		if (evolucionar.activeSelf) {
			evolucionar.SetActive (false);
		}
		if(opciones.activeSelf)
			opciones.SetActive (false);
	}

	public void Opciones(int indiceOpcion){
		switch (indiceOpcion) {
		case 1:
			visualizar.SetActive (false);
			break;
		case 2:
			añadirItem.SetActive (false);
			break;
		case 3:
			evolucionar.SetActive (false);
			break;
		}
		opciones.SetActive (true);
	}

	public void Visualizar(){
		opciones.SetActive (false);
		visualizar.SetActive (true);
	}

	public void AñadirItem(){
		opciones.SetActive (false);
		añadirItem.SetActive (true);
	}

	public void Evolucionar(){
		ControladorMenu.Instance.EvolucionarCarta (Acciones.Instance.ElementoActual);
		opciones.SetActive (false);
		evolucionar.SetActive (true);
	}
}
