using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsBatalla : MonoBehaviour {

	public Toggle toggleEntero;
	public Toggle togglePorcentaje;

	// Use this for initialization
	void Start () {
		switch (ConfiguracionUsuario.Instance.Batalla) {
			case ConfiguracionUsuario.TIPO_NUMERO.ENTERO:
				toggleEntero.isOn = true;
				togglePorcentaje.isOn = false;
				break;
			case ConfiguracionUsuario.TIPO_NUMERO.PORCENTAJE:
				toggleEntero.isOn = false;
				togglePorcentaje.isOn = true;
				break;
		}

	}

	public void CambiarBatalla(int valor){
		ConfiguracionUsuario settings = ConfiguracionUsuario.Instance;
		if (valor == 0)
			settings.Batalla = ConfiguracionUsuario.TIPO_NUMERO.ENTERO;
		else if(valor == 1)
			settings.Batalla = ConfiguracionUsuario.TIPO_NUMERO.PORCENTAJE;
		else 
			settings.Batalla = ConfiguracionUsuario.TIPO_NUMERO.FRACCION;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
