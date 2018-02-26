using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosicionCriaturaPopUp : MonoBehaviour {

    public static PosicionCriaturaPopUp Instance;
    public GameObject PosicionCriaturaPanel;
    private bool ataque;
    private DragEntityOnTable creature;
    public delegate void RegistrarFuncion(bool resultOK);
    public event RegistrarFuncion metodo;

    public bool Ataque
    {
        get { return ataque; }
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

    /// <summary>
    /// Valor puede tomar tres posibles valores:
    ///     -1 -> Se ha cancelado el popup sin obtener un valor valido
    ///     0 -> Corresponde al valor de ataque
    ///     1 -> Corresponde al valor de defensa
    /// </summary>
    /// <param name="valor"></param>
    public void PosicionCriaturaElegida(int valor)
    {
        Debug.Log("Valor popup +" + valor);
        bool resultOK = valor != -1 ? true : false;
        if(resultOK)
        {
            this.ataque = valor == 0? true: false;
        }
        PosicionCriaturaPanel.SetActive(false);
        //Llamar a metodo registrado
        metodo.Invoke(resultOK);
        //metodo = null;

    }
}
