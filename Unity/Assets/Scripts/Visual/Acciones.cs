using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acciones : MonoBehaviour {

	public GameObject PanelAcciones;
	public GameObject accionesCarta;
	public GameObject accionesItem;
	public static Acciones Instance;
	public GameObject Elementos;
	void Awake(){
		Instance = this;
		PanelAcciones.SetActive (false);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void MostrarAcciones(bool cartas){
		PanelAcciones.SetActive (true);
		Elementos.GetComponent<Elementos>().DeshabilitarColliderElementos ();
		if (cartas) {
			accionesCarta.GetComponent<AccionesCarta> ().MostrarAccionesCarta ();
		} else {
			//accionesItem.GetComponent<AccionesItem> ().MostrarAccionesItem ();
		}
	}


		
	public void CerrarMenu(){
		if (accionesCarta.activeSelf) {
			accionesCarta.GetComponent<AccionesCarta> ().OcultarAccionesCarta ();
		} else {
			//accionesItem.GetComponent<AccionesItem> ().OcultarAccionesItem();
		}
		Elementos.GetComponent<Elementos>().HabilitarColliderElementos ();
		PanelAcciones.SetActive (false);
	}



}
