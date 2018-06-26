using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.UI;

// this class will take care of switching turns and counting down time until the turn expires
//
public class Controlador : MonoBehaviour
{

    #region Atributos
    public static Controlador Instance;
    private ControladorJugador controladorJugador;
    private ControladorEnte controladorEnte;
	private List<JugadorPartida> jugadores;
    private RopeTimer timer;
    #endregion
    #region Getters/Setters

	//Jugador actual de la partida.
    public JugadorPartida JugadorActual
    {
        get
        {
            return controladorJugador.JugadorActual;
        }

        set
        {
            controladorJugador.JugadorActual = value;
        }
    }

	//Jugador que corresponde al usuario del juego.
    public JugadorPartida Local
    {
        get
        {
            return jugadores[0];
        }
    }

	//Jugador que corresponde al contrincante bot del juego.
	public JugadorPartida Enemigo
    {
        get
        {
			return jugadores[1];
        }
    }

	public List<JugadorPartida> Jugadores{
		get{
			return jugadores;
		}
	}


    #endregion

    // METHODS
    void Awake()
    {
        Instance = this;
		IniciarMusica ();
		BaseDatos.Instance.CrearJugadorEnemigo ();
        timer = GetComponent<RopeTimer>();
        controladorJugador = ControladorJugador.Instance;
        controladorEnte = ControladorEnte.Instance;
        Recursos.CartasCreadasEnElJuego.Clear();

        //INICIALIZAR CONTROLS PLAYER,ENTE
    }

	/// <summary>
	/// Inicializa la música de la batalla en función de la configuración.
	/// </summary>
	void IniciarMusica(){
		if (!ConfiguracionUsuario.Instance.Musica) {
			Camera.main.GetComponent<AudioSource> ().Pause ();
		}
	}

    void Start()
    {
        InicializacionJuego();
    }

	/// <summary>
	/// Se inicia la partida, inicializando los valores de cada jugador, repartiendo cartas y empezando el primer turno.
	/// </summary>
    public void InicializacionJuego()
    {
        //Debug.Log("In TurnManager.OnGameStart()");
		jugadores = new List<JugadorPartida>();
		JugadorPartida jugadorPartida;
        foreach (Jugador p in BaseDatos.Instance.GetPlayers())
        {
			if (p.TipoJugador.Equals (Jugador.TIPO_JUGADOR.MANUAL)) {
				jugadorPartida = JugadorHumano.Instance;
				jugadorPartida.Jugador = p;
				//jugadorPartida = new JugadorHumano (p);
				controladorJugador.InicializarValoresJugador (jugadorPartida);
				controladorJugador.ActualizarManaJugador (jugadorPartida);
			} else {
				jugadorPartida = JugadorBot.Instance;
				jugadorPartida.Jugador = p;
				//jugadorPartida = new JugadorBot (p);
				controladorJugador.InicializarValoresJugador (jugadorPartida);
				controladorJugador.DeshabilitarMana (jugadorPartida);
			}
			jugadores.Add (jugadorPartida);
        }

        Sequence s = DOTween.Sequence();
        //mueve los jugadores del centro a su posición
		PlayerArea areaJugador = controladorJugador.AreaJugador(jugadores[0]);
		PlayerArea areaJugador2 = controladorJugador.AreaJugador(jugadores[1]);
        s.Append(areaJugador.Personaje.transform.DOMove(areaJugador.PosicionPersonaje.position, 1f).SetEase(Ease.InQuad));
        s.Insert(0f, areaJugador2.Personaje.transform.DOMove(areaJugador2.PosicionPersonaje.position, 1f).SetEase(Ease.InQuad));
        //espera 3 segundos antes de ejecutar el onComplete
        s.PrependInterval(3f);
        s.OnComplete(() =>
        {
            int rnd = Random.Range(0,2);  
			JugadorPartida whoGoesFirst = jugadores[rnd];
			JugadorPartida whoGoesSecond = OtroJugador(whoGoesFirst);
            DibujarCartasMazo(whoGoesFirst, 4, true);
            DibujarCartasMazo(whoGoesSecond, 4, true);
            new StartATurnCommand(whoGoesFirst).AñadirAlaCola();
        });
    }

	/// <summary>
	/// Función que se llama al terminarse un turno, cierra los menus abiertos, 
	/// para el temporizador y el control del jugador y inicia un nuevo turno.
	/// </summary>
    public void EndTurn()
    {
		if (Comandas.Instance.ComandasDeCambioTurnoPendientes ())
			return;
		if (OpcionesObjeto.PrevisualizandoAlgunaCarta())
			OpcionesObjeto.PararTodasPrevisualizaciones();
		if (AccionesPopUp.Instance.EstaActivo())
			AccionesPopUp.Instance.OcultarPopup ();
		if(PosicionCriaturaPopUp.Instance.EstaActivo())
			PosicionCriaturaPopUp.Instance.PosicionCriaturaElegida (-1);
        timer.StopTimer();
        JugadorActual.OnTurnEnd();
		if(AreaJugador(JugadorActual).ControlActivado)
        	new StartATurnCommand(OtroJugador(JugadorActual)).AñadirAlaCola();
    }

	/// <summary>
	/// Stops the timer.
	/// </summary>
    public void StopTheTimer()
    {
        timer.StopTimer();
    }

	/// <summary>
	/// Actualiza los valores del jugador al iniciarse un turno.
	/// </summary>
	/// <param name="jugador">Jugador.</param>
	public void ActualizarValoresJugador(JugadorPartida jugador)
    {
		timer.StartTimer();

		ActivarBotonFinDeTurno(jugador);

		controladorJugador.ActualizarValoresJugador(jugador);
    }

	public bool SePermiteControlarElJugador(JugadorPartida ownerPlayer)
    {
        return controladorJugador.SePermiteControlarElJugador(ownerPlayer);
    }

	/// <summary>
	/// Habilita o deshabilita el botón de fin de turno en función del jugador.
	/// </summary>
	/// <param name="P">P.</param>
	public void ActivarBotonFinDeTurno(JugadorPartida P)
    {
		
		if (SePermiteControlarElJugador (P))
			GameObject.FindGameObjectWithTag ("BotonFinTurno").GetComponent<Button> ().interactable = true;
		else
			GameObject.FindGameObjectWithTag ("BotonFinTurno").GetComponent<Button> ().interactable = false;

    }

    /***************************************** PLAYER ****************************************************/

    /// <summary>
    /// Quita numCartas del mazo y las inserta en la mano
    /// </summary>
    /// <param name="player"></param>
    /// <param name="numCartas"></param>
    /// <param name="fast"></param>
	public void DibujarCartasMazo(JugadorPartida player, int numCartas, bool fast = false)
    {
        for (int i = 0; i < numCartas; i++)
        {
            DibujarCartaMazo(player, fast);
        }
    }


    /// <summary>
    /// Quita una carta del mazo y la muestra en la mano
    /// </summary>
    /// <param name="jugador"></param>
    /// <param name="fast"></param>
	public void DibujarCartaMazo(JugadorPartida jugador, bool fast = false)
    {
		if (jugador.Jugador.NumCartasMazo() > 0)
        {
            if (jugador.NumCartasMano() < 4)
            {
				//Esto nos devuelve la carta actual del mazo que se recorre infinitamente
				Carta newCard = (Carta)jugador.CartaActual();
				CartaPartida carta = new CartaPartida (newCard.AssetCarta);
				jugador.AñadirCartaMano(0, carta);
                // Debug.Log(hand.CardsInHand.Count);
                new DrawACardCommand(jugador.CartasEnLaMano()[0], jugador, fast, fromDeck: true).AñadirAlaCola();
            }
        }

    }

    /// <summary>
    /// Jugar una carta de la mano en el tablero mágica por su id
    /// </summary>
    /// <param name="UniqueID"></param>
    /// <param name="tablePos"></param>
    public void JugarMagicaMano(JugadorPartida jugador,int UniqueID, int tablePos)
    {
		JugarMagicaMano(jugador,Recursos.CartasCreadasEnElJuego[UniqueID], tablePos);
    }

    /// <summary>
    /// Jugar una carta de la mano en el tablero mágica
    /// </summary>
    /// <param name="magicaJugada"></param>
    /// <param name="tablePos"></param>
	public void JugarMagicaMano(JugadorPartida jugador,CartaPartida magicaJugada, int tablePos)
    {
		RestarManaCarta(jugador, magicaJugada);
		Magica nuevaMagica = new Magica(jugador.Area,magicaJugada.AssetCarta);
		JugarCarta(jugador,magicaJugada, nuevaMagica, tablePos);
    }

    /// <summary>
    /// Jugar una carta de la mano en el tablero no mágica por su id
    /// </summary>
    /// <param name="UniqueID"></param>
    /// <param name="tablePos"></param>
    /// <param name="posicionAtaque"></param>
	public void JugarCartaMano(JugadorPartida jugador,int UniqueID, int tablePos, bool posicionAtaque)
    {
        Debug.Log("Jugar carta mano: " + UniqueID);
		JugarCartaMano(jugador,Recursos.CartasCreadasEnElJuego[UniqueID], tablePos, posicionAtaque);
    }

    /// <summary>
    /// Jugar una carta de la mano en el tablero no mágica
    /// </summary>
    /// <param name="cartaJugada"></param>
    /// <param name="tablePos"></param>
    /// <param name="posicionAtaque"></param>
	public void JugarCartaMano(JugadorPartida jugador,CartaPartida cartaJugada, int tablePos, bool posicionAtaque)
    {
        //ELIMINATE
		RestarManaCarta(jugador, cartaJugada);
		Criatura newCreature = new Criatura(jugador.Area,cartaJugada.AssetCarta, posicionAtaque == true ? PosicionCriatura.ATAQUE : PosicionCriatura.DEFENSA);
        JugarCarta(jugador,cartaJugada,newCreature, tablePos);
        
    }

    /// <summary>
    /// Jugar carta 
    /// </summary>
    /// <param name="cartaJugada"></param>
    /// <param name="ente"></param>
    /// <param name="tablePos"></param>
	private void JugarCarta(JugadorPartida jugador,CartaPartida cartaJugada,Ente ente, int tablePos)
    {
		jugador.AñadirEnteMesa(tablePos, ente);
        // no matter what happens, move this card to PlayACardSpot
		new PlayAEntityCommand(cartaJugada, jugador, tablePos, ente).AñadirAlaCola();
		if (jugador.GetType () == typeof(JugadorHumano)) {
			ActualizarManaJugador(jugador);
			MostrarCartasJugablesJugador(jugador);
		}
		jugador.EliminarCartaMano(cartaJugada);
		if(jugador.GetType() == typeof(JugadorHumano))
			MostrarCartasJugablesJugador(jugador);

    }

	public void ActualizarVidaJugador(JugadorPartida jugador, int vidaActual){
		controladorJugador.ActualizarVidaJugador(jugador,vidaActual);
	}

	public void ActualizarManaJugador(JugadorPartida jugador)
    {
        controladorJugador.ActualizarManaJugador(jugador);
    }

	private void RestarManaCarta(JugadorPartida jugador, CartaPartida carta)
    {
        controladorJugador.RestarManaCarta(jugador, carta);
    }
		
    public void MostrarCartasJugablesJugador(JugadorPartida jugador)
    {
		controladorJugador.ActualizarEstadoCartasJugadorActual(jugador);
    }

	public PlayerArea AreaJugador(JugadorPartida jugador)
    {
		return controladorJugador.AreaJugador(jugador);
    }

	public JugadorPartida OtroJugador(JugadorPartida jugador)
    {
        return controladorJugador.OtroJugador(jugador);
    }

    public bool CartaOCriaturaDelJugador(string tagCartaOCriatura)
    {
        return controladorJugador.CartaOCriaturaDelJugador(tagCartaOCriatura);
    }

    public void OcultarManoJugadorAnterior()
    {
        controladorJugador.OcultarManoJugadorAnterior();
    }

    public void MostrarManoJugadorActual()
    {
        controladorJugador.MostrarManoJugadorActual();
    }

	public bool SePuedeAtacarJugadorDeCara(int idJugador){
		return controladorJugador.SePuedeAtacarJugadorDeCara (idJugador);
	}

	/// <summary>
	/// Permite añadir vida adicinal al jugador.
	/// </summary>
	/// <param name="jugador">Jugador.</param>
	/// <param name="vida">Vida.</param>
	public  void GiveHealth(JugadorPartida jugador, int vida){
		jugador.Defensa += vida;
		if(jugador.GetType() == typeof(JugadorHumano))
			new ShowMessageCommand ("¡Obtienes "+vida+" de vida!", 1.0f).AñadirAlaCola ();
		ActualizarVidaJugador (jugador, jugador.Defensa - vida);
	}

	/// <summary>
	/// Permite añadir mana adicinal al jugador.
	/// </summary>
	/// <param name="jugador">Jugador.</param>
	/// <param name="mana">Mana.</param>
	public void GiveManaBonus(JugadorPartida jugador, int mana)
	{
		jugador.ConseguirManaExtra(mana);
		if (jugador.GetType () == typeof(JugadorHumano)) {
			new ShowMessageCommand ("¡Obtienes " + mana + " de maná!", 1.0f).AñadirAlaCola ();
			ActualizarManaJugador (jugador);
			MostrarCartasJugablesJugador(jugador);
		}
	}

    /***************************************** CARTA ****************************************************/

	public bool CartaPuedeUsarse(JugadorPartida jugador,CartaPartida carta){
		return carta.CosteManaActual <= jugador.ManaRestante;
	}

    /***************************************** ENTE ****************************************************/
	public bool CriaturaHaAtacado(Criatura criatura){
		return controladorEnte.CriaturaHaAtacado (criatura);
	}

	public bool EsMagicaTrampa(Ente ente){
		return controladorEnte.EsMagicaTrampa (ente);
	}

	public bool EntePuedeUsarse(Ente ente){
		return controladorEnte.EntePreparado (ente);
	}

    public bool EstaEnPosicionAtaque(int idEnte)
    {
        return controladorEnte.EstaEnPosicionAtaque(idEnte);
    }

    public bool EsMagica(int idEnte)
    {
        return controladorEnte.EsMagica(idEnte);
    }

	/// <summary>
	/// Determina el jugador dueño del ente.
	/// </summary>
	/// <returns>The dueño ente.</returns>
	/// <param name="ente">Ente.</param>
	public JugadorPartida ObtenerDueñoEnte(Ente ente){

		JugadorPartida jugador = Controlador.Instance.Local.Area.Equals (ente.Area) ? Controlador.Instance.Local : Controlador.Instance.Enemigo;

		return jugador;
	}	

	/// <summary>
	/// Permite dañar una criatura sin mostrar un ataque visual, en caso de que se encuentre en defensa y 
	/// muera se quita vida al jugador, en ataque siempre se quita vida al jugador.
	/// </summary>
	/// <param name="criaturaObjetivo">Criatura objetivo.</param>
	/// <param name="daño">Daño.</param>
	public void DañarCriatura(Criatura criaturaObjetivo,int daño){
		JugadorPartida objetivo = ObtenerDueñoEnte (criaturaObjetivo);
		controladorEnte.QuitarVidaCriatura(criaturaObjetivo,daño);
		if (criaturaObjetivo.PosicionCriatura.Equals(PosicionCriatura.ATAQUE))
			controladorJugador.QuitarVidaJugador(objetivo,daño);
		else if (criaturaObjetivo.PosicionCriatura.Equals(PosicionCriatura.DEFENSA) && controladorEnte.CriaturaMuerta(criaturaObjetivo))
			controladorJugador.QuitarVidaJugador(objetivo,criaturaObjetivo.Defensa);
	}
		
	public void DañarCriatura(Criatura criaturaAtacante, Criatura criaturaObjetivo){
		DañarCriatura (criaturaObjetivo,criaturaAtacante.Ataque);
	}

	/// <summary>
	/// Función que permite que la criatura ataque al jugador.
	/// </summary>
	/// <param name="idCriaturaAtacante">Identifier criatura atacante.</param>
	/// <param name="idJugadorObjetivo">Identifier jugador objetivo.</param>
	public void AtacarJugador(int idCriaturaAtacante, int idJugadorObjetivo){
		Criatura atacante = (Criatura)Recursos.EntesCreadosEnElJuego[idCriaturaAtacante];
		JugadorPartida jugador = Controlador.Instance.Local.ID == idJugadorObjetivo ? Controlador.Instance.Local : Controlador.Instance.Enemigo;
		atacante.HaAtacado = true;
		controladorJugador.AtacarJugador (atacante, jugador);	
	}

	/// <summary>
	/// Permite atacar a una criatura, mostrándose la animación de ataque.
	/// </summary>
	/// <param name="criaturaAtacante">Criatura atacante.</param>
	/// <param name="criaturaObjetivo">Criatura objetivo.</param>
	public void AtacarCriatura(Criatura criaturaAtacante, Criatura criaturaObjetivo){
		JugadorPartida objetivo = ObtenerDueñoEnte (criaturaObjetivo);
		controladorEnte.AtacarCriatura(criaturaAtacante, criaturaObjetivo);
		if (criaturaObjetivo.PosicionCriatura.Equals (PosicionCriatura.ATAQUE)) {
			controladorJugador.QuitarVidaJugador (objetivo, criaturaAtacante.Ataque);
			controladorEnte.QuitarVidaCriatura (criaturaAtacante, criaturaObjetivo.Ataque);
		} else if (criaturaObjetivo.PosicionCriatura.Equals (PosicionCriatura.DEFENSA) && controladorEnte.CriaturaMuerta (criaturaObjetivo))
			controladorJugador.QuitarVidaJugador (objetivo, System.Math.Abs (criaturaObjetivo.Defensa));
	}

	/// <summary>
	/// Función que se llama cuando se ataca a un ente mágico.
	/// </summary>
	/// <param name="criaturaAtacante">Criatura atacante.</param>
	/// <param name="magicaObjetivo">Magica objetivo.</param>
	public void AtacarMagica(Criatura criaturaAtacante, Magica magicaObjetivo){
		JugadorPartida objetivo = ObtenerDueñoEnte (magicaObjetivo);
		controladorEnte.AtacarMagica(criaturaAtacante, magicaObjetivo);
		controladorJugador.QuitarVidaJugador(objetivo,criaturaAtacante.Ataque);
	}

	/// <summary>
	/// Función que permite atacar a un ente.
	/// </summary>
	/// <param name="idAtacante">Identifier atacante.</param>
	/// <param name="idObjetivo">Identifier objetivo.</param>
	public void AtacarEnte(int idAtacante, int idObjetivo)
    {
        Ente atacante = Recursos.EntesCreadosEnElJuego[idAtacante];
        Ente objetivo = Recursos.EntesCreadosEnElJuego[idObjetivo];
		if (atacante.GetType () == typeof(Criatura)) 
			((Criatura)atacante).HaAtacado = true;
			
        if(objetivo.GetType() == typeof(Criatura))
        {
			Criatura criaturaObjetivo = (Criatura)objetivo;
			AtacarCriatura ((Criatura)atacante, criaturaObjetivo);
        }
        else
        {
			ActivarEfectoMagica (idObjetivo, idAtacante);
        }

    }

	public void ActivarEfectoMagica(int idMagica){
		controladorEnte.ActivarEfectoMagica(idMagica,-1);
	}

	public void ActivarEfectoMagica(int idMagica, int idAtacante)
    {
		controladorEnte.ActivarEfectoMagica(idMagica,idAtacante);
    }

    public void CambiarPosicionCriatura(int idCriatura)
    {
        controladorEnte.CambiarPosicionCriatura(idCriatura);
    }

    public void MostrarAccion(int idEnte)
    {
        controladorEnte.MostrarAccion(idEnte);
    }

	/// <summary>
	/// Vacía los datos de la partida.
	/// </summary>
	public void Clear(){
		BaseDatos.Instance.Clear ();
		jugadores.Clear ();
		controladorJugador.Clear ();
		controladorEnte.Clear ();
	}

	public void MuerteEnte(int idEnte){
		controladorEnte.MuerteEnte (idEnte);
	}

}

