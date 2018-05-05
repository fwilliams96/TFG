using Firebase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Unity.Editor;

public class Menu : MonoBehaviour {

    public GameObject inventario;
    public GameObject batalla;
    public GameObject perfil;

    void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    public void Inventario()
    {
        //SceneManager.LoadScene("Inventario");
        inventario.SetActive(true);
        batalla.SetActive(false);
		perfil.SetActive(false);
    }

    public void Batalla()
    {
        //SceneManager.LoadScene("Batalla");
        batalla.SetActive(true);
        inventario.SetActive(false);
		perfil.SetActive(false);
    }

	public void Perfil()
	{
		//SceneManager.LoadScene("Batalla");
		perfil.SetActive(true);
		batalla.SetActive(false);
		inventario.SetActive(false);
	}
}
