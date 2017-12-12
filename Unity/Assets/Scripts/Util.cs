using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util {

	public static GameObject CargarCarta(int tipoCarta)
    {
        GameObject card = null;
        switch (tipoCarta)
        {
            case Global.TIPO_CARTA.Agua:
                card = Resources.Load("Cartas/Agua") as GameObject;
                break;
            case Global.TIPO_CARTA.Fuego:
                card = Resources.Load("Cartas/Fuego") as GameObject;
                break;
            case Global.TIPO_CARTA.Electrica:
                card = Resources.Load("Cartas/Electricidad") as GameObject;
                break;
            case Global.TIPO_CARTA.Tierra:
                card = Resources.Load("Cartas/Tierra") as GameObject;
                break;
            case Global.TIPO_CARTA.Fusion:
                card = Resources.Load("Cartas/Fusion") as GameObject;
                break;
            case Global.TIPO_CARTA.Magica:
                card = Resources.Load("Cartas/Magica") as GameObject;
                break;
            case Global.TIPO_CARTA.Ancestral:
                card = Resources.Load("Cartas/Ancestral") as GameObject;
                break;
            default:
                card = null;
                break;
        }
        return card;
    }
}
