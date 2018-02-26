using UnityEngine;
using System.Collections;

public abstract class EfectoEnte 
{
    protected Ente ente;
    protected Jugador owner;
    protected int specialAmount;

    public EfectoEnte(Jugador owner,Ente ente, int specialAmount)
    {
        this.owner = owner;
        this.ente = ente;
        this.specialAmount = specialAmount;
    }

    public virtual void RegisterEventEffect() { }

    public virtual void CauseEventEffect() { }

    //BattleCry
    public virtual void WhenACreatureIsPlayed() { }

    //BattleCry
    public virtual void WhenAMagicIsPlayed() { }

    //DeathRattle
    public virtual void WhenACreatureDies() { }

    public virtual void WhenAMagicDies() { }

}
