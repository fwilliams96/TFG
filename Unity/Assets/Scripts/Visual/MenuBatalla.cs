using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBatalla : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// Carga la escena de batalla.
	/// </summary>
    public void Jugar()
    {
		if (ControladorMenu.Instance.JugadorPuedeJugarBatalla ()) {
			IDHolder.ClearIDHoldersList ();
			SceneManager.LoadScene ("Batalla");
		} else {
			MessageManager.Instance.ShowMessage ("Tu mazo de batalla no contiene 8 cartas", 1.5f);
		}
    }
}
