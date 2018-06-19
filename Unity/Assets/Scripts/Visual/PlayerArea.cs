using UnityEngine;
using System.Collections;

public enum AreaPosition{Top, Low}

public class PlayerArea : MonoBehaviour 
{
    public AreaPosition areaPosicionJugador;
    public bool ControlActivado = true;
    public PlayerDeckVisual mazoVisual;
    public ManaPoolVisual ManaBar;
    public HandVisual manoVisual;
    public PlayerPortraitVisual Personaje;
    //public EndTurnButton EndTurnButton;
    public TableVisual tableVisual;
    public Transform PosicionPersonaje;
    public Transform PosicionInicialPersonaje;

    public bool PermitirControlJugador
    {
        get;
        set;
    }      


}
