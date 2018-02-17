﻿using UnityEngine;
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
    private Player _whoseTurn;
    public Player whoseTurn
    {
        get
        {
            return _whoseTurn;
        }

        set
        {
            _whoseTurn = value;   
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

        CardLogic.CardsCreatedThisGame.Clear();
        CreatureLogic.CreaturesCreatedThisGame.Clear();

        foreach (Player p in Players.Instance.GetPlayers())
        {
            p.InicializarValores();
        }

        Sequence s = DOTween.Sequence();
        s.Append(Players.Instance.GetPlayers()[0].PArea.Portrait.transform.DOMove(Players.Instance.GetPlayers()[0].PArea.PortraitPosition.position, 1f).SetEase(Ease.InQuad));
        s.Insert(0f, Players.Instance.GetPlayers()[1].PArea.Portrait.transform.DOMove(Players.Instance.GetPlayers()[1].PArea.PortraitPosition.position, 1f).SetEase(Ease.InQuad));
        s.PrependInterval(3f);
        s.OnComplete(() =>
            {
                // determine who starts the game.
                //int rnd = Random.Range(0,2);  // 2 is exclusive boundary
                int rnd = 1;
                // Debug.Log(Player.Players.Length);
                Player whoGoesFirst = Players.Instance.GetPlayers()[rnd];
                // Debug.Log(whoGoesFirst);
                Player whoGoesSecond = whoGoesFirst.otherPlayer;
                // Debug.Log(whoGoesSecond);
         
                // draw 4 cards for first player and 5 for second player
                int initDraw = 4;
                for (int i = 0; i < initDraw; i++)
                {            
                    // second player draws a card
                    whoGoesSecond.DrawACard(true);
                    // first player draws a card
                    whoGoesFirst.DrawACard(true);
                }
                // add one more card to second player`s hand
                whoGoesSecond.DrawACard(true);
                //new GivePlayerACoinCommand(null, whoGoesSecond).AddToQueue();
                whoGoesSecond.GetACardNotFromDeck(CoinCard);
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
        whoseTurn.OnTurnEnd();

        new StartATurnCommand(whoseTurn.otherPlayer).AñadirAlaCola();
    }

    public void StopTheTimer()
    {
        timer.StopTimer();
    }

    public void ActualizarValoresJugador()
    {
        timer.StartTimer();

        GlobalSettings.Instance.EnableEndTurnButtonOnStart(_whoseTurn);

        TurnMaker tm = whoseTurn.GetComponent<TurnMaker>();
        // player`s method OnTurnStart() will be called in tm.OnTurnStart();
        tm.OnTurnStart();
        if (tm is PlayerTurnMaker)
        {
            whoseTurn.MostrarCartasJugables();
        }
        // remove highlights for opponent.
        whoseTurn.otherPlayer.MostrarCartasJugables(true);
    }

}
