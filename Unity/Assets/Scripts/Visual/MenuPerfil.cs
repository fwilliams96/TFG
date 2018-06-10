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

}
