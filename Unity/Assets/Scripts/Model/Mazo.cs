using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mazo : MonoBehaviour {

    public List<CardAsset> CartasEnMazo = new List<CardAsset>();

    void Awake()
    {
        CartasEnMazo.Shuffle();
    }
	
}
