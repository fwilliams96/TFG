using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mazo {
	public List<System.Object> CartasEnMazo;

    public Mazo()
    {
		CartasEnMazo = new List<System.Object>();
    }

    public void Mezclar()
    {
        CartasEnMazo.Shuffle();
    }



}
