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
		switch (ConfiguracionUsuario.Instance.Items) {
			case ConfiguracionUsuario.TIPO_NUMERO.ENTERO:
				toggleEntero.isOn = true;
				toggleFracciones.isOn = false;
				togglePorcentaje.isOn = false;
			break;
		
		case ConfiguracionUsuario.TIPO_NUMERO.FRACCION:
				toggleEntero.isOn = false;
				toggleFracciones.isOn = true;
				togglePorcentaje.isOn = false;
			break;
		case ConfiguracionUsuario.TIPO_NUMERO.PORCENTAJE:
				toggleEntero.isOn = false;
				toggleFracciones.isOn = false;
				togglePorcentaje.isOn = true;
			break;
		}

	}

	public void CambiarItems(int valor){
		ConfiguracionUsuario settings = ConfiguracionUsuario.Instance;
		if (valor == 0)
			settings.Items = ConfiguracionUsuario.TIPO_NUMERO.ENTERO;
		else if(valor == 1)
			settings.Items = ConfiguracionUsuario.TIPO_NUMERO.PORCENTAJE;
		else
			settings.Items = ConfiguracionUsuario.TIPO_NUMERO.FRACCION;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
