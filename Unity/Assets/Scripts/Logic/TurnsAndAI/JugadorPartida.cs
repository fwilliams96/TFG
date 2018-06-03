using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class JugadorPartida : ICharacter
{
	// CODE FOR EVENTS TO LET CREATURES KNOW WHEN TO CAUSE EFFECTS
	public delegate void VoidWithNoArguments();
	//public event VoidWithNoArguments CreaturePlayedEvent;
	//public event VoidWithNoArguments SpellPlayedEvent;
	//public event VoidWithNoArguments StartTurnEvent;
	public event VoidWithNoArguments EndTurnEvent;
	private int PlayerID;
	private Mano mano;
	private Mesa mesa;
	private int posCartaActual;
	private int manaEnEsteTurno;
	private int manaRestante;
	private int defensa;

	protected Jugador p;

	public JugadorPartida(Jugador p){
		PlayerID = IDFactory.GetUniqueID();
		this.p = p;
		Reset ();
	}

	public void Reset(){
		this.mano = new Mano();
		this.mesa = new Mesa();
		this.defensa = 3000;
		this.posCartaActual = 0;
	}

	public void ConseguirManaExtra(int amount)
	{
		//ManaEnEsteTurno += amount;
		ManaRestante += amount;
	}

	public void AñadirEnteMesa(int posicionMesa, Ente ente)
	{
		mesa.EntesEnTablero.Insert(posicionMesa, ente);
	}

	public void AñadirCartaMano(int posicionMano, Carta carta)
	{
		mano.CartasEnMano.Insert(posicionMano, carta);
	}

	public System.Object CartaActual(){
		return p.CartasEnElMazo()[PosCartaActual];
	}

	public void EliminarEnteMesa(Ente ente)
	{
		mesa.EntesEnTablero.Remove(ente);
	}

	public void EliminarCartaMano(Carta carta)
	{
		mano.CartasEnMano.Remove(carta);
	}

	public List<Ente> EntesEnLaMesa()
	{
		return mesa.EntesEnTablero;
	}

	public List<Carta> CartasEnLaMano()
	{
		return mano.CartasEnMano;
	}

	public int NumEntesEnLaMesa()
	{
		return mesa.EntesEnTablero.Count;
	}

	public int NumCartasMano()
	{
		return mano.CartasEnMano.Count;
	}

	public void OnTurnEnd()
	{
		if (EndTurnEvent != null)
			EndTurnEvent.Invoke();
		//TODO Controlador.PararTurnMaker(this);
		//GetComponent<TurnMaker>().StopAllCoroutines();
	}

	public virtual void OnTurnStart()
	{
		// add one mana crystal to the pool;
		ManaEnEsteTurno = 10;
		ManaRestante++;
		/*ManaEnEsteTurno++;
        ManaRestante = ManaEnEsteTurno;*/
		foreach (Ente cl in mesa.EntesEnTablero)
			cl.OnTurnStart();
	}

	#region Getters/Setters
	public int ID
	{
		get { return PlayerID; }
	}

	public int ManaEnEsteTurno
	{
		get { return manaEnEsteTurno; }
		set
		{
			manaEnEsteTurno = value;
		}
	}

	public int ManaRestante
	{
		get
		{ return manaRestante; }
		set
		{
			manaRestante = value;
		}
	}

	private int PosCartaActual {
		get {
			int cActual = posCartaActual;
			if (cActual == p.NumCartasMazo() - 1) {
				posCartaActual = 0;
			} else {
				posCartaActual++;
			}
			return cActual;
		}
	}

	public void Morir() { }

	public int Defensa
	{
		get { return defensa; }
		set
		{
			defensa = value;
		}
	}

	public Jugador Jugador{
		get{
			return p;
		}
		set{ 
			this.p = value;
		}
	}

	#endregion
}
