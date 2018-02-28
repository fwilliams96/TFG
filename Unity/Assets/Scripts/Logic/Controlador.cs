using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using UnityEditor;
using System;
using System.Reflection;
public enum PosicionCriatura { ATAQUE, DEFENSA };

// this class will take care of switching turns and counting down time until the turn expires
//
public class Controlador : MonoBehaviour
{

    #region Atributos
    // PUBLIC FIELDS
    // for Singleton Pattern
    public static Controlador Instance;

    // PRIVATE FIELDS
    // reference to a timer to measure 
    private RopeTimer timer;
    #endregion
    #region Getters/Setters
    // PROPERTIES
    private Jugador _jugadorActual;
    public Jugador jugadorActual
    {
        get
        {
            return _jugadorActual;
        }

        set
        {
            _jugadorActual = value;
        }
    }
    #endregion

    // METHODS
    void Awake()
    {
        Instance = this;
        timer = GetComponent<RopeTimer>();
        //Recursos.InicializarCartas();
        //Recursos.CartasCreadasEnElJuego.Clear();
        //Recursos.EntesCreadosEnElJuego.Clear();
        //Recursos.InicializarJugadores();
        //INICIALIZAR CONTROLS PLAYER,ENTE
    }

    void Start()
    {
        InicializacionJuego();
        

    }

    public void InicializacionJuego()
    {
        //Debug.Log("In TurnManager.OnGameStart()");

        Recursos.CartasCreadasEnElJuego.Clear();
        Recursos.EntesCreadosEnElJuego.Clear();
        Recursos.InicializarJugadores();

        foreach (Jugador p in Players.Instance.GetPlayers())
        {
            p.InicializarValores();
            ActualizarManaJugador(p);
        }

        Sequence s = DOTween.Sequence();
        //mueve los jugadores del centro a su posición
        s.Append(Players.Instance.GetPlayers()[0].PArea.Personaje.transform.DOMove(Players.Instance.GetPlayers()[0].PArea.PosicionPersonaje.position, 1f).SetEase(Ease.InQuad));
        s.Insert(0f, Players.Instance.GetPlayers()[1].PArea.Personaje.transform.DOMove(Players.Instance.GetPlayers()[1].PArea.PosicionPersonaje.position, 1f).SetEase(Ease.InQuad));
        //espera 3 segundos antes de ejecutar el onComplete
        s.PrependInterval(3f);
        s.OnComplete(() =>
        {
            // determine who starts the game.
            //int rnd = Random.Range(0,2);  // 2 is exclusive boundary
            int rnd = 1;
            // Debug.Log(Player.Players.Length);
            Jugador whoGoesFirst = Players.Instance.GetPlayers()[rnd];
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

    public void EndTurnTest()
    {
        timer.StopTimer();
        timer.StartTimer();
    }

    public void EndTurn()
    {
        // stop timer
        timer.StopTimer();
        // send all commands in the end of current player`s turn
        jugadorActual.OnTurnEnd();

        new StartATurnCommand(OtroJugador(jugadorActual)).AñadirAlaCola();
    }

    public void StopTheTimer()
    {
        timer.StopTimer();
    }

    public void ActualizarValoresJugador()
    {
        timer.StartTimer();

        ActivarBotonFinDeTurno(_jugadorActual);

        TurnMaker tm = jugadorActual.GetComponent<TurnMaker>();
        // player`s method OnTurnStart() will be called in tm.OnTurnStart();
        tm.OnTurnStart();
        if (tm is PlayerTurnMaker)
        {
            ActualizarManaJugador(jugadorActual);
        }
        OcultarCartasJugablesJugadorContrario();
    }
    //TODO creo que falta añadir en que area se esta mirando
    public bool SePermiteControlarElJugador(Jugador ownerPlayer)
    {
        bool TurnoDelJugador = (Controlador.Instance.jugadorActual == ownerPlayer);
        bool NoCartasPendientesPorMostrar = !Comandas.Instance.ComandasDeDibujoCartaPendientes();
        return ownerPlayer.PArea.PermitirControlJugador && ownerPlayer.PArea.ControlActivado && TurnoDelJugador && NoCartasPendientesPorMostrar;
    }

    public void ActivarBotonFinDeTurno(Jugador P)
    {
        if (SePermiteControlarElJugador(P))
            DatosGenerales.Instance.EndTurnButton.interactable = true;
        else
            DatosGenerales.Instance.EndTurnButton.interactable = false;

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
            if (jugador.NumCartasMano() < jugador.PArea.manoVisual.slots.Children.Length)
            {
                // 1) logic: add card to hand
                Carta newCard = new Carta(jugador.CartasEnElMazo()[0]);
                jugador.AñadirCartaMano(0, newCard);
                // Debug.Log(hand.CardsInHand.Count);
                // 2) logic: remove the card from the deck
                jugador.EliminarCartaMazo(0);
                // 2) create a command
                new DrawACardCommand(jugador.CartasEnLaMano()[0], jugador, fast, fromDeck: true).AñadirAlaCola();
            }
        }
        else
        {
            // there are no cards in the deck, take fatigue damage.
        }

    }

    /// <summary>
    /// Jugar una carta fuera de la mano en el tablero
    /// </summary>
    /// <param name="jugador"></param>
    /// <param name="assetCarta"></param>
    public void DibujarCartaFueraMazo(Jugador jugador, CardAsset assetCarta)
    {
        if (jugador.NumCartasMano() < jugador.PArea.manoVisual.slots.Children.Length)
        {
            // 1) logic: add card to hand
            Carta newCard = new Carta(assetCarta);
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
        JugarMagicaMano(Recursos.CartasCreadasEnElJuego[UniqueID], tablePos);
    }

    /// <summary>
    /// Jugar una carta de la mano en el tablero mágica
    /// </summary>
    /// <param name="magicaJugada"></param>
    /// <param name="tablePos"></param>
    public void JugarMagicaMano(Carta magicaJugada, int tablePos)
    {
        RestarManaCarta(jugadorActual, magicaJugada);
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
        JugarCartaMano(Recursos.CartasCreadasEnElJuego[UniqueID], tablePos, posicionAtaque);
    }

    /// <summary>
    /// Jugar una carta de la mano en el tablero no mágica
    /// </summary>
    /// <param name="cartaJugada"></param>
    /// <param name="tablePos"></param>
    /// <param name="posicionAtaque"></param>
    public void JugarCartaMano(Carta cartaJugada, int tablePos, bool posicionAtaque)
    {
        RestarManaCarta(jugadorActual, cartaJugada);
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
        jugadorActual.AñadirEnteMesa(tablePos, ente);
        // no matter what happens, move this card to PlayACardSpot
        new PlayAEntityCommand(cartaJugada, jugadorActual, tablePos, ente).AñadirAlaCola();
        //causa battlecry effect
        if (ente.efecto != null)
            ente.efecto.WhenACreatureIsPlayed();
        // remove this card from hand
        jugadorActual.EliminarCartaMano(cartaJugada);
        MostrarCartasJugablesJugadorActual();
    }

    /// <summary>
    /// Resta mana al jugador según la carta lanzada al tablero
    /// </summary>
    /// <param name="jugador"></param>
    /// <param name="carta"></param>
    private void RestarManaCarta(Jugador jugador, Carta carta)
    {
        jugador.ManaRestante -= carta.CosteManaActual;
        ActualizarManaJugador(jugador);
    }

    /// <summary>
    /// Funcion que para el movimiento de los jugadores, el temporizador de turno y lanza el mensaje de fin de batalla
    /// </summary>
    public void MuerteJugador(Jugador jugador)
    {
        PararControlJugadores();
        Controlador.Instance.StopTheTimer();
        new GameOverCommand(jugador).AñadirAlaCola();
    }

    public void PararControlJugadores()
    {
        foreach (Jugador player in Players.Instance.GetPlayers())
        {
            player.PArea.ControlActivado = false;
        }
    }

    // Muestra cartas jugables de la mano del jugador
    public void MostrarCartasJugablesJugadorActual()
    {
        MostrarUOcultarCartas(jugadorActual, false);
    }

    public void OcultarCartasJugablesJugadorContrario()
    {
        MostrarUOcultarCartas(OtroJugador(jugadorActual), true);
    }

    public void MostrarUOcultarCartas(Jugador jugador, bool quitarTodasRemarcadas = false)
    {
        //Debug.Log("HighlightPlayable remove: "+ removeAllHighlights);
        foreach (Carta cl in jugador.CartasEnLaMano())
        {
            GameObject g = IDHolder.GetGameObjectWithID(cl.ID);
            if (g != null)
                g.GetComponent<OneCardManager>().PuedeSerJugada = (cl.CosteManaActual <= jugadorActual.ManaRestante) && !quitarTodasRemarcadas;
        }

        foreach (Ente crl in jugador.EntesEnLaMesa())
        {
            GameObject g = IDHolder.GetGameObjectWithID(crl.ID);
            if (g != null)
                g.GetComponent<OneCreatureManager>().PuedeAtacar = (crl.AtaquesRestantesEnTurno > 0) && !quitarTodasRemarcadas;
        }
    }

    public void ActualizarManaJugador(Jugador jugador)
    {
        new UpdateManaCrystalsCommand(jugador, jugador.ManaEnEsteTurno, jugador.ManaRestante).AñadirAlaCola();
        if (jugador == jugadorActual)
            MostrarCartasJugablesJugadorActual();
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


    /***************************************** CRIATURA ****************************************************/

    // returns true if we can attack with this creature now
    /*public bool PuedeAtacar
    {
        get
        {
            bool nuestroTurno = (jugadorActual == jugador);
            return (nuestroTurno && (AtaquesRestantesEnTurno > 0) && !Frozen);
        }
    }*/

    public void MuerteCriatura(int idCriatura)
    {
        //TODO mejorar estas lineas que vuelven a coger la criatura a partir de su id
        Criatura criatura = (Criatura)Recursos.EntesCreadosEnElJuego[idCriatura];
        jugadorActual.EliminarEnteMesa(criatura);
        criatura.Morir();
        new CreatureDieCommand(idCriatura, jugadorActual).AñadirAlaCola();
    }

    /*public void AtacarCriatura(int idAtacante, int idObjetivo)
    {
        //TODO ver quien ataca, si magica o criatura
        Criatura atacante = (Criatura)Recursos.EntesCreadosEnElJuego[idAtacante];
        Ente objetivo = Recursos.EntesCreadosEnElJuego[idObjetivo];
        atacante.AtaquesRestantesEnTurno--;
        if (atacante.PosicionCriatura.Equals(PosicionCriatura.ATAQUE))
        {
            //TODO se deberia mirar si el valor atacante.Ataque es menor que objetivo.Ataque, en ese caso no le hariamos daño a la carta (?)
            //Podriamos incluso hacer objetivo.Defensa -= atacante.Ataque, restar directamente sin pasar por el ataque del objetivo
            objetivo.Defensa = atacante.Ataque - objetivo.Ataque;
            
        }
        else
        {
            //TODO se deberia mirar si el valor atacante.Ataque es menor que objetivo.Defensa, en ese caso no le hariamos daño a la carta (?)
            objetivo.Defensa = atacante.Ataque - objetivo.Defensa;
        }
        //TODO el parametro objetivo.Ataque sobraria puesto que al atacar el objetivo no me quitaria vida
        new CreatureAttackCommand(objetivo.ID, atacante.ID, objetivo.Ataque, atacante.Ataque, atacante.Defensa, objetivo.Defensa).AñadirAlaCola();
        if (objetivo.Defensa <= 0)
            MuerteCriatura(idObjetivo);
        //TODO esta linea sobraria, es la que hace que la propia criatura se quite vida
        if (atacante.Defensa <= 0)
            MuerteCriatura(idAtacante);
        //TODO quitar vida al jugador, se haria jugadorObjetivo.Defensa -= objetivo.Defensa
        new QuitarVidaJugadorComanda(jugadorObjetivo,objetivo.Defensa);
        if(jugadorObjetivo.Defensa <= 0)
            MuerteJugador(jugadorObjetivo);

    }*/
    /// <summary>
    /// Permite atacar un ente a partir de otro ente
    /// </summary>
    /// <param name="idAtacante"></param>
    /// <param name="idObjetivo"></param>
    public void AtacarCriatura(int idAtacante, int idObjetivo)
    {
        //TODO ver quien ataca, si magica o criatura
        Criatura atacante = (Criatura)Recursos.EntesCreadosEnElJuego[idAtacante];
        Criatura objetivo = (Criatura)Recursos.EntesCreadosEnElJuego[idObjetivo];
        atacante.AtaquesRestantesEnTurno--;
        // calculate the values so that the creature does not fire the DIE command before the Attack command is sent
        int targetHealthAfter = objetivo.Defensa - atacante.Ataque;
        int attackerHealthAfter = atacante.Defensa - objetivo.Ataque;
        new CreatureAttackCommand(objetivo.ID, atacante.ID, objetivo.Ataque, atacante.Ataque, attackerHealthAfter, targetHealthAfter).AñadirAlaCola();

        objetivo.Defensa -= atacante.Ataque;
        if (objetivo.Defensa <= 0)
            MuerteCriatura(idObjetivo);
        //TODO esta linea sobraria, es la que hace que la propia criatura se quite vida
        atacante.Defensa -= objetivo.Ataque;
        if (atacante.Defensa <= 0)
            MuerteCriatura(idAtacante);

    }
    public Jugador OtroJugador(Jugador jugador)
    {
        return Players.Instance.GetPlayers()[0] == jugador ? Players.Instance.GetPlayers()[1] : Players.Instance.GetPlayers()[0];
    }

    public bool CartaOCriaturaDelJugador(String tagCartaOCriatura)
    {
        //Se trata de una carta
        /*if (tagCartaOCriatura.Equals("TopCard"))
        //Se trata de un ente
        else if (tagCartaOCriatura.Equals("TopEnte"))*/
        //Acceder al controlador del jugador y comprobar si la posicion del jugador actual coincide con la de la carta o ente.
        //return controladorJugador.CartaOCriaturaDelJugador(tag.Substring(0, 3));
        return true;
    }

}

