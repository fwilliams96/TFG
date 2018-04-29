using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Opciones : MonoBehaviour {

	public Button BotonVisualizar;
	public Button BotonAñadir;
	public Button BotonEvolucionar;

	void OnEnable(){
		BotonEvolucionar.interactable = ControladorMenu.Instance.SePuedeEvolucionar (TouchManager2.Instance.ObjetoActual.GetComponent<IDHolder>().UniqueID);
	}

	void OnDisable(){
	}
}
