using UnityEngine;
using UnityEngine.UI;

public class BotonInicialDefecto : MonoBehaviour {

	public Button botonInicial;

	void Awake(){
	}

	void Start(){
		botonInicial.Select ();
		botonInicial.OnSelect (null);
	}
}
