using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccionBaraja : MonoBehaviour {

	public GameObject BotonAccion;
	public Text textoBoton;
	public static AccionBaraja Instance;
	public delegate void RegistrarFuncion();
	private int idEnte;
	public event RegistrarFuncion metodo;
	private GameObject elementoActual;

	public GameObject ElementoActual {
		get {
			return elementoActual;
		}set {
			elementoActual = value;
		}
	}
	public enum TIPO_ACCION{
		QUITAR,
		USAR
	}

	void Awake(){
		Instance = this;
		BotonAccion.SetActive (false);
	}

	public void MostrarAccion(GameObject carta, TIPO_ACCION tipo, RegistrarFuncion funcion){
		this.elementoActual = carta;
		this.elementoActual.GetComponent<OneCardManager> ().PuedeSerJugada = true;
		BotonAccion.SetActive (true);
		metodo = funcion;
		switch (tipo) {
			case TIPO_ACCION.USAR:
				textoBoton.text = "Usar";
				break;
			case TIPO_ACCION.QUITAR:
				textoBoton.text = "Quitar";
				break;
		}
	}

	public void AccionClic(){
		metodo.Invoke();
		CerrarAccion ();
	}

	public void CerrarAccion(){
		if (BotonAccion.activeSelf) {
			this.elementoActual.GetComponent<OneCardManager> ().PuedeSerJugada = false;
			this.elementoActual = null;
			BotonAccion.SetActive (false);
		}
	}
}
