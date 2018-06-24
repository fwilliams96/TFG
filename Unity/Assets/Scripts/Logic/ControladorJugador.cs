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

	/// <summary>
	/// Determina el area visual a partir del jugador.
	/// </summary>
	/// <returns>The jugador.</returns>
	/// <param name="jugador">Jugador.</param>
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

	/// <summary>
	/// Mira si el jugador tiene el turno, si no hay cartas pendientes por mostrar y si tiene el control activado.
	/// </summary>
	/// <returns><c>true</c>, if permite controlar el jugador was sed, <c>false</c> otherwise.</returns>
	/// <param name="jugador">Jugador.</param>
	public bool SePermiteControlarElJugador(JugadorPartida jugador)
    {
		PlayerArea areaJugador = AreaJugador(jugador);
        bool TurnoDelJugador = (_jugadorActual == jugador);
        bool NoCartasPendientesPorMostrar = !Comandas.Instance.ComandasDeDibujoCartaPendientes();
        return areaJugador.PermitirControlJugador && areaJugador.ControlActivado && TurnoDelJugador && NoCartasPendientesPorMostrar;
    }

	/// <summary>
	/// Determina si el control tiene control.
	/// </summary>
	/// <param name="jugador">Jugador.</param>
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

	/// <summary>
	/// Inicializars los valores del jugador.
	/// </summary>
	/// <param name="jugador">Jugador.</param>
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

	/// <summary>
	/// Deshabilita el mana.
	/// </summary>
	/// <param name="jugador">Jugador.</param>
	public void DeshabilitarMana(JugadorPartida jugador){
		new DeshabilitarManaCommand (jugador).AñadirAlaCola ();
	}

	public void ActualizarValoresJugador(JugadorPartida jugador)
    {
		OnTurnStart(jugador);
    }

	/// <summary>
	/// Inicia el turno del jugador, muestra el mensaje de turno y actualiza el mana y las cartas.
	/// </summary>
	/// <param name="jugador">Jugador.</param>
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

	/// <summary>
	/// Muestra cartas jugables de la mano del jugador
	/// </summary>
	/// <param name="jugador">Jugador.</param>
	public void ActualizarEstadoCartasJugadorActual(JugadorPartida jugador)
    {
        if (jugador == JugadorActual)
            ActualizarEstadoCartasJugador(jugador, false);

    }

	/// <summary>
	/// Oculta las cartas del jugador que no tiene el turno.
	/// </summary>
	/// <param name="jugador">Jugador.</param>
	private void ActualizarEstadoCartasJugadorEnemigo(JugadorPartida jugador)
    {
        if (jugador == JugadorActual)
            ActualizarEstadoCartasJugador(OtroJugador(jugador), true);
            }

	/// <summary>
	/// Oculta o muestra la jugabilidad de las cartas en función del segundo parametro.
	/// </summary>
	/// <param name="jugador">Jugador.</param>
	/// <param name="quitarTodasRemarcadas">If set to <c>true</c> quitar todas remarcadas.</param>
	private void ActualizarEstadoCartasJugador(JugadorPartida jugador, bool quitarTodasRemarcadas = false)
    {
		foreach (CartaPartida cl in jugador.CartasEnLaMano())
        {
			GameObject g = IDHolder.GetGameObjectWithID (cl.ID);
			if(g != null)
				g.GetComponent<OneCardManager>().PuedeSerJugada = Controlador.Instance.CartaPuedeUsarse(jugador,cl) && !quitarTodasRemarcadas;
				
        }

        foreach (Ente crl in jugador.EntesEnLaMesa())
        {
			GameObject g = IDHolder.GetGameObjectWithID (crl.ID);
			if(g != null)
				g.GetComponent<OneEnteManager>().PuedeAtacar = Controlador.Instance.EntePuedeUsarse(crl) && !quitarTodasRemarcadas;
                
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
			BaseDatos.Instance.ActualizarNivelYExperienciaBaseDatos ();
		}else {
			JugadorPartida ganador = OtroJugador (jugador);
			Carta carta = ObtenerCartaPremio ();
			List<Item> items = ObtenerItemsPremio ();
			AñadirPremioJugador (ganador,carta,items);
			new PremioPartidaCommand (jugador,carta,items,exp).AñadirAlaCola ();
			BaseDatos.Instance.ActualizarJugadorBaseDatos (carta != null);
		}
			
    }

	/// <summary>
	/// Genera una carta de premio al jugador.
	/// </summary>
	/// <returns>The carta premio.</returns>
	private Carta ObtenerCartaPremio(){
		
		Carta carta = null;
		System.Random rnd = new System.Random ();
		if(rnd.Next(0,2) == 0)
			carta = BaseDatos.Instance.GenerarCartasAleatorias (1)[0];
		return carta;
	}

	/// <summary>
	/// Genera items de premio al jugador.
	/// </summary>
	/// <returns>The items premio.</returns>
	private List<Item> ObtenerItemsPremio(){

		List<Item> items = BaseDatos.Instance.GenerarItemsAleatorios (3);
		return items;
	}

	/// <summary>
	/// Añade al jugador los premios obtenidos.
	/// </summary>
	/// <param name="jugador">Jugador.</param>
	/// <param name="carta">Carta.</param>
	/// <param name="items">Items.</param>
	private void AñadirPremioJugador(JugadorPartida jugador,Carta carta, List<Item> items){
		if (carta != null)
			jugador.Jugador.AñadirCarta (carta);
		foreach (Item item in items) {
			jugador.Jugador.AñadirItem (item);
		}
	}

	/// <summary>
	/// Añade la experiencia obtenida al jugador, según el tipo de configuración da mas o menos.
	/// </summary>
	/// <returns>The experiencia jugador.</returns>
	/// <param name="jugador">Jugador.</param>
	private int AñadirExperienciaJugador(JugadorPartida jugador){
		ConfiguracionUsuario settings = ConfiguracionUsuario.Instance;
		int min = 0;
		int max = 0;
		if (settings.ConfiguracionBatalla.Equals (ConfiguracionUsuario.TIPO_CONFIGURACION.PORCENTAJE)) {
			min = 50;
			max = 90;
		} else {
			min = 10;
			max = 50;
		}
		int exp = Random.Range (min, max);
		jugador.Jugador.Experiencia += exp; 
		if (jugador.Jugador.Experiencia >= 100) {
			jugador.Jugador.Experiencia -= 100;
			jugador.Jugador.Nivel += 1;
		}
		return exp;
	}

	/// <summary>
	/// Para el control de todos los jugadores.
	/// </summary>
    public void PararControlJugadores()
    {
		foreach (JugadorPartida player in Controlador.Instance.Jugadores)
        {
			AreaJugador(player).ControlActivado = false;
        }
    }

	/// <summary>
	/// Determina si un ente o carta es del jugador actual.
	/// </summary>
	/// <returns><c>true</c>, if O criatura del jugador was cartaed, <c>false</c> otherwise.</returns>
	/// <param name="tagCartaOCriatura">Tag carta O criatura.</param>
    public bool CartaOCriaturaDelJugador(string tagCartaOCriatura)
    {
		return _jugadorActual.Area.ToString().Equals(tagCartaOCriatura.Substring(0, 3));
    }

	/// <summary>
	/// Indica si se puede atacar de cara al jugador.
	/// </summary>
	/// <returns><c>true</c>, if puede atacar jugador de cara was sed, <c>false</c> otherwise.</returns>
	/// <param name="idJugador">Identifier jugador.</param>
	public bool SePuedeAtacarJugadorDeCara(int idJugador){
		JugadorPartida jugador = Controlador.Instance.Local.ID == idJugador ? Controlador.Instance.Local : Controlador.Instance.Enemigo;
		return jugador.NumEntesEnLaMesa () == 0;
	}

	/// <summary>
	/// Permite atacar de cara al jugador.
	/// </summary>
	/// <param name="atacante">Atacante.</param>
	/// <param name="jugadorObjetivo">Jugador objetivo.</param>
	public void AtacarJugador(Criatura atacante,  JugadorPartida jugadorObjetivo)
	{
		atacante.AtaquesRestantesEnTurno--;
		new CreatureAttackCommand(jugadorObjetivo.ID, atacante.ID, atacante.Ataque,jugadorObjetivo.Defensa).AñadirAlaCola();
		jugadorObjetivo.Defensa -= atacante.Ataque;
		if(JugadorMuerto(jugadorObjetivo))
			MuerteJugador(jugadorObjetivo);
	}

	/// <summary>
	/// Devuelve el jugador contrario al del parametro.
	/// </summary>
	/// <returns>The jugador.</returns>
	/// <param name="jugador">Jugador.</param>
	public JugadorPartida OtroJugador(JugadorPartida jugador)
    {
		return Controlador.Instance.Local == jugador ? Controlador.Instance.Enemigo : Controlador.Instance.Local;
    }

    /// <summary>
    /// Resta mana al jugador según la carta lanzada al tablero
    /// </summary>
    /// <param name="jugador"></param>
    /// <param name="carta"></param>
	public void RestarManaCarta(JugadorPartida jugador, CartaPartida carta)
    {
		jugador.ManaRestante -= carta.CosteManaActual;
    }

	/// <summary>
	/// Quita vida al jugador sin un ataque visual.
	/// </summary>
	/// <param name="jugadorObjetivo">Jugador objetivo.</param>
	/// <param name="valorAtaque">Valor ataque.</param>
	public void QuitarVidaJugador(JugadorPartida jugadorObjetivo,int valorAtaque)
    {
		new DealDamageCommand(jugadorObjetivo.ID, valorAtaque, jugadorObjetivo.Defensa).AñadirAlaCola();
        jugadorObjetivo.Defensa -= valorAtaque;
        if (JugadorMuerto(jugadorObjetivo))
            MuerteJugador(jugadorObjetivo);
    }

	/// <summary>
	/// Indica si el jugador ha muerto.
	/// </summary>
	/// <returns><c>true</c>, if muerto was jugadored, <c>false</c> otherwise.</returns>
	/// <param name="jugador">Jugador.</param>
	public bool JugadorMuerto(JugadorPartida jugador)
    {
        return jugador.Defensa <= 0;
    }

	/// <summary>
	/// Oculta la mano del jugador que no tiene el turno.
	/// </summary>
    public void OcultarManoJugadorAnterior()
    {
        //Ocultamos la mano del jugador anterior 
		AreaJugador(OtroJugador(JugadorActual)).manoVisual.OcultarMano();
    }

	/// <summary>
	/// Muestra la mano del jugador actual.
	/// </summary>
    public void MostrarManoJugadorActual()
    {
        //Mostramos la mano del nuevo jugador actual
		AreaJugador(JugadorActual).manoVisual.MostrarMano();
    }

	/// <summary>
	/// Elimina la instancia
	/// </summary>
	public void Clear(){
		instance = null;
	}

}
