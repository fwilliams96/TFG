using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsBatalla : MonoBehaviour {

	public Toggle toggleEntero;
	public Toggle togglePorcentaje;

	// Use this for initialization
	void Start () {
		switch (Settings.Instance.Batalla) {
			case Settings.TIPO_NUMERO.ENTERO:
				toggleEntero.isOn = true;
				togglePorcentaje.isOn = false;
				break;
			case Settings.TIPO_NUMERO.PORCENTAJE:
				toggleEntero.isOn = false;
				togglePorcentaje.isOn = true;
				break;
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
