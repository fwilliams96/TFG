using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPerfil : MonoBehaviour {

	public Text nivel;
	public Slider experiencia;

	void Awake(){
		nivel.text = ControladorMenu.Instance.ObtenerNivelJugador ();
		experiencia.value = ControladorMenu.Instance.ObtenerExperienciaJugador ();
	}

	public void CambiarBatalla(int valor){
		Settings settings = Settings.Instance;
		if (valor == 0)
			settings.Batalla = Settings.TIPO_NUMERO.ENTERO;
		else if(valor == 1)
			settings.Batalla = Settings.TIPO_NUMERO.PORCENTAJE;
		else 
			settings.Batalla = Settings.TIPO_NUMERO.FRACCION;
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

	public void CambiarMusica(int valor){
		Settings settings = Settings.Instance;
		if (valor == 0) {
			settings.Musica = true;
		} else {
			settings.Musica = false;
		}
		ControladorMenu.Instance.ActualizarMusica (settings.Musica);
	}
}
