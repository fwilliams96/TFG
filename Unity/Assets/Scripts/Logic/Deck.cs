using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Deck : MonoBehaviour {

    public List<CardAsset> cartas = new List<CardAsset>();

    void Awake()
    {
        cartas.Shuffle();
    }
	
}
