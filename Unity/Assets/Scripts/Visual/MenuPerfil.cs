using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPerfil : MonoBehaviour {

	public void CambiarBatalla(int valor){
		Settings settings = Settings.Instance;
		if (valor == 0)
			settings.Batalla = Settings.TIPO_NUMERO.ENTERO;
		else
			settings.Batalla = Settings.TIPO_NUMERO.RACIONAL;
	}

	public void CambiarItems(int valor){
		Settings settings = Settings.Instance;
		if (valor == 0)
			settings.Items = Settings.TIPO_NUMERO.ENTERO;
		else
			settings.Items = Settings.TIPO_NUMERO.RACIONAL;
	}
}
