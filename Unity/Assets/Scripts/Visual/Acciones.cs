using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Acciones : MonoBehaviour {

	public GameObject PanelAcciones;

	public GameObject opciones;
	public GameObject visualizar;
	public GameObject añadirItem;
	public GameObject evolucionar;

	public static Acciones Instance;
	public GameObject Elementos;
	private GameObject elementoActual;
	private Button botonEvolucionar;

	public GameObject ElementoActual {
		get {
			return elementoActual;
		}set {
			elementoActual = value;
		}
	}


	void Awake(){
		Instance = this;
		botonEvolucionar = GameObject.FindGameObjectWithTag ("BotonEvolucionar").GetComponent<Button> ();
		PanelAcciones.SetActive (false);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void MostrarAcciones(GameObject actual){
		this.elementoActual = actual;
		this.elementoActual.GetComponent<OneCardManager> ().PuedeSerJugada = true;
		PanelAcciones.SetActive (true);
		Elementos.GetComponent<Elementos>().DeshabilitarColliderElementos ();
		botonEvolucionar.interactable = ControladorMenu.Instance.SePuedeEvolucionar (ElementoActual.GetComponent<IDHolder>().UniqueID);
	}
		
	public void CerrarMenu(){
		Elementos.GetComponent<Elementos>().HabilitarColliderElementos ();
		if (elementoActual != null) {
			this.elementoActual.GetComponent<OneCardManager> ().PuedeSerJugada = false;
			elementoActual = null;
		}
		PanelAcciones.SetActive (false);
		OcultarAccionesCarta ();
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
		if(!opciones.activeSelf)
			opciones.SetActive (true);
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
		botonEvolucionar.interactable = ControladorMenu.Instance.SePuedeEvolucionar (ElementoActual.GetComponent<IDHolder>().UniqueID);
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

	public void BuscarInfoCarta(){
		if (!Familia.Ancestral.Equals (elementoActual.GetComponent<OneCardManager> ().CartaAsset.Familia) &&
		    !Familia.Magica.Equals (elementoActual.GetComponent<OneCardManager> ().CartaAsset.Familia)) {

			string urlInfoCarta = elementoActual.GetComponent<OneCardManager> ().CartaAsset.InfoCarta;
			if(null != urlInfoCarta && !"".Equals(urlInfoCarta))
				Application.OpenURL (urlInfoCarta);
			else
				MessageManager.Instance.ShowMessage ("Ha habido un error al buscar la información",1.5f);
		} else {
			MessageManager.Instance.ShowMessage ("No existe información al respecto",1.5f);
		}

	}

}
