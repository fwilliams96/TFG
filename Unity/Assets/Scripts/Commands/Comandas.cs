using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Comandas
{
    private Queue<Comanda> CommandQueue;
    private static Comandas instance = null;
    public bool playingQueue = false;

    private Comandas()
    {
        CommandQueue = new Queue<Comanda>();
    }

    public static Comandas Instance
    {
        get{
            if (instance == null)
                instance = new Comandas();
            return instance;
        }
    }

	/// <summary>
	/// Se añade una comanda a la cola.
	/// </summary>
	/// <param name="comanda">Comanda.</param>
    public void Enqueue(Comanda comanda)
    {
        CommandQueue.Enqueue(comanda);
    }

	/// <summary>
	/// Devuelve el número de comandas.
	/// </summary>
	/// <value>The count.</value>
    public int Count
    {
        get
        {
            return CommandQueue.Count;
        }
        
    }

	/// <summary>
	/// Vacía la cola.
	/// </summary>
    public void Clear()
    {
        CommandQueue.Clear();
    }

	/// <summary>
	/// Se saca la primera comanda y se ejecuta.
	/// </summary>
    public void EjecutarPrimeraComanda()
    {
        playingQueue = true;
        CommandQueue.Dequeue().EmpezarEjecucionComanda();
    }

	/// <summary>
	/// Una vez la comanda se ha completado, se ejecuta la siguiente comanda.
	/// </summary>
    public void CompletarEjecucionComanda()
    {
        if (Count > 0)
            EjecutarPrimeraComanda();
        else
            playingQueue = false;
    }

	/// <summary>
	/// Mira si existen comandas de dibujo de cartas en cola.
	/// </summary>
	/// <returns><c>true</c>, if de dibujo carta pendientes was comandased, <c>false</c> otherwise.</returns>
    public bool ComandasDeDibujoCartaPendientes()
    {
        foreach (Comanda c in CommandQueue)
        {
            if (c is DrawACardCommand)
                return true;
        }
        return false;
    }

	/// <summary>
	/// Mira si hay comandas de cambio de turno en cola.
	/// </summary>
	/// <returns><c>true</c>, if de cambio turno pendientes was comandased, <c>false</c> otherwise.</returns>
	public bool ComandasDeCambioTurnoPendientes()
	{
		foreach (Comanda c in CommandQueue)
		{
			if (c is StartATurnCommand)
				return true;
		}
		return false;
	}
}