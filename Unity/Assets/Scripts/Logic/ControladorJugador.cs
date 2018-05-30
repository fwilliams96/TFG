using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControladorJugador
{
    #region Atributos
    private static ControladorJugador instance;
    private Jugador _jugadorActual;
    private PlayerArea areaJugadorActual;
    #endregion

    #region Getters/Setters
    public Jugador JugadorActual
    {
        get
        {
            return _jugadorActual;
        }

        set
        {
            areaJugadorActual = AreaJugador(value);
            _jugadorActual = value;
        }
    }
    #endregion

    private ControladorJugador()
    {
        Recursos.InicializarJugadores();
    }

    public static ControladorJugador Instance
    {
        get
        {
            if (instance == null)
                instance = new ControladorJugador();
            return instance;
        }
    }

    public PlayerArea AreaJugador(Jugador jugador)
    {
        if (jugador == _jugadorActual)
            return areaJugadorActual;
        PlayerArea areaJugador;
        AreaPosition area = jugador.Area.Equals("Top") ? AreaPosition.Top : AreaPosition.Low;
        switch (area)
        {
            case AreaPosition.Low:
                areaJugador = GameObject.FindGameObjectWithTag("AreaBaja").GetComponent<PlayerArea>();
                break;
            case AreaPosition.Top:
                areaJugador = GameObject.FindGameObjectWithTag("AreaAlta").GetComponent<PlayerArea>();
                break;
            default:
                areaJugador = null;
                break;
        }
        return areaJugador;
    }

    public bool SePermiteControlarElJugador(Jugador jugador)
    {
        PlayerArea areaJugador = AreaJugador(jugador);
        bool TurnoDelJugador = (_jugadorActual == jugador);
        bool NoCartasPendientesPorMostrar = !Comandas.Instance.ComandasDeDibujoCartaPendientes();
        return areaJugador.PermitirControlJugador && areaJugador.ControlActivado && TurnoDelJugador && NoCartasPendientesPorMostrar;
    }

    //TODO ver si esta funcion seguira aqui
    public void TransmitirInformacionVisualJugador(Jugador jugador)
    {
        PlayerArea areaJugador = AreaJugador(jugador);
        //TODO cuando el jugador muere esta dando un pete aqui por acceder a algo destruido
        areaJugador.Personaje.gameObject.AddComponent<IDHolder>().UniqueID = jugador.ID;
        areaJugador.PermitirControlJugador = true;
        /*if (jugador.GetComponent<TurnMaker>() is AITurnMaker)
        {
            // turn off turn making for this character
            areaJugador.PermitirControlJugador = false;
        }
        else
        {
            // allow turn making for this character
            areaJugador.PermitirControlJugador = true;
        }*/
    }

    public void InicializarValoresJugador(Jugador jugador)
    {
        PlayerArea areaJugador = AreaJugador(jugador);
        jugador.ManaEnEsteTurno = 0;
        jugador.ManaRestante = 0;
        //LeerInformacionPersonajeAsset();
        TransmitirInformacionVisualJugador(jugador);
        areaJugador.mazoVisual.CartasEnMazo = jugador.NumCartasMazo();//mazo.CartasEnMazo.Count;
        // move both portraits to the center
        areaJugador.Personaje.transform.position = areaJugador.PosicionInicialPersonaje.position;
    }

    public void ActualizarValoresJugador()
    {
        //TurnMaker tm = JugadorActual.GetComponent<TurnMaker>();
        // player`s method OnTurnStart() will be called in tm.OnTurnStart();
        //Aqui se crea la comanda para dar la carta al jugador
        OnTurnStart();
        //tm.OnTurnStart();
        //if (tm is PlayerTurnMaker)
        //{
            ActualizarManaJugador(JugadorActual);
            ActualizarEstadoCartasJugadorActual(JugadorActual);
        //}
        ActualizarEstadoCartasJugadorEnemigo(JugadorActual);
    }

    private void OnTurnStart()
    {
        JugadorActual.OnTurnStart();
        if (JugadorActual.Area.Equals("Top"))
            new ShowMessageCommand("¡Turno enemigo!", 2.0f).AñadirAlaCola();
        else
            new ShowMessageCommand("¡Tu Turno!", 2.0f).AñadirAlaCola();
        Controlador.Instance.DibujarCartaMazo(JugadorActual);
    }

    public void ActualizarManaJugador(Jugador jugador)
    {
        new UpdateManaCrystalsCommand(jugador, jugador.ManaEnEsteTurno, jugador.ManaRestante).AñadirAlaCola();
    }

    // Muestra cartas jugables de la mano del jugador
    public void ActualizarEstadoCartasJugadorActual(Jugador jugador)
    {
        if (jugador == JugadorActual)
            ActualizarEstadoCartasJugador(jugador, false);

    }

    private void ActualizarEstadoCartasJugadorEnemigo(Jugador jugador)
    {
        if (jugador == JugadorActual)
            ActualizarEstadoCartasJugador(OtroJugador(jugador), true);
            }

    private void ActualizarEstadoCartasJugador(Jugador jugador, bool quitarTodasRemarcadas = false)
    {
        foreach (Carta cl in jugador.CartasEnLaMano())
        {
            GameObject g = IDHolder.GetGameObjectWithID(cl.ID);
            if (g != null)
                g.GetComponent<OneCardManager>().PuedeSerJugada = (cl.CosteManaActual <= JugadorActual.ManaRestante) && !quitarTodasRemarcadas;
        }

        foreach (Ente crl in jugador.EntesEnLaMesa())
        {
			if (crl.GetType () == typeof(Criatura)) 
				((Criatura)crl).HaAtacado = false;
            GameObject g = IDHolder.GetGameObjectWithID(crl.ID);

			if (g != null) {
				g.GetComponent<OneEnteManager>().PuedeAtacar = (crl.AtaquesRestantesEnTurno > 0) && !quitarTodasRemarcadas;
			}
                
        }
    }

    /// <summary>
    /// Funcion que para el movimiento de los jugadores, el temporizador de turno y lanza el mensaje de fin de batalla
    /// </summary>
    public void MuerteJugador(Jugador jugador)
    {
        PararControlJugadores();
        Controlador.Instance.StopTheTimer();
		new MuerteJugadorCommand (jugador).AñadirAlaCola ();
		if (jugador.Area.Equals ("Low"))
			new GameOverCommand (jugador).AñadirAlaCola ();
		else {
			Jugador ganador = OtroJugador (jugador);
			Carta carta = ObtenerCartaPremio ();
			List<Item> items = ObtenerItemsPremio ();
			AñadirPremioJugador (ganador,carta,items);
			int exp = AñadirExperienciaJugador (ganador);
			BaseDatos.Instance.ActualizarJugadorBaseDatos (carta != null);
			new PremioPartidaCommand (jugador,carta,items,exp).AñadirAlaCola ();
		}
			
    }

	private Carta ObtenerCartaPremio(){
		
		Carta carta = null;
		System.Random rnd = new System.Random ();
		if(rnd.Next(0,2) == 0)
			carta = BaseDatos.Instance.GenerarCartasAleatorias (1)[0];
		return carta;
	}

	private List<Item> ObtenerItemsPremio(){

		List<Item> items = BaseDatos.Instance.GenerarItemsAleatorios (3);
		return items;
	}

	private void AñadirPremioJugador(Jugador jugador,Carta carta, List<Item> items){
		if (carta != null)
			jugador.AñadirCarta (carta);
		foreach (Item item in items) {
			jugador.AñadirItem (item);
		}
	}

	private int AñadirExperienciaJugador(Jugador jugador){
		System.Random rnd = new System.Random ();
		int exp = rnd.Next (5, 15);
		jugador.Experiencia += exp; 
		if (jugador.Experiencia >= 100) {
			jugador.Experiencia -= 100;
			jugador.Nivel += 1;
		}
		return exp;
	}

    public void PararControlJugadores()
    {
		foreach (Jugador player in BaseDatos.Instance.GetPlayers())
        {
            AreaJugador(player).ControlActivado = false;
        }
    }

    public bool CartaOCriaturaDelJugador(string tagCartaOCriatura)
    {
        return _jugadorActual.Area.Substring(0,3).Equals(tagCartaOCriatura.Substring(0, 3));
    }

	public bool SePuedeAtacarJugadorDeCara(int idJugador){
		Jugador jugador = Controlador.Instance.Local.ID == idJugador ? Controlador.Instance.Local : Controlador.Instance.Enemigo;
		return jugador.NumEntesEnLaMesa () == 0;
	}

	public void AtacarJugador(Criatura atacante,  Jugador jugadorObjetivo)
	{
		atacante.AtaquesRestantesEnTurno--;
		new CreatureAttackCommand(jugadorObjetivo.ID, atacante.ID, atacante.Ataque,jugadorObjetivo.Defensa).AñadirAlaCola();
		jugadorObjetivo.Defensa -= atacante.Ataque;
		if(JugadorMuerto(jugadorObjetivo))
			MuerteJugador(jugadorObjetivo);
	}

    public Jugador OtroJugador(Jugador jugador)
    {
		return BaseDatos.Instance.Local == jugador ? BaseDatos.Instance.Enemigo : BaseDatos.Instance.Local;
    }

    /// <summary>
    /// Resta mana al jugador según la carta lanzada al tablero
    /// </summary>
    /// <param name="jugador"></param>
    /// <param name="carta"></param>
    public void RestarManaCarta(Jugador jugador, Carta carta)
    {
		jugador.ManaRestante -= carta.CosteManaActual;
		//jugador.ManaEnEsteTurno -= carta.CosteManaActual;
		//jugador.ManaRestante = jugador.ManaEnEsteTurno;
    }

	public void QuitarVidaJugador(Jugador jugadorObjetivo,int valorAtaque)
    {
		new DealDamageCommand(jugadorObjetivo.ID, valorAtaque, jugadorObjetivo.Defensa).AñadirAlaCola();
        jugadorObjetivo.Defensa -= valorAtaque;
        if (JugadorMuerto(jugadorObjetivo))
            MuerteJugador(jugadorObjetivo);
    }

    public bool JugadorMuerto(Jugador jugador)
    {
        return jugador.Defensa <= 0;
    }

    public void OcultarManoJugadorAnterior()
    {
        //Ocultamos la mano del jugador anterior 
        AreaJugador(OtroJugador(JugadorActual)).manoVisual.OcultarMano();
    }

    public void MostrarManoJugadorActual()
    {
        //Mostramos la mano del nuevo jugador actual
        AreaJugador(JugadorActual).manoVisual.MostrarMano();
    }

	public void Clear(){
		instance = null;
	}

}
