using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acciones : MonoBehaviour {

	public GameObject opciones;
	public GameObject visualizar;
	public GameObject añadirItem;
	public GameObject evolucionar;
	public static Acciones Instance;
	void Awake(){
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
		opciones.SetActive (false);
		evolucionar.SetActive (true);
	}

	public void CerrarMenu(){
		opciones.SetActive (true);
		visualizar.SetActive (false);
		añadirItem.SetActive (false);
		this.gameObject.SetActive (false);
	}



}
