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
		ControladorMenu.Instance.PantallaActual = PANTALLA_MENU.INVENTARIO;
        inventario.SetActive(true);
        batalla.SetActive(false);
		perfil.SetActive(false);
		mazo.SetActive (false);
    }

    public void Batalla()
    {
		ControladorMenu.Instance.PantallaActual = PANTALLA_MENU.BATALLA;
        batalla.SetActive(true);
        inventario.SetActive(false);
		perfil.SetActive(false);
		mazo.SetActive(false);
    }

	public void Perfil()
	{
		ControladorMenu.Instance.PantallaActual = PANTALLA_MENU.PERFIL;
		perfil.SetActive(true);
		batalla.SetActive(false);
		inventario.SetActive(false);
		mazo.SetActive(false);
	}

	public void  Mazo()
	{
		ControladorMenu.Instance.PantallaActual = PANTALLA_MENU.MAZO;
		mazo.SetActive(true);
		batalla.SetActive(false);
		inventario.SetActive(false);
		perfil.SetActive(false);
	}
}
