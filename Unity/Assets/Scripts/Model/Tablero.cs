using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tablero : MonoBehaviour 
{
    public List<Criatura> CriaturasEnTablero = new List<Criatura>();

    public void PlaceCreatureAt(int index, Criatura creature)
    {
        CriaturasEnTablero.Insert(index, creature);
    }
        
}
