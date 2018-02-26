using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Ente: ICharacter 
{
    // PUBLIC FIELDS
    public CardAsset assetCarta;
    public EfectoEnte efecto;
    private int idCriatura;
    public bool Frozen = false;

    // PROPERTIES
    // property from ICharacter interface
    public int ID
    {
        get{ return idCriatura; }
    }
        
    // the basic health that we have in CardAsset
    private int vidaBase;
    // health with all the current buffs taken into account
    public int VidaMaxima
    {
        get{ return vidaBase;}
    }

    // current health of this creature
    private int vida;
    public int Vida
    {
        get{ return vida; }

        set
        {
            //TODO mirar donde se hace el set para no hacer las siguientes lineas
            if (value > VidaMaxima)
                vida = VidaMaxima;
            else
                vida = value;
        }
    }

    // property for Attack
    private int ataqueBasico;
    public int Ataque
    {
        get{ return ataqueBasico; }
    }
     
    // number of attacks for one turn if (attacksForOneTurn==2) => Windfury
    private int attacksForOneTurn = 1;
    public int AtaquesRestantesEnTurno
    {
        get;
        set;
    }

    // CONSTRUCTOR
    public Ente(CardAsset ca)
    {
        this.assetCarta = ca;
        vidaBase = ca.Defensa;
        Vida = ca.Defensa;
        ataqueBasico = ca.Ataque;
        attacksForOneTurn = ca.AtaquesPorTurno;
        // AttacksLeftThisTurn is now equal to 0
        if (ca.Charge)
            AtaquesRestantesEnTurno = attacksForOneTurn;
        idCriatura = IDFactory.GetUniqueID();
        if (ca.CreatureScriptName!= null && ca.CreatureScriptName!= "")
        {
            //TODO le estamos pasando null de momento, este null referencia al jugador dueño de la criatura
            efecto = System.Activator.CreateInstance(System.Type.GetType(ca.CreatureScriptName), new System.Object[]{null,this, ca.specialCreatureAmount}) as EfectoEnte;
            efecto.RegisterEventEffect();
        }
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
