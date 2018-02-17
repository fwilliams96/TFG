using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using UnityEditor;
using System;
using System.Reflection;

// this class will take care of switching turns and counting down time until the turn expires
public class ControladorTurno : MonoBehaviour {

    // PUBLIC FIELDS
    public CardAsset CoinCard;

    // for Singleton Pattern
    public static ControladorTurno Instance;

    // PRIVATE FIELDS
    // reference to a timer to measure 
    private RopeTimer timer;


    // PROPERTIES
    private Player _jugadorActual;
    public Player jugadorActual
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

        foreach (Player p in Players.Instance.GetPlayers())
        {
            p.InicializarValores();
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
                Player whoGoesFirst = Players.Instance.GetPlayers()[rnd];
                // Debug.Log(whoGoesFirst);
                Player whoGoesSecond = whoGoesFirst.otroJugador;
                // Debug.Log(whoGoesSecond);
                // draw 4 cards for first player and 5 for second player
                // first player draws a card
                whoGoesFirst.DibujarCartasMazo(4, true);
                // second player draws a card
                whoGoesSecond.DibujarCartasMazo(4, true);
                // add one more card to second player`s hand
               // whoGoesSecond.DibujarCartaMazo(true);
                //new GivePlayerACoinCommand(null, whoGoesSecond).AddToQueue();
               // whoGoesSecond.DibujarCartaFueraMazo(CoinCard);
                new StartATurnCommand(whoGoesFirst).AñadirAlaCola();
            });
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            EndTurn();
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

        new StartATurnCommand(jugadorActual.otroJugador).AñadirAlaCola();
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
            jugadorActual.MostrarCartasJugables();
        }
        // remove highlights for opponent.
        jugadorActual.otroJugador.MostrarCartasJugables(true);
    }
    //TODO creo que falta añadir en que area se esta mirando
    public bool SePermiteControlarElJugador(Player ownerPlayer)
    {
        bool TurnoDelJugador = (ControladorTurno.Instance.jugadorActual == ownerPlayer);
        bool NoCartasPendientesPorMostrar = !Comandas.Instance.ComandasDeDibujoCartaPendientes();
        return ownerPlayer.PArea.PermitirControlJugador && ownerPlayer.PArea.ControlActivado && TurnoDelJugador && NoCartasPendientesPorMostrar;
    }

    public void ActivarBotonFinDeTurno(Player P)
    {
        if (SePermiteControlarElJugador(P))
            DatosGenerales.Instance.EndTurnButton.interactable = true;
        else
            DatosGenerales.Instance.EndTurnButton.interactable = false;

    }

}

