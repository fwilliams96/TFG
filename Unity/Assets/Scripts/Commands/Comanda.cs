﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Comanda
{
    protected Comandas comandas;

    public Comanda()
    {
        comandas = Comandas.Instance;
    }

	/// <summary>
	/// Función que añde a la cola una comanda y la ejecuta en caso de que sea la primera comanda.
	/// </summary>
    public virtual void AñadirAlaCola()
    {
        comandas.Enqueue(this);
        if (!comandas.playingQueue)
            comandas.EjecutarPrimeraComanda();
    }

	/// <summary>
	/// Función que todas las comandas deben incorporar, es la que se llama al ejecutar una comanda.
	/// </summary>
    public virtual void EmpezarEjecucionComanda()
    {
        // list of everything that we have to do with this command (draw a card, play a card, play spell effect, etc...)
        // there are 2 options of timing : 
        // 1) use tween sequences and call CommandExecutionComplete in OnComplete()
        // 2) use coroutines (IEnumerator) and WaitFor... to introduce delays, call CommandExecutionComplete() in the end of coroutine
    }   
}
