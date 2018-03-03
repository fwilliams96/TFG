﻿using UnityEngine;
using System.Collections;

public class ControladorJugador
{
    #region Atributos
    private static ControladorJugador instance;
    private Jugador _jugadorActual;
    private PlayerArea areaJugadorActual;
    #endregion

    #region Getters/Setters
    public Jugador JugadorActual
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
        if (jugador == _jugadorActual)
            return areaJugadorActual;
        PlayerArea areaJugador;
        AreaPosition area = jugador.gameObject.tag == "TopPlayer" ? AreaPosition.Top : AreaPosition.Low;
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

    public bool SePermiteControlarElJugador(Jugador jugador)
    {
        PlayerArea areaJugador = AreaJugador(jugador);
        bool TurnoDelJugador = (_jugadorActual == jugador);
        bool NoCartasPendientesPorMostrar = !Comandas.Instance.ComandasDeDibujoCartaPendientes();
        return areaJugador.PermitirControlJugador && areaJugador.ControlActivado && TurnoDelJugador && NoCartasPendientesPorMostrar;
    }

    //TODO ver si esta funcion seguira aqui
    public void TransmitirInformacionVisualJugador(Jugador jugador)
    {
        PlayerArea areaJugador = AreaJugador(jugador);
        //TODO cuando el jugador muere esta dando un pete aqui por acceder a algo destruido
        areaJugador.Personaje.gameObject.AddComponent<IDHolder>().UniqueID = jugador.ID;
        if (jugador.GetComponent<TurnMaker>() is AITurnMaker)
        {
            // turn off turn making for this character
            areaJugador.PermitirControlJugador = false;
        }
        else
        {
            // allow turn making for this character
            areaJugador.PermitirControlJugador = true;
        }
    }

    public void InicializarValoresJugador(Jugador jugador)
    {
        PlayerArea areaJugador = AreaJugador(jugador);
        jugador.ManaEnEsteTurno = 0;
        jugador.ManaRestante = 0;
        //LeerInformacionPersonajeAsset();
        TransmitirInformacionVisualJugador(jugador);
        //TO
        areaJugador.mazoVisual.CartasEnMazo = jugador.NumCartasMazo();//mazo.CartasEnMazo.Count;
        // move both portraits to the center
        areaJugador.Personaje.transform.position = areaJugador.PosicionInicialPersonaje.position;
    }

    public void ActualizarManaJugador(Jugador jugador)
    {
        new UpdateManaCrystalsCommand(jugador, jugador.ManaEnEsteTurno, jugador.ManaRestante).AñadirAlaCola();
        if (jugador == JugadorActual)
            MostrarCartasJugablesJugadorActual();
    }

    // Muestra cartas jugables de la mano del jugador
    public void MostrarCartasJugablesJugadorActual()
    {
        MostrarUOcultarCartas(JugadorActual, false);
    }

    private void OcultarCartasJugablesJugadorContrario()
    {
        MostrarUOcultarCartas(OtroJugador(JugadorActual), true);
    }

    private void MostrarUOcultarCartas(Jugador jugador, bool quitarTodasRemarcadas = false)
    {
        //Debug.Log("HighlightPlayable remove: "+ removeAllHighlights);
        foreach (Carta cl in jugador.CartasEnLaMano())
        {
            GameObject g = IDHolder.GetGameObjectWithID(cl.ID);
            if (g != null)
                g.GetComponent<OneCardManager>().PuedeSerJugada = (cl.CosteManaActual <= JugadorActual.ManaRestante) && !quitarTodasRemarcadas;
        }

        foreach (Ente crl in jugador.EntesEnLaMesa())
        {
            GameObject g = IDHolder.GetGameObjectWithID(crl.ID);
            if (g != null)
                g.GetComponent<OneCreatureManager>().PuedeAtacar = (crl.AtaquesRestantesEnTurno > 0) && !quitarTodasRemarcadas;
        }
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
            AreaJugador(player).ControlActivado = false;
        }
    }

    public bool CartaOCriaturaDelJugador(string tagCartaOCriatura)
    {
        return _jugadorActual.gameObject.tag.Substring(0,3).Equals(tagCartaOCriatura.Substring(0, 3));
    }

    public Jugador OtroJugador(Jugador jugador)
    {
        return Players.Instance.GetPlayers()[0] == jugador ? Players.Instance.GetPlayers()[1] : Players.Instance.GetPlayers()[0];
    }

    public void ActualizarValoresJugador()
    {
        TurnMaker tm = JugadorActual.GetComponent<TurnMaker>();
        // player`s method OnTurnStart() will be called in tm.OnTurnStart();
        tm.OnTurnStart();
        if (tm is PlayerTurnMaker)
        {
            ActualizarManaJugador(JugadorActual);
        }
        OcultarCartasJugablesJugadorContrario();
    }

    /// <summary>
    /// Resta mana al jugador según la carta lanzada al tablero
    /// </summary>
    /// <param name="jugador"></param>
    /// <param name="carta"></param>
    public void RestarManaCarta(Jugador jugador, Carta carta)
    {
        jugador.ManaRestante -= carta.CosteManaActual;
        ActualizarManaJugador(jugador);
    }

   public void QuitarVidaJugador(int valorAtaque)
    {
        //TODO quitar vida al jugador, se haria jugadorObjetivo.Defensa -= objetivo.Defensa
        Jugador jugadorObjetivo = OtroJugador(JugadorActual);
        jugadorObjetivo.Defensa -= valorAtaque;
        new DealDamageCommand(jugadorObjetivo.ID, valorAtaque, jugadorObjetivo.Defensa).AñadirAlaCola();
        if (JugadorMuerto(jugadorObjetivo))
            MuerteJugador(jugadorObjetivo);
    }

    public bool JugadorMuerto(Jugador jugador)
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

}
