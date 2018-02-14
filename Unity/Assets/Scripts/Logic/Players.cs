using UnityEngine;
using System.Collections;

public class Players
{
    #region Atributos
    private Player[] players;
    private int numPlayers;
    private static Players instance = null;
    #endregion Atributos

    private Players()
    {
        players = new Player[2];
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

    public Player this[int i]
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

    public void Add(Player player)
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

    public Player [] GetPlayers()
    {
        return players;
    }


}
