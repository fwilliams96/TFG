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
		switch (ConfiguracionUsuario.Instance.ConfiguracionItems) {
			case ConfiguracionUsuario.TIPO_CONFIGURACION.ENTERO:
				toggleEntero.isOn = true;
				toggleFracciones.isOn = false;
				togglePorcentaje.isOn = false;
			break;
		
		case ConfiguracionUsuario.TIPO_CONFIGURACION.FRACCION:
				toggleEntero.isOn = false;
				toggleFracciones.isOn = true;
				togglePorcentaje.isOn = false;
			break;
		case ConfiguracionUsuario.TIPO_CONFIGURACION.PORCENTAJE:
				toggleEntero.isOn = false;
				toggleFracciones.isOn = false;
				togglePorcentaje.isOn = true;
			break;
		}

	}

	public void CambiarItems(int valor){
		ConfiguracionUsuario settings = ConfiguracionUsuario.Instance;
		if (valor == 0)
			settings.ConfiguracionItems = ConfiguracionUsuario.TIPO_CONFIGURACION.ENTERO;
		else if(valor == 1)
			settings.ConfiguracionItems = ConfiguracionUsuario.TIPO_CONFIGURACION.PORCENTAJE;
		else
			settings.ConfiguracionItems = ConfiguracionUsuario.TIPO_CONFIGURACION.FRACCION;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
