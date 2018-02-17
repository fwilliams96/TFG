using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CreatureLogic: ICharacter 
{
    // PUBLIC FIELDS
    public Player jugador;
    public CardAsset assetCarta;
    public CreatureEffect efecto;
    public int idCriatura;
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
            if (value > VidaMaxima)
                vida = VidaMaxima;
            else if (value <= 0)
                Morir();
            else
                vida = value;
        }
    }

    // returns true if we can attack with this creature now
    public bool PuedeAtacar
    {
        get
        {
            bool nuestroTurno = (ControladorTurno.Instance.jugadorActual == jugador);
            return (nuestroTurno && (AtaquesRestantesEnTurno > 0) && !Frozen);
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
    public CreatureLogic(Player owner, CardAsset ca)
    {
        this.assetCarta = ca;
        vidaBase = ca.Defensa;
        Vida = ca.Defensa;
        ataqueBasico = ca.Ataque;
        attacksForOneTurn = ca.AtaquesPorTurno;
        // AttacksLeftThisTurn is now equal to 0
        if (ca.Charge)
            AtaquesRestantesEnTurno = attacksForOneTurn;
        this.jugador = owner;
        idCriatura = IDFactory.GetUniqueID();
        if (ca.CreatureScriptName!= null && ca.CreatureScriptName!= "")
        {
            efecto = System.Activator.CreateInstance(System.Type.GetType(ca.CreatureScriptName), new System.Object[]{owner, this, ca.specialCreatureAmount}) as CreatureEffect;
            efecto.RegisterEventEffect();
        }
        Recursos.CriaturasCreadasEnElJuego.Add(idCriatura, this);
    }

    // METHODS
    public void OnTurnStart()
    {
        AtaquesRestantesEnTurno = attacksForOneTurn;
    }

    public void Morir()
    {
        jugador.EliminarCriaturaMesa(this);
        //cause deathrattle effect
        if (efecto != null)
            efecto.WhenACreatureDies();
        new CreatureDieCommand(idCriatura, jugador).AñadirAlaCola();
    }

    //TODO POR ELIMINAR, NO SE PODRÁ IR DE CARA
    public void GoFace()
    {
        AtaquesRestantesEnTurno--;
        int targetHealthAfter = jugador.otroJugador.Vida - Ataque;
        new CreatureAttackCommand(jugador.otroJugador.PlayerID, idCriatura, 0, Ataque, Vida, targetHealthAfter).AñadirAlaCola();
        jugador.otroJugador.Vida -= Ataque;
    }

    public void AtacarCriatura (CreatureLogic target)
    {
        AtaquesRestantesEnTurno--;
        // calculate the values so that the creature does not fire the DIE command before the Attack command is sent
        int targetHealthAfter = target.Vida - Ataque;
        int attackerHealthAfter = Vida - target.Ataque;
        new CreatureAttackCommand(target.idCriatura, idCriatura, target.Ataque, Ataque, attackerHealthAfter, targetHealthAfter).AñadirAlaCola();

        target.Vida -= Ataque;
        //TODO esta linea sobraria, es la que hace que la propia criatura se quite vida
        Vida -= target.Ataque;
    }

    public void AtacarCriaturaPorID(int uniqueCreatureID)
    {
        CreatureLogic target = Recursos.CriaturasCreadasEnElJuego[uniqueCreatureID];
        AtacarCriatura(target);
    }

}
