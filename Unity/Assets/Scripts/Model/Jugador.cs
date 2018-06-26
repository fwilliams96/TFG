using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Jugador
{
	public enum TIPO_JUGADOR{
		MANUAL,
		AUTOMÁTICO
	}

    #region Atributos
	private TIPO_JUGADOR tipoJugador;
	private List<int> idCartasMazo;
	private List<System.Object> cartas;
	private List<System.Object> items;
	private Mazo mazo;
	private int nivel;
	private int experiencia;

    #endregion Atributos
    #region Getters/Setters

	public TIPO_JUGADOR TipoJugador{
		get{
			return tipoJugador;
		}
		set{
			tipoJugador = value;
		}
	}
		
	public int Nivel{
		get{
			return nivel;
		}
		set{ 
			nivel = value;
		}
	}

	public int Experiencia{
		get{
			return experiencia;
		}
		set{ 
			experiencia = value;
		}
	}

    #endregion

	public Jugador(TIPO_JUGADOR tipoJugador)
    {
		this.mazo = new Mazo ();
		this.idCartasMazo = new List<int>();
		this.cartas = new List<System.Object>();
		this.items = new List<System.Object>();
		this.nivel = 1;
		this.experiencia = 0;
		this.tipoJugador = tipoJugador;
    }
		
	public void ClearMazo(){
		mazo.CartasEnMazo.Clear ();
	}

	public void AñadirIDCartaMazo(int id)
	{
		idCartasMazo.Add(id);
	}

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

	/// <summary>
	/// Retorna la posicion del item en los items del jugador
	/// </summary>
	/// <returns>The posicion item.</returns>
	/// <param name="item">Item.</param>
	public int BuscarPosicionItem(Item item){
		bool trobat = false;
		int i = 0; 
		while(i < items.Count && !trobat){
			if (items [i] == item)
				trobat = true;
			else
				i+=1;
		}

		return i;
	}

	/// <summary>
	/// Busca la posicion de la carta en las cartas del jugador.
	/// </summary>
	/// <returns>The posicion carta.</returns>
	/// <param name="carta">Carta.</param>
	public int BuscarPosicionCarta(Carta carta){
		bool trobat = false;
		int i = 0; 
		while(i < cartas.Count && !trobat){
			if (cartas [i] == carta)
				trobat = true;
			else
				i+=1;
		}

		return i;
	}

	public void EliminarItem(Item item)
	{
		items.Remove(item);
	}

    public void AñadirCartaMazo(Carta carta)
    {
        mazo.CartasEnMazo.Add(carta);
    }

    public void EliminarCartaMazo(int pos)
    {
        mazo.CartasEnMazo.RemoveAt(pos);
    }

	public List<int> IDCartasMazo()
	{
		return idCartasMazo;
	}

    public List<System.Object> Cartas()
    {
        return cartas;
    }

	public int NumCartas(){
		return cartas.Count;
	}

	public List<System.Object> Items()
	{
		return items;
	}

	public int NumItems(){
		return items.Count;
	}
		
	public List<System.Object> CartasEnElMazo()
    {
        return mazo.CartasEnMazo;
    }
		
    public int NumCartasMazo()
    {
        return mazo.CartasEnMazo.Count;
    }
		
	/// <summary>
	/// Datos del jugador que se guardan luego en base de datos.
	/// </summary>
	/// <returns>The dictionary.</returns>
    public Dictionary<string, System.Object> ToDictionary()
    {
        Dictionary <string, System.Object> result = new Dictionary<string, System.Object>();
        result["nivel"] = nivel;
		result["experiencia"] = experiencia;
		result["cartas"] = CartasToDictionary ();
		result["mazo"] = MazoToDictionary ();
		result["items"] = ItemsToDictionary();
        return result;
    }

	/// <summary>
	/// Del mazo solo se cogen los identificadores de las cartas que se encuentran dentro.
	/// </summary>
	/// <returns>The to dictionary.</returns>
	public string MazoToDictionary(){
		string idCartas = "";
		foreach (int idCartaMazo in idCartasMazo)
		{
			idCartas += idCartaMazo.ToString()+",";
		}
		return idCartas.Substring (0, idCartas.Length - 1);
	}

	/// <summary>
	/// Para las cartas, su identificador es su posición dentro del array.
	/// </summary>
	/// <returns>The to dictionary.</returns>
	public Dictionary<string, System.Object> CartasToDictionary(){
		int i = 0;
		Dictionary<string, System.Object> cards = new Dictionary<string, System.Object>();
		foreach (Carta carta in cartas)
		{
			cards[i.ToString()] = carta.ToDictionary();
			i += 1;
		}
		return cards;
	}

	/// <summary>
	/// Para los items, su identificador es su posición dentro del array.
	/// </summary>
	/// <returns>The to dictionary.</returns>
	public Dictionary<string, System.Object> ItemsToDictionary(){
		int i = 0;
		Dictionary<string, System.Object> dictItems = new Dictionary<string, System.Object>();
		foreach (Item item in items)
		{
			dictItems [i.ToString ()] = item.ToDictionary ();
			i += 1;
		}
		return dictItems;
	}

	/// <summary>
	/// A partir de los identificadores, añade las cartas pertenecientes al mazo.
	/// </summary>
	public void InicializarMazo(){
		foreach (int indiceCarta in idCartasMazo) {
			AñadirCartaMazo ((Carta)cartas[indiceCarta]);
		}
	}


		
}
