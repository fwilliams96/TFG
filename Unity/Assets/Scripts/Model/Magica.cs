using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Magica: Ente,ICharacter 
{
    private bool efectoActivado;
    public bool EfectoActivado
    {
        get
        {
            return efectoActivado;
        }
        set
        {
            efectoActivado = value;
        }
    }
    // CONSTRUCTOR
    public Magica(CartaAsset ca) : base(ca) { efectoActivado = false; }

}
