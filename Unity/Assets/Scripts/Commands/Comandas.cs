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

    public void Enqueue(Comanda comanda)
    {
        CommandQueue.Enqueue(comanda);
    }

    public int Count
    {
        get
        {
            return CommandQueue.Count;
        }
        
    }

    public void Clear()
    {
        CommandQueue.Clear();
    }

    public void EjecutarPrimeraComanda()
    {
        playingQueue = true;
        CommandQueue.Dequeue().EmpezarEjecucionComanda();
    }

    public void CompletarEjecucionComanda()
    {
        if (Count > 0)
            EjecutarPrimeraComanda();
        else
            playingQueue = false;
        if (TurnManager.Instance.whoseTurn != null)
            TurnManager.Instance.whoseTurn.HighlightPlayableCards();
    }

    public bool ComandasDeDibujoCartaPendientes()
    {
        foreach (Comanda c in CommandQueue)
        {
            if (c is DrawACardCommand)
                return true;
        }
        return false;
    }
}