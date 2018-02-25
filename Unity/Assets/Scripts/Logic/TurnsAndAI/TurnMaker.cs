using UnityEngine;
using System.Collections;

public abstract class TurnMaker : MonoBehaviour {

    protected Jugador p;

    void Awake()
    {
        p = GetComponent<Jugador>();
    }

    public virtual void OnTurnStart()
    {
        // add one mana crystal to the pool;
        //TODO cambiar
        p.OnTurnStart();
    }

}
