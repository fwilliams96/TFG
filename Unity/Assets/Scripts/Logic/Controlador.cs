using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.UI;


public enum PosicionCriatura { ATAQUE, DEFENSA };

// this class will take care of switching turns and counting down time until the turn expires
//
public class Controlador : MonoBehaviour
{

    #region Atributos
    // PUBLIC FIELDS
    // for Singleton Pattern
    public static Controlador Instance;
    private ControladorJugador controladorJugador;
    private ControladorEnte controladorEnte;
	private List<JugadorPartida> jugadores;

    // PRIVATE FIELDS
    // reference to a timer to measure 
    private RopeTimer timer;
    #endregion
    #region Getters/Setters
    // PROPERTIES
   
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

    public JugadorPartida Local
    {
        get
        {
            return jugadores[0];
        }
    }

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

	void IniciarMusica(){
		if (!ConfiguracionUsuario.Instance.Musica) {
			Camera.main.GetComponent<AudioSource> ().Pause ();
		}
	}

    void Start()
    {
        InicializacionJuego();
    }

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

    public void StopTheTimer()
    {
        timer.StopTimer();
    }

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
                //Carta newCard = new Carta(jugador.CartasEnElMazo()[0]);
				//Esto nos devuelve la carta actual del mazo que se recorre infinitamente
				Carta newCard = (Carta)jugador.CartaActual();
				//TODO ver si ya existe la carta en baraja, si existe volver a crear una instancia para cambiar el id
				/*if (jugador.ContieneCarta (newCard)) {
					newCard = newCard.
				}*/
                jugador.AñadirCartaMano(0, newCard);
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
		JugarMagicaMano(jugador,BaseDatos.Instance.Cartas[UniqueID], tablePos);
    }

    /// <summary>
    /// Jugar una carta de la mano en el tablero mágica
    /// </summary>
    /// <param name="magicaJugada"></param>
    /// <param name="tablePos"></param>
	public void JugarMagicaMano(JugadorPartida jugador,Carta magicaJugada, int tablePos)
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
		JugarCartaMano(jugador,BaseDatos.Instance.Cartas[UniqueID], tablePos, posicionAtaque);
    }

    /// <summary>
    /// Jugar una carta de la mano en el tablero no mágica
    /// </summary>
    /// <param name="cartaJugada"></param>
    /// <param name="tablePos"></param>
    /// <param name="posicionAtaque"></param>
	public void JugarCartaMano(JugadorPartida jugador,Carta cartaJugada, int tablePos, bool posicionAtaque)
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
	private void JugarCarta(JugadorPartida jugador,Carta cartaJugada,Ente ente, int tablePos)
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

	private void RestarManaCarta(JugadorPartida jugador, Carta carta)
    {
        controladorJugador.RestarManaCarta(jugador, carta);
    }

    /// <summary>
	/// Muestra cartas jugables de la mano del jugador
    /// </summary>
    /// <param name="jugador">Jugador.</param>
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

	public  void GiveHealth(JugadorPartida jugador, int vida){
		jugador.Defensa += vida;
		if(jugador.GetType() == typeof(JugadorHumano))
			new ShowMessageCommand ("¡Obtienes "+vida+" de vida!", 1.0f).AñadirAlaCola ();
		ActualizarVidaJugador (jugador, jugador.Defensa - vida);
	}
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

	public bool CartaPuedeUsarse(JugadorPartida jugador,Carta carta){
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

	public JugadorPartida ObtenerDueñoEnte(Ente ente){

		JugadorPartida jugador = Controlador.Instance.Local.Area.Equals (ente.Area) ? Controlador.Instance.Local : Controlador.Instance.Enemigo;

		return jugador;
	}	

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

	public void AtacarJugador(int idCriaturaAtacante, int idJugadorObjetivo){
		Criatura atacante = (Criatura)Recursos.EntesCreadosEnElJuego[idCriaturaAtacante];
		JugadorPartida jugador = Controlador.Instance.Local.ID == idJugadorObjetivo ? Controlador.Instance.Local : Controlador.Instance.Enemigo;
		atacante.HaAtacado = true;
		controladorJugador.AtacarJugador (atacante, jugador);
		
	}

	public void AtacarCriatura(Criatura criaturaAtacante, Criatura criaturaObjetivo){
		JugadorPartida objetivo = ObtenerDueñoEnte (criaturaObjetivo);
		controladorEnte.AtacarCriatura(criaturaAtacante, criaturaObjetivo);
		if (criaturaObjetivo.PosicionCriatura.Equals (PosicionCriatura.ATAQUE)) {
			controladorJugador.QuitarVidaJugador (objetivo, criaturaAtacante.Ataque);
			controladorEnte.QuitarVidaCriatura (criaturaAtacante, criaturaObjetivo.Ataque);
		} else if (criaturaObjetivo.PosicionCriatura.Equals (PosicionCriatura.DEFENSA) && controladorEnte.CriaturaMuerta (criaturaObjetivo))
			controladorJugador.QuitarVidaJugador (objetivo, System.Math.Abs (criaturaObjetivo.Defensa));
	}

	public void AtacarMagica(Criatura criaturaAtacante, Magica magicaObjetivo){
		JugadorPartida objetivo = ObtenerDueñoEnte (magicaObjetivo);
		controladorEnte.AtacarMagica(criaturaAtacante, magicaObjetivo);
		controladorJugador.QuitarVidaJugador(objetivo,criaturaAtacante.Ataque);
	}

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

