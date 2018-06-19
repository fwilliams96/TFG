using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosicionVisual : MonoBehaviour {

	public GameObject Face;
	public GameObject Back;

	public void MostrarCara(){
		Face.SetActive (true);
		Back.SetActive (false);
	}

	public void MostrarAtras(){
		Back.SetActive (true);
		Face.SetActive (false);
	}
}
