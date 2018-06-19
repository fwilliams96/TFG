using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsBatalla : MonoBehaviour {

	public Toggle toggleEntero;
	public Toggle togglePorcentaje;

	// Use this for initialization
	void Start () {
		switch (ConfiguracionUsuario.Instance.ConfiguracionBatalla) {
			case ConfiguracionUsuario.TIPO_CONFIGURACION.ENTERO:
				toggleEntero.isOn = true;
				togglePorcentaje.isOn = false;
				break;
			case ConfiguracionUsuario.TIPO_CONFIGURACION.PORCENTAJE:
				toggleEntero.isOn = false;
				togglePorcentaje.isOn = true;
				break;
		}

	}

	public void CambiarBatalla(int valor){
		ConfiguracionUsuario settings = ConfiguracionUsuario.Instance;
		if (valor == 0)
			settings.ConfiguracionBatalla = ConfiguracionUsuario.TIPO_CONFIGURACION.ENTERO;
		else if(valor == 1)
			settings.ConfiguracionBatalla = ConfiguracionUsuario.TIPO_CONFIGURACION.PORCENTAJE;
		else 
			settings.ConfiguracionBatalla = ConfiguracionUsuario.TIPO_CONFIGURACION.FRACCION;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
