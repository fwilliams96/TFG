using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalirPartida : MonoBehaviour {

	public static SalirPartida Instance;
	public GameObject SalirPanel;

	void Awake()
	{
		Instance = this;
		SalirPanel.SetActive(false);
	}

	public void MostrarMensajeSalirPartida(){
		SalirPanel.SetActive(true);
	}

	public void Cancelar(){
		SalirPanel.SetActive (false);
	}
}
