using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class IDFactory {

    private static int Count;

    public static int GetUniqueID()
    {
        // Count++ has to go first, otherwise - unreachable code.
        Count++;
        return Count;
    }

	/// <summary>
	/// Elimina todos los identificadores creados
	/// </summary>
	public static void ResetIDs()
    {
        Count = 0;
    }

	/// <summary>
	/// Resetea hasta los elementos constantes del jugador (items y cartas)
	/// </summary>
	public static void EliminarIDsBatalla()
	{
		//Cartas enemigo
		for (int i = 0; i < 8; i++) {
			Count -= 1;
		}
		//Entes enemigo
		for (int i = 0; i < 8; i++) {
			Count -= 1;
		}
		//Enemigo
		Count -= 1;
	}

	public static void EliminarID(){
		Count -= 1;
	}



}
