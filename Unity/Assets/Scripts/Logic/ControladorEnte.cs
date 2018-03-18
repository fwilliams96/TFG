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

    public void ActivarEfectoMagica(int idMagica)
    {
        Magica magica = (Magica)Recursos.EntesCreadosEnElJuego[idMagica];
        magica.EfectoActivado = true;
        new ActivateEffectCommand(idMagica).AñadirAlaCola();
    }

    public void CambiarPosicionCriatura(int idCriatura)
    {
        Criatura criatura = (Criatura)Recursos.EntesCreadosEnElJuego[idCriatura];
        if (criatura.PosicionCriatura.Equals(PosicionCriatura.ATAQUE))
        {
            new ChangeCreaturePosition(idCriatura, PosicionCriatura.DEFENSA).AñadirAlaCola();
        }
        else
        {
            new ChangeCreaturePosition(idCriatura, PosicionCriatura.ATAQUE).AñadirAlaCola();
        }
    }

    public void MostrarAccion(int idEnte)
    {
        Ente ente = Recursos.EntesCreadosEnElJuego[idEnte];
        if (ente.GetType() == typeof(Magica))
        {
            //Solo mostraremos la opcion activar efecto si no lo ha activado aun
            if (!((Magica)ente).EfectoActivado)
            {
                AccionesPopUp.Instance.MostrarAccionEfecto();
                AccionesPopUp.Instance.RegistrarCallBack(ActivarEfectoMagica, idEnte);
            }
        }
        else
        {
            Criatura criatura = (Criatura)ente;
            if (criatura.PosicionCriatura.Equals(PosicionCriatura.ATAQUE))
            {
                AccionesPopUp.Instance.MostrarAccionDefensa();
            }
            else
            {
                AccionesPopUp.Instance.MostrarAccionAtaque();
            }
            AccionesPopUp.Instance.RegistrarCallBack(CambiarPosicionCriatura, idEnte);
        }
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
            return ((Criatura)ente).PosicionCriatura.Equals(PosicionCriatura.ATAQUE);
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