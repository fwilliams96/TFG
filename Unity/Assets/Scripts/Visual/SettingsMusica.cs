using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMusica : MonoBehaviour {

	public Toggle toggleOn;
	public Toggle toggleOff;

	// Use this for initialization
	void Start () {
		if (ConfiguracionUsuario.Instance.Musica) {
			toggleOn.isOn = true;
			toggleOff.isOn = false;
		} else {
			toggleOn.isOn = false;
			toggleOff.isOn = true;
		}
	}

	/// <summary>
	/// Actualiza la musica en settings.
	/// </summary>
	/// <param name="valor">Valor.</param>
	public void CambiarMusica(int valor){
		ConfiguracionUsuario settings = ConfiguracionUsuario.Instance;
		if (valor == 0) {
			settings.Musica = true;
		} else {
			settings.Musica = false;
		}
		ControladorMenu.Instance.ActualizarMusica (settings.Musica);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
