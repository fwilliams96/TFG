using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {

	public GameObject PanelProgressBar;
	public GameObject LoadingBar;
	public static ProgressBar Instance;
	public int speed = 5;
	private float currentAmount;

	void Awake(){
		Instance = this;
		PanelProgressBar.SetActive (false);
	}

	public void OcultarBarraProgreso(){
		PanelProgressBar.SetActive (false);
	}

	public void MostrarBarraProgreso(){
		PanelProgressBar.SetActive (true);
	}

	// Use this for initialization
	void Start () {
		this.currentAmount = 0;
	}
	
	//Actualiza visualmente la barra de progreso, cuando llega a 100 vuelve a empezar la 'animacion'
	void Update () {
		if (currentAmount < 100f) {
			currentAmount += speed * Time.deltaTime;

		} else {
			currentAmount = 0;
		}
		LoadingBar.GetComponent<Image> ().fillAmount = currentAmount / 100f;
	}
}
