using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsItems : MonoBehaviour {

	public Toggle toggleEntero;
	public Toggle toggleFracciones;
	public Toggle togglePorcentaje;

	// Use this for initialization
	void Start () {
		switch (Settings.Instance.Items) {
			case Settings.TIPO_NUMERO.ENTERO:
				toggleEntero.isOn = true;
				toggleFracciones.isOn = false;
				togglePorcentaje.isOn = false;
			break;
		
		case Settings.TIPO_NUMERO.FRACCION:
				toggleEntero.isOn = false;
				toggleFracciones.isOn = true;
				togglePorcentaje.isOn = false;
			break;
		case Settings.TIPO_NUMERO.PORCENTAJE:
				toggleEntero.isOn = false;
				toggleFracciones.isOn = false;
				togglePorcentaje.isOn = true;
			break;
		}

	}

	public void CambiarItems(int valor){
		Settings settings = Settings.Instance;
		if (valor == 0)
			settings.Items = Settings.TIPO_NUMERO.ENTERO;
		else if(valor == 1)
			settings.Items = Settings.TIPO_NUMERO.PORCENTAJE;
		else
			settings.Items = Settings.TIPO_NUMERO.FRACCION;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
