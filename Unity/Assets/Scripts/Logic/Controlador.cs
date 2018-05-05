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

    // PRIVATE FIELDS
    // reference to a timer to measure 
    private RopeTimer timer;
    #endregion
    #region Getters/Setters
    // PROPERTIES
   
    public Jugador JugadorActual
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

    public Jugador Local
    {
        get
        {
            return BaseDatos.Instance.Local;
        }
    }

    public Jugador Enemigo
    {
        get
        {
            return BaseDatos.Instance.Enemigo;
        }
    }
    #endregion

    // METHODS
    void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Instance = this;
        timer = GetComponent<RopeTimer>();
        controladorJugador = ControladorJugador.Instance;
        controladorEnte = ControladorEnte.Instance;
        Recursos.CartasCreadasEnElJuego.Clear();
        
        //INICIALIZAR CONTROLS PLAYER,ENTE
    }

    void Start()
    {
        InicializacionJuego();
    }

    public void InicializacionJuego()
    {
        //Debug.Log("In TurnManager.OnGameStart()");

        foreach (Jugador p in BaseDatos.Instance.GetPlayers())
        {
            controladorJugador.InicializarValoresJugador(p);
            controladorJugador.ActualizarManaJugador(p);
        }

        Sequence s = DOTween.Sequence();
        //mueve los jugadores del centro a su posición
		PlayerArea areaJugador = controladorJugador.AreaJugador(BaseDatos.Instance.Local);
		PlayerArea areaJugador2 = controladorJugador.AreaJugador(BaseDatos.Instance.Enemigo);
        s.Append(areaJugador.Personaje.transform.DOMove(areaJugador.PosicionPersonaje.position, 1f).SetEase(Ease.InQuad));
        s.Insert(0f, areaJugador2.Personaje.transform.DOMove(areaJugador2.PosicionPersonaje.position, 1f).SetEase(Ease.InQuad));
        //espera 3 segundos antes de ejecutar el onComplete
        s.PrependInterval(3f);
        s.OnComplete(() =>
        {
            // determine who starts the game.
            //int rnd = Random.Range(0,2);  // 2 is exclusive boundary
            int rnd = 1;
            // Debug.Log(Player.Players.Length);
			Jugador whoGoesFirst = BaseDatos.Instance.Local;
            // Debug.Log(whoGoesFirst);
            Jugador whoGoesSecond = OtroJugador(whoGoesFirst);
            // Debug.Log(whoGoesSecond);
            // draw 4 cards for first player and 5 for second player
            // first player draws a card
            DibujarCartasMazo(whoGoesFirst, 4, true);
            // second player draws a card
            DibujarCartasMazo(whoGoesSecond, 4, true);
            // add one more card to second player`s hand
            // whoGoesSecond.DibujarCartaMazo(true);
            //new GivePlayerACoinCommand(null, whoGoesSecond).AddToQueue();
            // whoGoesSecond.DibujarCartaFueraMazo(CoinCard);
            new StartATurnCommand(whoGoesFirst).AñadirAlaCola();
        });
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //EndTurn();
    }
		
    public void EndTurn()
    {
		if (OpcionesObjeto.PrevisualizandoAlgunaCarta())
			OpcionesObjeto.PararTodasPrevisualizaciones();
        // stop timer
        timer.StopTimer();
        // send all commands in the end of current player`s turn
        JugadorActual.OnTurnEnd();

        new StartATurnCommand(OtroJugador(JugadorActual)).AñadirAlaCola();
    }

    public void StopTheTimer()
    {
        timer.StopTimer();
    }

    public void ActualizarValoresJugador()
    {
        timer.StartTimer();

        ActivarBotonFinDeTurno(JugadorActual);

        controladorJugador.ActualizarValoresJugador();
    }
    //TODO creo que falta añadir en que area se esta mirando
    public bool SePermiteControlarElJugador(Jugador ownerPlayer)
    {
        return controladorJugador.SePermiteControlarElJugador(ownerPlayer);
    }

    public void ActivarBotonFinDeTurno(Jugador P)
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
    public void DibujarCartasMazo(Jugador player, int numCartas, bool fast = false)
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
    public void DibujarCartaMazo(Jugador jugador, bool fast = false)
    {
        if (jugador.NumCartasMazo() > 0)
        {
            if (jugador.NumCartasMano() < 4)
            {
                //Carta newCard = new Carta(jugador.CartasEnElMazo()[0]);
				//Esto nos devuelve la carta actual del mazo que se recorre infinitamente
				Carta newCard = jugador.CartaActual();
                jugador.AñadirCartaMano(0, newCard);
                // Debug.Log(hand.CardsInHand.Count);
                new DrawACardCommand(jugador.CartasEnLaMano()[0], jugador, fast, fromDeck: true).AñadirAlaCola();
            }
        }

    }

    /// <summary>
    /// Jugar una carta fuera de la mano en el tablero
    /// </summary>
    /// <param name="jugador"></param>
    /// <param name="assetCarta"></param>
    public void DibujarCartaFueraMazo(Jugador jugador, CardAsset assetCarta)
    {
        if (jugador.NumCartasMano() < controladorJugador.AreaJugador(jugador).manoVisual.slots.Children.Length)
        {
            // 1) logic: add card to hand
            //ELIMINATE
            Carta newCard = new Carta(null);
            jugador.AñadirCartaMano(0, newCard);
            // 2) send message to the visual Deck
            new DrawACardCommand(jugador.CartasEnLaMano()[0], jugador, fast: true, fromDeck: false).AñadirAlaCola();
        }
        // no removal from deck because the card was not in the deck
    }

    /// <summary>
    /// Jugar una carta de la mano en el tablero mágica por su id
    /// </summary>
    /// <param name="UniqueID"></param>
    /// <param name="tablePos"></param>
    public void JugarMagicaMano(int UniqueID, int tablePos)
    {
		JugarMagicaMano(BaseDatos.Instance.Cartas[UniqueID], tablePos);
    }

    /// <summary>
    /// Jugar una carta de la mano en el tablero mágica
    /// </summary>
    /// <param name="magicaJugada"></param>
    /// <param name="tablePos"></param>
    public void JugarMagicaMano(Carta magicaJugada, int tablePos)
    {
        RestarManaCarta(JugadorActual, magicaJugada);
        Magica nuevaMagica = new Magica(magicaJugada.assetCarta);
        JugarCarta(magicaJugada, nuevaMagica, tablePos);
    }

    /// <summary>
    /// Jugar una carta de la mano en el tablero no mágica por su id
    /// </summary>
    /// <param name="UniqueID"></param>
    /// <param name="tablePos"></param>
    /// <param name="posicionAtaque"></param>
    public void JugarCartaMano(int UniqueID, int tablePos, bool posicionAtaque)
    {
        Debug.Log("Jugar carta mano: " + UniqueID);
		JugarCartaMano(BaseDatos.Instance.Cartas[UniqueID], tablePos, posicionAtaque);
    }

    /// <summary>
    /// Jugar una carta de la mano en el tablero no mágica
    /// </summary>
    /// <param name="cartaJugada"></param>
    /// <param name="tablePos"></param>
    /// <param name="posicionAtaque"></param>
    public void JugarCartaMano(Carta cartaJugada, int tablePos, bool posicionAtaque)
    {
        //ELIMINATE
        RestarManaCarta(JugadorActual, cartaJugada);
        Criatura newCreature = new Criatura(cartaJugada.assetCarta, posicionAtaque == true ? PosicionCriatura.ATAQUE : PosicionCriatura.DEFENSA);
        JugarCarta(cartaJugada,newCreature, tablePos);
        
    }

    /// <summary>
    /// Jugar carta 
    /// </summary>
    /// <param name="cartaJugada"></param>
    /// <param name="ente"></param>
    /// <param name="tablePos"></param>
    private void JugarCarta(Carta cartaJugada,Ente ente, int tablePos)
    {
        JugadorActual.AñadirEnteMesa(tablePos, ente);
        // no matter what happens, move this card to PlayACardSpot
        new PlayAEntityCommand(cartaJugada, JugadorActual, tablePos, ente).AñadirAlaCola();
        //causa battlecry effect
        if (ente.efecto != null)
            ente.efecto.WhenACreatureIsPlayed();
        // remove this card from hand
        JugadorActual.EliminarCartaMano(cartaJugada);
        //ELIMINATE o sobra esto
        //controladorJugador.ActualizarManaJugador(JugadorActual);
        //MostrarCartasJugablesJugadorActual();
    }

    public void ActualizarManaJugador(Jugador jugador)
    {
        controladorJugador.ActualizarManaJugador(jugador);
    }

    private void RestarManaCarta(Jugador jugador, Carta carta)
    {
        controladorJugador.RestarManaCarta(jugador, carta);
    }

    // Muestra cartas jugables de la mano del jugador
    public void MostrarCartasJugablesJugador(Jugador jugador)
    {
		controladorJugador.ActualizarEstadoCartasJugadorActual(jugador);
    }

    public PlayerArea AreaJugador(Jugador jugador)
    {
        return controladorJugador.AreaJugador(jugador);
    }

    public Jugador OtroJugador(Jugador jugador)
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

    /***************************************** CARTA ****************************************************/

    /*public bool PuedeSerJugada
    {
        get
        {
            bool nuestroTurno = (Controlador.Instance.jugadorActual == jugador);
            // for spells the amount of characters on the field does not matter
            bool fieldNotFull = true;
            // but if this is a creature, we have to check if there is room on board (table)
            //TODO Esto de momento sobrará porque todas las cartas ocuparan sitio en la mesa
            if (assetCarta.Defensa > 0)
                fieldNotFull = (jugador.NumCriaturasEnLaMesa() < DatosGenerales.Instance.NumMaximoCriaturasMesa);
            //Debug.Log("Card: " + ca.name + " has params: ownersTurn=" + ownersTurn + "fieldNotFull=" + fieldNotFull + " hasMana=" + (CurrentManaCost <= owner.ManaLeft));
            return nuestroTurno && fieldNotFull && (CosteManaActual <= jugador.ManaRestante);
        }
    }*/


    /***************************************** ENTE ****************************************************/

    // returns true if we can attack with this creature now
    /*public bool PuedeAtacar
    {
        get
        {
            bool nuestroTurno = (jugadorActual == jugador);
            return (nuestroTurno && (AtaquesRestantesEnTurno > 0) && !Frozen);
        }
    }*/

    public bool EstaEnPosicionAtaque(int idEnte)
    {
        return controladorEnte.EstaEnPosicionAtaque(idEnte);
    }

    public bool EsMagica(int idEnte)
    {
        return controladorEnte.EsMagica(idEnte);
    }

    public void AtacarCriatura(int idAtacante, int idObjetivo)
    {
        //TODO ver quien ataca, si magica o criatura
        Ente atacante = Recursos.EntesCreadosEnElJuego[idAtacante];
        Ente objetivo = Recursos.EntesCreadosEnElJuego[idObjetivo];
		if (atacante.GetType () == typeof(Criatura)) 
			((Criatura)atacante).HaAtacado = true;
			
        if(objetivo.GetType() == typeof(Criatura))
        {
            Criatura criaturaObjetivo = (Criatura)objetivo;
            controladorEnte.QuitarVidaEnte(atacante, criaturaObjetivo);
            if (criaturaObjetivo.PosicionCriatura.Equals(PosicionCriatura.ATAQUE))
                controladorJugador.QuitarVidaJugador(atacante.Ataque);
            else if (criaturaObjetivo.PosicionCriatura.Equals(PosicionCriatura.DEFENSA) && controladorEnte.EnteMuerto(criaturaObjetivo))
                controladorJugador.QuitarVidaJugador(objetivo.Defensa);
        }
        //En caso de una magica, activamos su efecto al atacarla
        else
        {
            //Criatura magicaObjetivo = (Magica)objetivo;
            Controlador.Instance.ActivarEfectoMagica(idObjetivo);
        }
        
        
        //else
        //controladorJugador.QuitarVidaJugador(objetivo.Defensa);
    }

    public void ActivarEfectoMagica(int idMagica)
    {
        controladorEnte.ActivarEfectoMagica(idMagica);
    }

    public void CambiarPosicionCriatura(int idCriatura)
    {
        controladorEnte.CambiarPosicionCriatura(idCriatura);
    }

    public void MostrarAccion(int idEnte)
    {
        controladorEnte.MostrarAccion(idEnte);
    }

}

