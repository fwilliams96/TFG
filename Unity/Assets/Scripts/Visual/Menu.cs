using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {

    public GameObject inventario;
    public GameObject batalla;
    public GameObject perfil;

	// Use this for initialization
	void Start () {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Inventario()
    {
        //SceneManager.LoadScene("Inventario");
        inventario.SetActive(true);
        batalla.SetActive(false);
    }

    public void Batalla()
    {
        //SceneManager.LoadScene("Batalla");
        batalla.SetActive(true);
        inventario.SetActive(false);
    }
}
