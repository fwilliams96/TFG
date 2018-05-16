using Firebase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Unity.Editor;

public class Menu : MonoBehaviour {

    public GameObject inventario;
    public GameObject batalla;
    public GameObject perfil;
	public GameObject mazo;

    void Awake()
    {
    }

    public void Inventario()
    {
        inventario.SetActive(true);
        batalla.SetActive(false);
		perfil.SetActive(false);
		mazo.SetActive (false);
		ControladorMenu.Instance.PantallaActual = PANTALLA_MENU.INVENTARIO;
    }

    public void Batalla()
    {
        batalla.SetActive(true);
        inventario.SetActive(false);
		perfil.SetActive(false);
		mazo.SetActive(false);
		ControladorMenu.Instance.PantallaActual = PANTALLA_MENU.BATALLA;
    }

	public void Perfil()
	{
		perfil.SetActive(true);
		batalla.SetActive(false);
		inventario.SetActive(false);
		mazo.SetActive(false);
		ControladorMenu.Instance.PantallaActual = PANTALLA_MENU.PERFIL;
	}

	public void  Mazo()
	{
		mazo.SetActive(true);
		batalla.SetActive(false);
		inventario.SetActive(false);
		perfil.SetActive(false);
		ControladorMenu.Instance.PantallaActual = PANTALLA_MENU.MAZO;
	}
}
