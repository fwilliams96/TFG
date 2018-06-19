using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IDHolder : MonoBehaviour {

    public int UniqueID;
    private static List<IDHolder> allIDHolders = new List<IDHolder>();

    void Awake()
    {
        allIDHolders.Add(this);   
    }

    public static GameObject GetGameObjectWithID(int ID)
    {
        foreach (IDHolder i in allIDHolders)
        {
            if (i.UniqueID == ID)
                return i.gameObject;
        }
        return null;
    }

	public static List<GameObject> GetGameObjectsWithID(int ID)
	{
		List<GameObject> listGameObjects = new List<GameObject> ();
		foreach (IDHolder i in allIDHolders)
		{
			if (i.UniqueID == ID)
				listGameObjects.Add (i.gameObject);
		}
		return listGameObjects;
	}

    public static void ClearIDHoldersList()
    {
        allIDHolders.Clear();
    }

	public static void EliminarElemento(IDHolder elemento){
		allIDHolders.Remove (elemento);
	}
}
