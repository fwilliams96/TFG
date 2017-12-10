using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carta {

    private int tipoCarta;
    private string rutaImatge;
    private int ataque;
    private GameObject card;

	// Use this for initialization
	public Carta (int tipoCarta) {
        this.tipoCarta = tipoCarta;

    }

    public void Start()
    {
        card = Util.CargarCarta(tipoCarta);        
    }
}
