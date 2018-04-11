﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour {

    void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        if (!BaseDatos.Instance.BaseDatosInicializada)
             BaseDatos.Instance.InicializarBase(CargaCompleta);
        //Recursos.InicializarCartas();
    }

    public void CargaCompleta()
    {
        if (SesionUsuario.Instance.ExisteSesion())
        {
            CargarEscenaMenu();
        }
        else
        {
            CargarEscenaLogin();
        }
    }

    public void CargarEscenaMenu()
    {
        SceneManager.LoadSceneAsync("Menu");
    }

    public void CargarEscenaLogin()
    {
        SceneManager.LoadSceneAsync("Login");
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
