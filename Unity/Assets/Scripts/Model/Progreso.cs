using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Progreso 
{
    #region Atributos
    private int piedra;
    private int pocion;
    #endregion
    
    public Progreso()
    {
		this.piedra = 0;
        this.pocion = 0;
    }

    #region Getters/Setters
    public int Piedra
    {
        get
        {
            return piedra;
        }

        set
        {
            piedra = value;
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

	/// <summary>
	/// Determina los atributos necesarios que se guardan en base de datos del progreso.
	/// </summary>
	/// <returns>The dictionary.</returns>
    public Dictionary<string, System.Object> ToDictionary()
    {
        Dictionary<string, System.Object> result = new Dictionary<string, System.Object>();
        result["material"] = piedra;
        result["pocion"] = pocion;
        return result;
    }

    #endregion


}
