using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class IDFactory {

	private static int countMenu;
	private static int countBattle;
	private static int countPreviousMenu;

    public static int GetMenuUniqueID()
    {
        // Count++ has to go first, otherwise - unreachable code.
        countMenu++;
        return countMenu;
    }

	public static int GetBattleUniqueID()
	{
		// Count++ has to go first, otherwise - unreachable code.
		countBattle++;
		return countBattle;
	}

	/// <summary>
	/// Elimina todos los identificadores creados
	/// </summary>
	public static void ResetAllIDs()
    {
        countMenu = 0;
		countBattle = 0;
    }

	/// <summary>
	/// Resetea los identificadores de la batalla.
	/// </summary>
	public static void ResetIDsBattle()
	{
		countBattle = 0;
	}

	/// <summary>
	/// Resetea los identificadores de los menus.
	/// </summary>
	public static void ResetIDsMenu(){
		countMenu = 0;
	}

	public static void RecoverCountMenu(){
		countMenu = countPreviousMenu;
	}

	public static int CountMenu{
		get{
			return countMenu;
		}
	}

	public static int CountBattle{
		get{
			return countBattle;
		}
	}

	public static void SaveCountMenu(){
		countPreviousMenu = countMenu;
	}
}
