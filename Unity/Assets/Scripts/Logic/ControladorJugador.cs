using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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
			areaJugadorActual = AreaJugador(value);
            _jugadorActual = value;
        }
    }
    #endregion

    private ControladorJugador()
    {
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

	public PlayerArea AreaJugador(JugadorPartida jugador)
    {
		if (null != _jugadorActual && jugador == _jugadorActual)
            return areaJugadorActual;
        PlayerArea areaJugador;
		switch (jugador.Area)
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
		PlayerArea areaJugador = AreaJugador(jugador);
        bool TurnoDelJugador = (_jugadorActual == jugador);
        bool NoCartasPendientesPorMostrar = !Comandas.Instance.ComandasDeDibujoCartaPendientes();
        return areaJugador.PermitirControlJugador && areaJugador.ControlActivado && TurnoDelJugador && NoCartasPendientesPorMostrar;
    }

	public void TransmitirInformacionVisualJugador(JugadorPartida jugador)
    {
		PlayerArea areaJugador = AreaJugador(jugador);
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
		PlayerArea areaJugador = AreaJugador(jugador);
        jugador.ManaEnEsteTurno = 0;
        jugador.ManaRestante = 0;
        TransmitirInformacionVisualJugador(jugador);
		areaJugador.mazoVisual.CartasEnMazo = jugador.Jugador.NumCartasMazo();
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
		if (jugador.Jugador.TipoJugador.Equals(Jugador.TIPO_JUGADOR.AUTOMÁTICO))
            new ShowMessageCommand("¡Turno enemigo!", 2.0f).AñadirAlaCola();
        else
            new ShowMessageCommand("¡Tu Turno!", 2.0f).AñadirAlaCola();
		Controlador.Instance.DibujarCartaMazo(jugador);
		if (jugador.GetType() == typeof(JugadorBot)) {
			((JugadorBot)jugador).EmpezarTurnoBot ();
		} else {
			ActualizarManaJugador(jugador);
			ActualizarEstadoCartasJugadorActual(jugador);
		}
		if (OtroJugador(jugador).GetType () == typeof(JugadorHumano)) {
			ActualizarEstadoCartasJugadorEnemigo(jugador);
		}
			
    }

	public void ActualizarVidaJugador(JugadorPartida jugador, int vidaActual){
		new UpdatePlayerHealthCommand(jugador,vidaActual).AñadirAlaCola();
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
            List<GameObject> listG = IDHolder.GetGameObjectsWithID(cl.ID);
			foreach(GameObject g in listG){
				if(g != null)
					g.GetComponent<OneCardManager>().PuedeSerJugada = Controlador.Instance.CartaPuedeUsarse(jugador,cl) && !quitarTodasRemarcadas;
			}
				
        }

        foreach (Ente crl in jugador.EntesEnLaMesa())
        {
			List<GameObject> listG = IDHolder.GetGameObjectsWithID(crl.ID);
			foreach(GameObject g in listG){
				if(g != null)
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
		Button salirPartida = GameObject.FindGameObjectWithTag ("SalirPartida").GetComponent<Button> ();
		salirPartida.interactable = false;
        Controlador.Instance.StopTheTimer();
		new MuerteJugadorCommand (jugador).AñadirAlaCola ();
		int exp = AñadirExperienciaJugador (jugador);	
		if (jugador.GetType() == typeof(JugadorHumano)) {
			new GameOverCommand (jugador,exp).AñadirAlaCola ();
			BaseDatos.Instance.ActualizarExperienciaBaseDatos ();
		}else {
			JugadorPartida ganador = OtroJugador (jugador);
			Carta carta = ObtenerCartaPremio ();
			List<Item> items = ObtenerItemsPremio ();
			AñadirPremioJugador (ganador,carta,items);
			new PremioPartidaCommand (jugador,carta,items,exp).AñadirAlaCola ();
			BaseDatos.Instance.ActualizarJugadorBaseDatos (carta != null);
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
			AreaJugador(player).ControlActivado = false;
        }
    }

    public bool CartaOCriaturaDelJugador(string tagCartaOCriatura)
    {
		return _jugadorActual.Area.ToString().Equals(tagCartaOCriatura.Substring(0, 3));
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
