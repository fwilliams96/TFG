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
	
	/*public static void ResetIDs()
    {
        Count = 0;
    }*/

	public static void ResetIDs()
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
