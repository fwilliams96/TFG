using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class JugadorPartida : MonoBehaviour,ICharacter, IIdentifiable
{
	public delegate void VoidWithNoArguments();
	public event VoidWithNoArguments EndTurnEvent;

	private int PlayerID;
	private Mano mano;
	private Mesa mesa;
	private int posCartaActual;
	private int manaEnEsteTurno;
	private int manaRestante;
	private int defensa;

	protected Jugador p;
	protected AreaPosition area;

	protected virtual void Awake(){
		PlayerID = IDFactory.GetBattleUniqueID();
		Reset ();
	}

	void Start(){
		
	}

	/// <summary>
	/// Resetea los datos iniciales del jugador de partida.
	/// </summary>
	public void Reset(){
		this.mano = new Mano();
		this.mesa = new Mesa();
		this.defensa = 3000;
		this.manaRestante = 0;
		this.posCartaActual = 0;
	}

	/// <summary>
	/// Añade mana extra al jugador.
	/// </summary>
	/// <param name="amount">Amount.</param>
	public void ConseguirManaExtra(int amount)
	{
		ManaRestante += amount;
	}

	public void AñadirEnteMesa(int posicionMesa, Ente ente)
	{
		mesa.EntesEnTablero.Insert(posicionMesa, ente);
	}

	public void AñadirCartaMano(int posicionMano, CartaPartida carta)
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

	public void EliminarCartaMano(CartaPartida carta)
	{
		mano.CartasEnMano.Remove(carta);
	}

	public List<Ente> EntesEnLaMesa()
	{
		return mesa.EntesEnTablero;
	}

	public List<CartaPartida> CartasEnLaMano()
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

	public virtual void OnTurnEnd()
	{
		if (EndTurnEvent != null)
			EndTurnEvent.Invoke();
	}

	/// <summary>
	/// Actualiza los datos del jugador
	/// </summary>
	public virtual void OnTurnStart()
	{
		ManaEnEsteTurno = 10;
		ManaRestante++;
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

	/// <summary>
	/// Determina que carta se debe sacar ahora del mazo.
	/// </summary>
	/// <value>The position carta actual.</value>
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

	public AreaPosition Area{
		get{
			return area;
		}
	}

	#endregion
}
