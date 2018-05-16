using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Opciones : MonoBehaviour {

	public Button BotonVisualizar;
	public Button BotonAñadir;
	public Button BotonEvolucionar;

	void OnEnable(){
		BotonEvolucionar.interactable = ControladorMenu.Instance.SePuedeEvolucionar (Acciones.Instance.ElementoActual.GetComponent<IDHolder>().UniqueID);
	}

	void OnDisable(){
	}
}
