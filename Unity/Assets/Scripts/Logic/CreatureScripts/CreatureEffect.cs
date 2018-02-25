using UnityEngine;
using System.Collections;

public abstract class CreatureEffect 
{
    protected Criatura creature;
    protected Jugador owner;
    protected int specialAmount;

    public CreatureEffect(Jugador owner,Criatura creature, int specialAmount)
    {
        this.owner = owner;
        this.creature = creature;
        this.specialAmount = specialAmount;
    }

    public virtual void RegisterEventEffect() { }

    public virtual void CauseEventEffect() { }

    //BattleCry
    public virtual void WhenACreatureIsPlayed() { }

    //DeathRattle
    public virtual void WhenACreatureDies() { }

}
