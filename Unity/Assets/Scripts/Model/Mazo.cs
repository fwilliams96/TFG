using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mazo {
    public List<Carta> CartasEnMazo;

    public Mazo()
    {
        CartasEnMazo = new List<Carta>();
    }

    public void Mezclar()
    {
        CartasEnMazo.Shuffle();
    }



}
