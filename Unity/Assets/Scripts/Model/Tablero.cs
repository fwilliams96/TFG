using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tablero : MonoBehaviour
{
    public List<Ente> EntesEnTablero = new List<Ente>();

    public void PlaceCreatureAt(int index, Ente ente)
    {
        EntesEnTablero.Insert(index, ente);
    }

}
