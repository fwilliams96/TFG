using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMazo : MonoBehaviour {

	public GameObject menus;

	// Use this for initialization
	void Start () {
	}

	void OnDisable(){
		ControladorMenu.Instance.CerrarAccion ();
	}

	/// <summary>
	/// Guarda o no el nuevo mazo.
	/// </summary>
	public void Aceptar(){
		if (ControladorMenu.Instance.GuardarNuevoMazo () == 0) {
			menus.GetComponent<Menu> ().Batalla ();
		}
	}

}
