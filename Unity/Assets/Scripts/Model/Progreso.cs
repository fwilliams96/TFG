using UnityEngine;
using System.Collections;

public class Progreso 
{
    #region Atributos
    private int material;
    private int pocion;
    #endregion
    
    public Progreso()
    {
        this.material = 0;
        this.pocion = 0;
    }

    #region Getters/Setters
    public int Material
    {
        get
        {
            return material;
        }

        set
        {
            material = value;
        }
    }

    public int Pocion
    {
        get
        {
            return pocion;
        }

        set
        {
            pocion = value;
        }
    }
    #endregion


}
