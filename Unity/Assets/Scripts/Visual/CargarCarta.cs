using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargarCarta : MonoBehaviour {
	public UnityEngine.UI.GridLayoutGroup gridLayoutGroup;
	GameObject elemento;
	// Use this for initialization

	void OnEnable(){
		Debug.Log ("On enable");
		elemento = Instantiate (TouchManager2.Instance.ObjetoActual, transform);
		elemento.transform.parent = gridLayoutGroup.gameObject.transform;
	}

	void OnDisable(){
		Debug.Log ("On disable");
		Destroy (elemento);
	}

	void Start () {
		/*GameObject elemento = Instantiate (TouchManager2.Instance.ObjetoActual, transform);
		elemento.transform.parent = gridLayoutGroup.gameObject.transform;*/
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
