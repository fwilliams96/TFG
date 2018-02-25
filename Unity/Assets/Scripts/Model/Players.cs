using UnityEngine;
using System.Collections;

public class Players
{
    #region Atributos
    private Jugador[] players;
    private int numPlayers;
    private static Players instance = null;
    #endregion Atributos

    private Players()
    {
        players = new Jugador[2];
        numPlayers = 0;
    }

    public static Players Instance
    {
        get
        {
            if (instance == null)
                instance = new Players();
            return instance;
        }
    }

    public Jugador this[int i]
    {
        get
        {
            return players[i];
        }
        set
        {
            players[i] = value;
        }
    }

    public void Add(Jugador player)
    {
        players[numPlayers] = player;
        numPlayers += 1;
    }

    public int Length
    {
        get
        {
            return numPlayers;
        }
    }

    public Jugador [] GetPlayers()
    {
        return players;
    }


}
