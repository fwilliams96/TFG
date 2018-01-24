using UnityEngine;
using System.Collections;

public abstract class CreatureEffect 
{
    protected Player owner;
    protected CreatureLogic creature;
    protected int specialAmount;

    public CreatureEffect(Player owner, CreatureLogic creature, int specialAmount)
    {
        this.creature = creature;
        this.owner = owner;
        this.specialAmount = specialAmount;
    }

    public virtual void RegisterEventEffect() { }

    public virtual void CauseEventEffect() { }

    //BattleCry
    public virtual void WhenACreatureIsPlayed() { }

    //DeathRattle
    public virtual void WhenACreatureDies() { }

}
