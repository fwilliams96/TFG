using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotonSalir : MonoBehaviour {

	public void Salir(){
		this.gameObject.GetComponent<Button> ().interactable = false;
		SceneReloader.Instance.ReloadScene ();
	}
}
