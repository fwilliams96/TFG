using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using UnityEditor;
using System;
using System.Reflection;

// this class will take care of switching turns and counting down time until the turn expires
public class Controlador : MonoBehaviour {

    // PUBLIC FIELDS
    public CardAsset CoinCard;

    // for Singleton Pattern
    public static Controlador Instance;

    // PRIVATE FIELDS
    // reference to a timer to measure 
    private RopeTimer timer;


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


    // METHODS
    void Awake()
    {
        Instance = this;
        timer = GetComponent<RopeTimer>();
    }

    void Start()
    {
        OnGameStart();
        //Recursos.InicializarCartas();
       
    }

    public void OnGameStart()
    {
        //Debug.Log("In TurnManager.OnGameStart()");

        Recursos.CartasCreadasEnElJuego.Clear();
        Recursos.CriaturasCreadasEnElJuego.Clear();
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
                DibujarCartasMazo(whoGoesFirst,4, true);
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
            //TODO creo que debo llamar tambien a actualizar mana
            ActualizarManaJugador(jugadorActual);
        }
        // remove highlights for opponent.
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

    public void DibujarCartasMazo(Jugador player, int numCartas, bool fast = false)
    {
        for (int i = 0; i < numCartas; i++)
        {
            DibujarCartaMazo(player,fast);
        }
    }


    // draw a single card from the deck
    public void DibujarCartaMazo(Jugador jugador,bool fast = false)
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

    // get card NOT from deck (a token or a coin)
    //TODO eliminar porque no tenemos cartas que provengan fuera del mazo (coincard)
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

    // 2 METHODS FOR PLAYING SPELLS
    // 1st overload - takes ids as arguments
    // it is cnvenient to call this method from visual part
    public void JugarSpellMano(int idCartaSpell, int idObjetivo)
    {
        // TODO: !!!
        // if TargetUnique ID < 0 , for example = -1, there is no target.
        if (idObjetivo < 0)
            JugarSpellMano(Recursos.CartasCreadasEnElJuego[idCartaSpell], null);
        //TODO estas dos siguientes condiciones sobraran puesto que no se podrá ir de cara al personaje
        else
        {
            // target is a creature
            JugarSpellMano(Recursos.CartasCreadasEnElJuego[idCartaSpell], Recursos.CriaturasCreadasEnElJuego[idObjetivo]);
        }

    }

    // 2nd overload - takes CardLogic and ICharacter interface - 
    // this method is called from Logic, for example by AI
    public void JugarSpellMano(Carta playedCard, ICharacter target)
    {
        jugadorActual.ManaRestante -= playedCard.CosteManaActual;
        ActualizarManaJugador(jugadorActual);
        // cause effect instantly:
        if (playedCard.efecto != null)
            playedCard.efecto.ActivateEffect(playedCard.assetCarta.specialSpellAmount, target);
        else
        {
            Debug.LogWarning("No effect found on card " + playedCard.assetCarta.name);
        }
        // no matter what happens, move this card to PlayACardSpot
        new PlayASpellCardCommand(jugadorActual, playedCard).AñadirAlaCola();
        // remove this card from hand
        jugadorActual.EliminarCartaMano(playedCard);
        // check if this is a creature or a spell
    }

    // METHODS TO PLAY CREATURES 
    // 1st overload - by ID
    public void JugarCartaMano(int UniqueID, int tablePos, bool posicionAtaque)
    {
        JugarCartaMano(Recursos.CartasCreadasEnElJuego[UniqueID], tablePos, posicionAtaque);
    }

    // 2nd overload - by logic units
    public void JugarCartaMano(Carta cartaJugada, int tablePos, bool posicionAtaque)
    {
        // Debug.Log(ManaLeft);
        // Debug.Log(playedCard.CurrentManaCost);
        jugadorActual.ManaRestante -= cartaJugada.CosteManaActual;
        ActualizarManaJugador(jugadorActual);
        // Debug.Log("Mana Left after played a creature: " + ManaLeft);
        // create a new creature object and add it to Table
        Criatura newCreature = new Criatura(cartaJugada.assetCarta);
        jugadorActual.AñadirCriaturaMesa(tablePos, newCreature);
        // no matter what happens, move this card to PlayACardSpot
        new PlayACreatureCommand(cartaJugada, jugadorActual, tablePos, posicionAtaque, newCreature.idCriatura).AñadirAlaCola();
        //causa battlecry effect
        if (newCreature.efecto != null)
            newCreature.efecto.WhenACreatureIsPlayed();
        // remove this card from hand
        jugadorActual.EliminarCartaMano(cartaJugada);
        MostrarCartasJugablesJugadorActual();
    }

    public void Morir()
    {
        // game over
        // block both players from taking new moves
        PararControlJugadores();
        Controlador.Instance.StopTheTimer();
        new GameOverCommand(jugadorActual).AñadirAlaCola();
    }

    public void PararControlJugadores()
    {
        foreach(Jugador player in Players.Instance.GetPlayers())
        {
            player.PArea.ControlActivado = false;
        }
    }

    // METHOD TO SHOW GLOW HIGHLIGHTS
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
            GameObject g = IDHolder.GetGameObjectWithID(cl.idCarta);
            if (g != null)
                g.GetComponent<OneCardManager>().PuedeSerJugada = (cl.CosteManaActual <= jugadorActual.ManaRestante) && !quitarTodasRemarcadas;
        }

        foreach (Criatura crl in jugador.CriaturasEnLaMesa())
        {
            GameObject g = IDHolder.GetGameObjectWithID(crl.idCriatura);
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
        Criatura criatura = Recursos.CriaturasCreadasEnElJuego[idCriatura];
        jugadorActual.EliminarCriaturaMesa(criatura);
        criatura.Morir();
        new CreatureDieCommand(idCriatura, jugadorActual).AñadirAlaCola();
    }

    public void AtacarCriatura(int idAtacante, int idObjetivo)
    {
        Criatura atacante = Recursos.CriaturasCreadasEnElJuego[idAtacante];
        Criatura objetivo = Recursos.CriaturasCreadasEnElJuego[idObjetivo];
        atacante.AtaquesRestantesEnTurno--;
        // calculate the values so that the creature does not fire the DIE command before the Attack command is sent
        int targetHealthAfter = objetivo.Vida - atacante.Ataque;
        int attackerHealthAfter = atacante.Vida - objetivo.Ataque;
        new CreatureAttackCommand(objetivo.idCriatura, atacante.idCriatura, objetivo.Ataque, atacante.Ataque, attackerHealthAfter, targetHealthAfter).AñadirAlaCola();

        objetivo.Vida -= atacante.Ataque;
        if (objetivo.Vida <= 0)
            MuerteCriatura(idObjetivo);
        //TODO esta linea sobraria, es la que hace que la propia criatura se quite vida
        atacante.Vida -= objetivo.Ataque;
        if(atacante.Vida <= 0)
            MuerteCriatura(idAtacante);

    }

    public Jugador OtroJugador(Jugador jugador)
    {
        return Players.Instance.GetPlayers()[0] == jugador ? Players.Instance.GetPlayers()[1] : Players.Instance.GetPlayers()[0];
    }

}

