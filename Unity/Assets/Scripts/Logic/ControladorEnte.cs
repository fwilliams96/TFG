using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

	public void QuitarVidaCriatura(Criatura objetivo,int daño)
	{
		new DealDamageCommand(objetivo.ID, daño,objetivo.Defensa).AñadirAlaCola();
		objetivo.Defensa -= daño;
		if(CriaturaMuerta(objetivo))
			MuerteEnte(objetivo.ID);
	}

    /// <summary>
    /// Permite atacar a una criatura a partir de otro ente
    /// </summary>
    /// <param name="idAtacante"></param>
    /// <param name="idObjetivo"></param>
    public void AtacarCriatura(Criatura atacante,  Criatura objetivo)
    {
        atacante.AtaquesRestantesEnTurno--;
		new CreatureAttackCommand(objetivo.ID, atacante.ID, atacante.Ataque,objetivo.Defensa).AñadirAlaCola();
		objetivo.Defensa -= atacante.Ataque;
		if(CriaturaMuerta(objetivo))
            MuerteEnte(objetivo.ID);
    }

	public void AtacarMagica(Criatura atacante,  Magica objetivo)
	{
		atacante.AtaquesRestantesEnTurno--;
		new CreatureAttackCommand(objetivo.ID, atacante.ID, atacante.Ataque, objetivo.Defensa).AñadirAlaCola();
		//MuerteEnte(objetivo.ID);
	}

	public void ActivarEfectoMagica(int idMagica){
		ActivarEfectoMagica (idMagica, -1);
	}

	public void ActivarEfectoMagica(int idMagica, int idAtacante)
    {
		Criatura criaturaAtacante = null;
		JugadorPartida jugador = null;
		Magica magica = (Magica)Recursos.EntesCreadosEnElJuego[idMagica];
		if (idAtacante != -1) {
			criaturaAtacante = (Criatura)Recursos.EntesCreadosEnElJuego [idAtacante];
			AtacarMagica (criaturaAtacante, magica);
		}
        
        new ActivateEffectCommand(idMagica).AñadirAlaCola();
		switch (magica.AssetCarta.Efecto) {
			case Efecto.Destructor:
				jugador = Controlador.Instance.ObtenerDueñoEnte (magica);
				DamageAllCreatures(Controlador.Instance.OtroJugador(jugador),100);
				break;
			case Efecto.Espejo:
				DealDamageToTarget(criaturaAtacante,criaturaAtacante.AssetCarta.Ataque);
				break;
			case Efecto.Mana:
				jugador = Controlador.Instance.ObtenerDueñoEnte (magica);
				GiveManaBonus(jugador,2);
				break;
			case Efecto.Vida:
				//TODO por implementar, dar vida a todas las criaturas
				jugador = Controlador.Instance.ObtenerDueñoEnte (magica);
				GiveHealth (jugador, 100);
				break;
			default:
				break;
		}

		//MuerteEnte(magica.ID);
    }

	public void DamageAllCreatures(JugadorPartida jugador, int daño)
	{
		List<Ente> CreaturesToDamage = jugador.EntesEnLaMesa();
		foreach (Ente cl in CreaturesToDamage)
		{
			if (cl.GetType () == typeof(Criatura)) {
				Controlador.Instance.DañarCriatura ((Criatura)cl,daño);
			}
		}
	}

	public  void GiveHealth(JugadorPartida jugador, int vida){
		
	}
	public void GiveManaBonus(JugadorPartida jugador, int mana)
	{
		jugador.ConseguirManaExtra(mana);
	}

	public void DealDamageToTarget(Criatura criatura, int daño)
	{
		Controlador.Instance.DañarCriatura (criatura, daño);
	}

    public void CambiarPosicionCriatura(int idCriatura)
    {
        Criatura criatura = (Criatura)Recursos.EntesCreadosEnElJuego[idCriatura];
        if (criatura.PosicionCriatura.Equals(PosicionCriatura.ATAQUE))
        {
            criatura.PosicionCriatura = PosicionCriatura.DEFENSA;
            new ChangeCreaturePosition(idCriatura, PosicionCriatura.DEFENSA).AñadirAlaCola();
        }
        else
        {
            criatura.PosicionCriatura = PosicionCriatura.ATAQUE;
            new ChangeCreaturePosition(idCriatura, PosicionCriatura.ATAQUE).AñadirAlaCola();
        }
    }

    public void MostrarAccion(int idEnte)
    {
        Ente ente = Recursos.EntesCreadosEnElJuego[idEnte];
        if (ente.GetType() == typeof(Magica))
        {
			//Si la magica es de tipo trampa no debe salir la opcion de activar voluntariamente
			if (EntePreparado(ente) && !EsMagicaTrampa(ente))
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
				if(!CriaturaHaAtacado(criatura))
                	AccionesPopUp.Instance.MostrarAccionDefensa();
            }
            else
            {
                AccionesPopUp.Instance.MostrarAccionAtaque();
            }
            AccionesPopUp.Instance.RegistrarCallBack(CambiarPosicionCriatura, idEnte);
        }
    }

	public bool EsMagicaTrampa(Ente ente){
		Magica magica = (Magica)ente;
		//TODO aqui añadire mas magicas de tipo trampa
		return magica.AssetCarta.Efecto.Equals (Efecto.Espejo);
	}

	public bool EntePreparado(Ente ente){
		return ente.AtaquesRestantesEnTurno > 0;
	}

	public bool CriaturaHaAtacado(Criatura criatura){
		return criatura.HaAtacado;
	}

    public void MuerteEnte(int idEnte)
    {
        Ente ente = Recursos.EntesCreadosEnElJuego[idEnte];
		JugadorPartida jugadorObjetivo = Controlador.Instance.ObtenerDueñoEnte (ente);
        jugadorObjetivo.EliminarEnteMesa(ente);
        ente.Morir();
        new CreatureDieCommand(idEnte, jugadorObjetivo).AñadirAlaCola();
    }
		
    public bool CriaturaMuerta(Criatura objetivo)
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

	public void Clear(){
		instance = null;
	}

}