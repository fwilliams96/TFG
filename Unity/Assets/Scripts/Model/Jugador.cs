using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Jugador : ICharacter
{

    #region Atributos
    // PUBLIC FIELDS
    // int ID that we get from ID factory
    private int PlayerID;

    // REFERENCES TO LOGICAL STUFF THAT BELONGS TO THIS PLAYER
    //private Dictionary<string, System.Object> cartas = new Dictionary<string, System.Object>();
    private List<System.Object> cartas = new List<System.Object>();
	private List<System.Object> items = new List<System.Object>();
    private Mazo mazo;
    private Mano mano;
    private Mesa mesa;
    private int nivel;
    private string area;
	private int posCartaActual;

    #endregion Atributos
    #region Getters/Setters
    // PROPERTIES 
    // this property is a part of interface ICharacter
    public int ID
    {
        get { return PlayerID; }
    }

    // total mana crystals that this player has this turn
    private int manaEnEsteTurno;
    public int ManaEnEsteTurno
    {
        get { return manaEnEsteTurno; }
        set
        {
            manaEnEsteTurno = value;
        }
    }

    // full mana crystals available right now to play cards / use hero power 
    private int manaRestante;
    public int ManaRestante
    {
        get
        { return manaRestante; }
        set
        {
            manaRestante = value;
        }
    }

    private int defensa;
    public int Defensa
    {
        get { return defensa; }
        set
        {
            defensa = value;
        }
    }

    public string Area
    {
        get
        {
            return area;
        }
    }
	private int PosCartaActual {
		get {
			int cActual = posCartaActual;
			if (cActual == NumCartasMazo() - 1) {
				posCartaActual = 0;
			} else {
				posCartaActual++;
			}
			return cActual;
		}
	}
    #endregion

    // CODE FOR EVENTS TO LET CREATURES KNOW WHEN TO CAUSE EFFECTS
    public delegate void VoidWithNoArguments();
    //public event VoidWithNoArguments CreaturePlayedEvent;
    //public event VoidWithNoArguments SpellPlayedEvent;
    //public event VoidWithNoArguments StartTurnEvent;
    public event VoidWithNoArguments EndTurnEvent;

	public Jugador()
	{
		PlayerID = IDFactory.GetUniqueID();
		this.mano = new Mano();
		this.mazo = new Mazo();
		this.mesa = new Mesa();
		this.defensa = 3000;
		this.nivel = 0;
	}

	public Jugador(string area)
    {
        PlayerID = IDFactory.GetUniqueID();
        this.mano = new Mano();
        this.mazo = new Mazo();
        this.mesa = new Mesa();
        this.defensa = 3000;
		this.nivel = 0;
		this.area = area;
		this.posCartaActual = 0;
    }

    //TODO get mana from coin or other spells 
    public void ConseguirManaExtra(int amount)
    {
        ManaEnEsteTurno += amount;
        ManaRestante += amount;
    }

    public void Morir() { }

    public void AñadirCarta(Carta carta)
    {
        cartas.Add(carta);
    }

	public void AñadirItem(Item item)
	{
		items.Add(item);
	}

    public void EliminarCarta(Carta carta)
    {
		cartas.Remove(carta);
    }

	public void EliminarItem(Item item)
	{
		items.Remove(item);
	}

    public void AñadirEnteMesa(int posicionMesa, Ente ente)
    {
        mesa.EntesEnTablero.Insert(posicionMesa, ente);
    }

    public void AñadirCartaMano(int posicionMano, Carta carta)
    {
        mano.CartasEnMano.Insert(posicionMano, carta);
    }

    public void AñadirCartaMazo(Carta carta)
    {
        mazo.CartasEnMazo.Add(carta);
    }

    public void EliminarEnteMesa(Ente ente)
    {
        mesa.EntesEnTablero.Remove(ente);
    }

    public void EliminarCartaMano(Carta carta)
    {
        mano.CartasEnMano.Remove(carta);
    }

    public void EliminarCartaMazo(int pos)
    {
        mazo.CartasEnMazo.RemoveAt(pos);
    }

    public List<System.Object> Cartas()
    {
        return cartas;
    }

	public List<System.Object> Items()
	{
		return items;
	}

    public Ente[] EntesEnLaMesa()
    {
        return mesa.EntesEnTablero.ToArray();
    }

    public Carta[] CartasEnLaMano()
    {
        return mano.CartasEnMano.ToArray();
    }

    public Carta[] CartasEnElMazo()
    {
        return mazo.CartasEnMazo.ToArray();
    }

	public Carta CartaActual(){
		return mazo.CartasEnMazo [PosCartaActual];
	}

    public int NumEntesEnLaMesa()
    {
        return mesa.EntesEnTablero.Count;
    }

    public int NumCartasMano()
    {
        return mano.CartasEnMano.Count;
    }

    public int NumCartasMazo()
    {
        return mazo.CartasEnMazo.Count;
    }

    public virtual void OnTurnStart()
    {
        // add one mana crystal to the pool;
        ManaEnEsteTurno++;
        ManaRestante = ManaEnEsteTurno;
        foreach (Ente cl in mesa.EntesEnTablero)
            cl.OnTurnStart();
    }

    public void OnTurnEnd()
    {
        if (EndTurnEvent != null)
            EndTurnEvent.Invoke();
        //TODO Controlador.PararTurnMaker(this);
        //GetComponent<TurnMaker>().StopAllCoroutines();
    }

    public Dictionary<string, System.Object> ToDictionary()
    {
        Dictionary <string, System.Object> result = new Dictionary<string, System.Object>();
        result["nivel"] = nivel;
        int i = 0;
        Dictionary<string, System.Object> cards = new Dictionary<string, System.Object>();
        foreach (Carta carta in cartas)
        {
            cards[i.ToString()] = carta.ToDictionary();
            i += 1;
        }
		result["cartas"] = cards;
		i = 0;
		Dictionary<string, System.Object> dictItems = new Dictionary<string, System.Object>();
		foreach (Item item in items)
		{
			dictItems [i.ToString ()] = item.ToDictionary ();
			i += 1;
		}
		result["items"] = dictItems;
        return result;
    }

	public void InicializarMazo(){
		foreach (System.Object carta in cartas) {
			AñadirCartaMazo ((Carta)carta);
		}
	}
}
