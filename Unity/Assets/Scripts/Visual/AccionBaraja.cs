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

	public enum TIPO_ACCION{
		QUITAR,
		USAR
	}

	void Awake(){
		Instance = this;
		BotonAccion.SetActive (false);
	}

	public void MostrarAccion(TIPO_ACCION tipo, RegistrarFuncion funcion){
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
		BotonAccion.SetActive (false);
	}
}
