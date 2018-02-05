using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosicionCriatura : MonoBehaviour {

    public static PosicionCriatura Instance;
    public GameObject PosicionCriaturaPanel;
    private bool ataque;
    private bool closed;
    private DragCreatureOnTable creature;
    public delegate void RegistrarFuncion(bool result);
    public event RegistrarFuncion metodo;

    public bool Ataque
    {
        get { return ataque; }
    }

    public bool Closed
    {
        get { return closed; }
    }

    void Awake()
    {
        Instance = this;
        PosicionCriaturaPanel.SetActive(false);
    }

    public void MostrarPopupEleccionPosicion()
    {
        PosicionCriaturaPanel.SetActive(true);
    }

    public void RegistrarCallBack(RegistrarFuncion funcion)
    {
        metodo = funcion;
    }

    public void PosicionCriaturaElegida(bool ataque = true)
    {
        this.ataque = ataque;
        PosicionCriaturaPanel.SetActive(false);
        //Llamar a metodo registrado
        metodo.Invoke(ataque);
        //metodo = null;

    }
}
