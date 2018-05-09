using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Ente : ICharacter
{
    #region Atributos
    // PUBLIC FIELDS
    private CartaAsset assetCarta;
    public EfectoEnte efecto;
    private int idCriatura;
    public bool Frozen = false;
    #endregion
    #region Getters/Setters
    // property from ICharacter interface
	public CartaAsset AssetCarta{
		get{
			return assetCarta;
		}
		set{
			assetCarta = value;
		}
	}
    public int ID
    {
        get { return idCriatura; }
    }

    // the basic health that we have in CardAsset
    private int defensaBase;
    // health with all the current buffs taken into account
    public int DefensaMaxima
    {
        get { return defensaBase; }
    }

    // current health of this creature
    private int defensa;
    public int Defensa
    {
        get { return defensa; }

        set
        {
            //TODO mirar donde se hace el set para no hacer las siguientes lineas
            if (value > DefensaMaxima)
                defensa = DefensaMaxima;
            else
                defensa = value;
        }
    }

    // property for Attack
    private int ataqueBasico;
    public int Ataque
    {
        get { return ataqueBasico; }
    }

    // number of attacks for one turn if (attacksForOneTurn==2) => Windfury
    private int attacksForOneTurn = 1;
    public int AtaquesRestantesEnTurno
    {
        get;
        set;
    }

    #endregion

    // CONSTRUCTOR
    public Ente(CartaAsset ca)
    {
        this.assetCarta = ca;
        defensaBase = ca.Defensa;
        Defensa = ca.Defensa;
        ataqueBasico = ca.Ataque;
        //attacksForOneTurn = ca.AtaquesPorTurno;
        // AttacksLeftThisTurn is now equal to 0
        //if (ca.Charge)
            //AtaquesRestantesEnTurno = attacksForOneTurn;
        idCriatura = IDFactory.GetUniqueID();
        /*if (ca.CreatureScriptName != null && ca.CreatureScriptName != "")
        {
            //TODO le estamos pasando null de momento, este null referencia al jugador dueño de la criatura
            efecto = System.Activator.CreateInstance(System.Type.GetType(ca.CreatureScriptName), new System.Object[] { null, this, ca.specialCreatureAmount }) as EfectoEnte;
            efecto.RegisterEventEffect();
        }*/
        Recursos.EntesCreadosEnElJuego.Add(idCriatura, this);
    }

    public virtual void Morir()
    {
        if (efecto != null)
            efecto.WhenACreatureDies();
    }

    public virtual void OnTurnStart()
    {
        AtaquesRestantesEnTurno = attacksForOneTurn;
    }

}
