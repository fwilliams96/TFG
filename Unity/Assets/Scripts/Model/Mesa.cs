using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mesa
{
    public List<Ente> EntesEnTablero;

    public Mesa()
    {
        EntesEnTablero = new List<Ente>();
    }

    public void PlaceCreatureAt(int index, Ente ente)
    {
        EntesEnTablero.Insert(index, ente);
    }

}
