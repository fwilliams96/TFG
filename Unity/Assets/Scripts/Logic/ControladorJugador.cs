using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControladorJugador
{
    #region Atributos
    private static ControladorJugador instance;
	private JugadorPartida _jugadorActual;
    private PlayerArea areaJugadorActual;
    #endregion

    #region Getters/Setters
	public JugadorPartida JugadorActual
    {
        get
        {
            return _jugadorActual;
        }

        set
        {
			areaJugadorActual = AreaJugador(value.Jugador);
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
		if (null != _jugadorActual && jugador == _jugadorActual.Jugador)
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

	public bool SePermiteControlarElJugador(JugadorPartida jugador)
    {
		PlayerArea areaJugador = AreaJugador(jugador.Jugador);
        bool TurnoDelJugador = (_jugadorActual == jugador);
        bool NoCartasPendientesPorMostrar = !Comandas.Instance.ComandasDeDibujoCartaPendientes();
        return areaJugador.PermitirControlJugador && areaJugador.ControlActivado && TurnoDelJugador && NoCartasPendientesPorMostrar;
    }

    //TODO ver si esta funcion seguira aqui
	public void TransmitirInformacionVisualJugador(JugadorPartida jugador)
    {
		PlayerArea areaJugador = AreaJugador(jugador.Jugador);
		areaJugador.Personaje.gameObject.AddComponent<IDHolder>().UniqueID = jugador.ID;
        areaJugador.PermitirControlJugador = true;
		if (jugador.GetType() == typeof(JugadorBot))
        {
            areaJugador.PermitirControlJugador = false;
        }
        else
        {
            areaJugador.PermitirControlJugador = true;
        }
    }

	public void InicializarValoresJugador(JugadorPartida jugador)
    {
		PlayerArea areaJugador = AreaJugador(jugador.Jugador);
        jugador.ManaEnEsteTurno = 0;
        jugador.ManaRestante = 0;
        //LeerInformacionPersonajeAsset();
        TransmitirInformacionVisualJugador(jugador);
		areaJugador.mazoVisual.CartasEnMazo = jugador.Jugador.NumCartasMazo();//mazo.CartasEnMazo.Count;
        // move both portraits to the center
        areaJugador.Personaje.transform.position = areaJugador.PosicionInicialPersonaje.position;
    }

	public void DeshabilitarMana(JugadorPartida jugador){
		new DeshabilitarManaCommand (jugador).AñadirAlaCola ();
	}
	public void ActualizarValoresJugador(JugadorPartida jugador)
    {
		OnTurnStart(jugador);
    }

	private void OnTurnStart(JugadorPartida jugador)
    {
		jugador.OnTurnStart();
		if (jugador.Jugador.Area.Equals("Top"))
            new ShowMessageCommand("¡Turno enemigo!", 2.0f).AñadirAlaCola();
        else
            new ShowMessageCommand("¡Tu Turno!", 2.0f).AñadirAlaCola();
		Controlador.Instance.DibujarCartaMazo(jugador);
		if (jugador.GetType() == typeof(JugadorBot)) {
			Controlador.Instance.StartCoroutine(((JugadorBot)jugador).RealizarTurno());
		} else {
			ActualizarManaJugador(jugador);
			ActualizarEstadoCartasJugadorActual(jugador);
		}
		if (OtroJugador(jugador).GetType () == typeof(JugadorHumano)) {
			ActualizarEstadoCartasJugadorEnemigo(jugador);
		}
			
    }

	public void ActualizarManaJugador(JugadorPartida jugador)
    {
        new UpdateManaCrystalsCommand(jugador, jugador.ManaEnEsteTurno, jugador.ManaRestante).AñadirAlaCola();
    }

    // Muestra cartas jugables de la mano del jugador
	public void ActualizarEstadoCartasJugadorActual(JugadorPartida jugador)
    {
        if (jugador == JugadorActual)
            ActualizarEstadoCartasJugador(jugador, false);

    }

	private void ActualizarEstadoCartasJugadorEnemigo(JugadorPartida jugador)
    {
        if (jugador == JugadorActual)
            ActualizarEstadoCartasJugador(OtroJugador(jugador), true);
            }

	private void ActualizarEstadoCartasJugador(JugadorPartida jugador, bool quitarTodasRemarcadas = false)
    {
        foreach (Carta cl in jugador.CartasEnLaMano())
        {
            GameObject g = IDHolder.GetGameObjectWithID(cl.ID);
            if (g != null)
				g.GetComponent<OneCardManager>().PuedeSerJugada = Controlador.Instance.CartaPuedeUsarse(jugador,cl) && !quitarTodasRemarcadas;
        }

        foreach (Ente crl in jugador.EntesEnLaMesa())
        {
			if (crl.GetType () == typeof(Criatura)) 
				((Criatura)crl).HaAtacado = false;
            GameObject g = IDHolder.GetGameObjectWithID(crl.ID);

			if (g != null) {
				g.GetComponent<OneEnteManager>().PuedeAtacar = Controlador.Instance.EntePuedeUsarse(crl) && !quitarTodasRemarcadas;
			}
                
        }
    }

    /// <summary>
    /// Funcion que para el movimiento de los jugadores, el temporizador de turno y lanza el mensaje de fin de batalla
    /// </summary>
	public void MuerteJugador(JugadorPartida jugador)
    {
        PararControlJugadores();
        Controlador.Instance.StopTheTimer();
		new MuerteJugadorCommand (jugador).AñadirAlaCola ();
		if (jugador.Jugador.Area.Equals ("Low"))
			new GameOverCommand (jugador).AñadirAlaCola ();
		else {
			JugadorPartida ganador = OtroJugador (jugador);
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

	private void AñadirPremioJugador(JugadorPartida jugador,Carta carta, List<Item> items){
		if (carta != null)
			jugador.Jugador.AñadirCarta (carta);
		foreach (Item item in items) {
			jugador.Jugador.AñadirItem (item);
		}
	}

	private int AñadirExperienciaJugador(JugadorPartida jugador){
		System.Random rnd = new System.Random ();
		int exp = rnd.Next (5, 15);
		jugador.Jugador.Experiencia += exp; 
		if (jugador.Jugador.Experiencia >= 100) {
			jugador.Jugador.Experiencia -= 100;
			jugador.Jugador.Nivel += 1;
		}
		return exp;
	}

    public void PararControlJugadores()
    {
		foreach (JugadorPartida player in Controlador.Instance.Jugadores)
        {
			AreaJugador(player.Jugador).ControlActivado = false;
        }
    }

    public bool CartaOCriaturaDelJugador(string tagCartaOCriatura)
    {
		return _jugadorActual.Jugador.Area.Substring(0,3).Equals(tagCartaOCriatura.Substring(0, 3));
    }

	public bool SePuedeAtacarJugadorDeCara(int idJugador){
		JugadorPartida jugador = Controlador.Instance.Local.ID == idJugador ? Controlador.Instance.Local : Controlador.Instance.Enemigo;
		return jugador.NumEntesEnLaMesa () == 0;
	}

	public void AtacarJugador(Criatura atacante,  JugadorPartida jugadorObjetivo)
	{
		atacante.AtaquesRestantesEnTurno--;
		new CreatureAttackCommand(jugadorObjetivo.ID, atacante.ID, atacante.Ataque,jugadorObjetivo.Defensa).AñadirAlaCola();
		jugadorObjetivo.Defensa -= atacante.Ataque;
		if(JugadorMuerto(jugadorObjetivo))
			MuerteJugador(jugadorObjetivo);
	}

	public JugadorPartida OtroJugador(JugadorPartida jugador)
    {
		return Controlador.Instance.Local == jugador ? Controlador.Instance.Enemigo : Controlador.Instance.Local;
    }

    /// <summary>
    /// Resta mana al jugador según la carta lanzada al tablero
    /// </summary>
    /// <param name="jugador"></param>
    /// <param name="carta"></param>
    public void RestarManaCarta(JugadorPartida jugador, Carta carta)
    {
		jugador.ManaRestante -= carta.CosteManaActual;
		//jugador.ManaEnEsteTurno -= carta.CosteManaActual;
		//jugador.ManaRestante = jugador.ManaEnEsteTurno;
    }

	public void QuitarVidaJugador(JugadorPartida jugadorObjetivo,int valorAtaque)
    {
		new DealDamageCommand(jugadorObjetivo.ID, valorAtaque, jugadorObjetivo.Defensa).AñadirAlaCola();
        jugadorObjetivo.Defensa -= valorAtaque;
        if (JugadorMuerto(jugadorObjetivo))
            MuerteJugador(jugadorObjetivo);
    }

	public bool JugadorMuerto(JugadorPartida jugador)
    {
        return jugador.Defensa <= 0;
    }

    public void OcultarManoJugadorAnterior()
    {
        //Ocultamos la mano del jugador anterior 
		AreaJugador(OtroJugador(JugadorActual).Jugador).manoVisual.OcultarMano();
    }

    public void MostrarManoJugadorActual()
    {
        //Mostramos la mano del nuevo jugador actual
		AreaJugador(JugadorActual.Jugador).manoVisual.MostrarMano();
    }

	public void Clear(){
		instance = null;
	}

}
