using UnityEngine;
using System.Collections;

public class ControladorEnte
{
    #region Atributos
    private static ControladorEnte instance;
    #endregion

    #region Getters/Setters
    #endregion

    private ControladorEnte()
    {
        Recursos.EntesCreadosEnElJuego.Clear();
        //Recursos.InicializarCartas();
    }

    public static ControladorEnte Instance
    {
        get
        {
            if (instance == null)
                instance = new ControladorEnte();
            return instance;
        }
    }

    /// <summary>
    /// Permite atacar un ente a partir de otro ente
    /// </summary>
    /// <param name="idAtacante"></param>
    /// <param name="idObjetivo"></param>
    public void QuitarVidaEnte(Ente atacante,  Criatura objetivo)
    {
        atacante.AtaquesRestantesEnTurno--;
        objetivo.Defensa -= objetivo.Ataque;
        new CreatureAttackCommand(objetivo.ID, atacante.ID, objetivo.Ataque, atacante.Ataque, atacante.Defensa, objetivo.Defensa).AñadirAlaCola();
        if(EnteMuerto(objetivo))
            MuerteEnte(objetivo.ID);
    }

    public void MuerteEnte(int idCriatura)
    {
        //TODO mejorar estas lineas que vuelven a coger la criatura a partir de su id
        Ente ente = Recursos.EntesCreadosEnElJuego[idCriatura];
        Jugador jugadorObjetivo = Controlador.Instance.OtroJugador(Controlador.Instance.JugadorActual);
        jugadorObjetivo.EliminarEnteMesa(ente);
        ente.Morir();
        new CreatureDieCommand(idCriatura, jugadorObjetivo).AñadirAlaCola();
    }

    public bool EnteMuerto(Criatura objetivo)
    {
        return objetivo.Defensa <= 0;
    }

    public bool EstaEnPosicionAtaque(int idEnte)
    {
        //TODO crear excepcion si idEnte no existe en el diccionario
        Ente ente = Recursos.EntesCreadosEnElJuego[idEnte];
        if(ente.GetType() == typeof(Criatura))
        {
            return ((Criatura)ente).PosicionCriatura == PosicionCriatura.ATAQUE;
        }
        //throw new EnteException();
        throw new System.Exception();
    }

    public bool EsMagica(int idEnte)
    {
        //TODO crear excepcion si idEnte no existe en el diccionario
        Ente ente = Recursos.EntesCreadosEnElJuego[idEnte];
        return ente.GetType() == typeof(Magica);
    }

}