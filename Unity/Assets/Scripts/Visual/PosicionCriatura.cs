using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosicionCriatura : MonoBehaviour {

    public static PosicionCriatura Instance;
    public GameObject PosicionCriaturaPanel;
    private bool ataque;
    private bool closed;
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
        closed = true;
    }

    public void MostrarPopupEleccionPosicion()
    {
        PosicionCriaturaPanel.SetActive(true);
        closed = false;
    }

    public void PosicionCriaturaElegida(bool ataque = true)
    {
        this.ataque = ataque;
        PosicionCriaturaPanel.SetActive(false);
        closed = true;
    }
}
