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
        if (objetivo.PosicionCriatura.Equals(PosicionCriatura.ATAQUE))
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
        new CreatureAttackCommand(objetivo.ID, atacante.ID, objetivo.Ataque, atacante.Ataque, atacante.Defensa, objetivo.Defensa).AñadirAlaCola();
        if(EnteMuerto(objetivo))
            MuerteEnte(objetivo.ID);
    }

    public void MuerteEnte(int idCriatura)
    {
        //TODO mejorar estas lineas que vuelven a coger la criatura a partir de su id
        Ente ente = Recursos.EntesCreadosEnElJuego[idCriatura];
        Jugador jugadorObjetivo = Controlador.Instance.OtroJugador(Controlador.Instance.jugadorActual);
        jugadorObjetivo.EliminarEnteMesa(ente);
        ente.Morir();
        new CreatureDieCommand(idCriatura, jugadorObjetivo).AñadirAlaCola();
    }

    public bool EnteMuerto(Criatura objetivo)
    {
        return objetivo.Defensa <= 0;
    }

}