using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccionesPopUp : MonoBehaviour {
    public GameObject PanelAcciones;
    public GameObject ataque;
    public GameObject defensa;
    public GameObject efecto;
    public static AccionesPopUp Instance;
    public delegate void RegistrarFuncion(int idEnte);
    private int idEnte;
    public event RegistrarFuncion metodo;

    public enum TipoAccion { ATAQUE, DEFENSA, EFECTO };
    private void Awake()
    {
        Instance = this;
        PanelAcciones.SetActive(false);
    }
	
	public void MostrarAccionDefensa()
    {
        MostrarAccion(TipoAccion.DEFENSA);
    }

    public void MostrarAccionAtaque()
    {
        MostrarAccion(TipoAccion.ATAQUE);
    }

    public void MostrarAccionEfecto()
    {
        MostrarAccion(TipoAccion.EFECTO);
    }

    private void MostrarAccion(TipoAccion accion) 
    {
        MostrarPopup();
        QuitarTodasAcciones();
        switch (accion)
        {
            case TipoAccion.ATAQUE:
                ataque.SetActive(true);
                break;
            case TipoAccion.DEFENSA:
                defensa.SetActive(true);
                break;
            case TipoAccion.EFECTO:
                efecto.SetActive(true);
                break;
        }
    }

    private void QuitarTodasAcciones()
    {
        ataque.SetActive(false);
        defensa.SetActive(false);
        efecto.SetActive(false);
    }

    public void MostrarPopup()
    {
        PanelAcciones.SetActive(true);
    }

    public void OcultarPopup()
    {
        PanelAcciones.SetActive(false);
    }

    public void RegistrarCallBack(RegistrarFuncion funcion, int idEnte)
    {
        metodo = funcion;
        this.idEnte = idEnte;
    }

    public void Accion()
    {
        PanelAcciones.SetActive(false);
        //Llamar a metodo registrado
        metodo.Invoke(idEnte);
    }
}
