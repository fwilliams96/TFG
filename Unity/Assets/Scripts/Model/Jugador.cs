using UnityEngine;
using System.Collections;

public class Jugador : MonoBehaviour, ICharacter
{

    #region Atributos
    // PUBLIC FIELDS
    // int ID that we get from ID factory
    public int PlayerID;
    // a Character Asset that contains data about this Hero
    public CharacterAsset charAsset;
    // a script with references to all the visual game objects for this player
    public PlayerArea PArea;
    // this value used exclusively for our coin spell
    private int bonusManaThisTurn = 0;

    // REFERENCES TO LOGICAL STUFF THAT BELONGS TO THIS PLAYER
    private Mazo mazo;
    private Mano mano;
    private Tablero mesa;

    #endregion Atributos
    #region Getters/Setters
    // PROPERTIES 
    // this property is a part of interface ICharacter
    public int ID
    {
        get { return PlayerID; }
    }

    // total mana crystals that this player has this turn
    private int manaEnEsteTurno;
    public int ManaEnEsteTurno
    {
        get { return manaEnEsteTurno; }
        set
        {
            manaEnEsteTurno = value;
        }
    }

    // full mana crystals available right now to play cards / use hero power 
    private int manaRestante;
    public int ManaRestante
    {
        get
        { return manaRestante; }
        set
        {
            manaRestante = value;
        }
    }

    private int health;
    public int Defensa
    {
        get { return health; }
        set
        {
            health = value;
        }
    }
    #endregion

    // CODE FOR EVENTS TO LET CREATURES KNOW WHEN TO CAUSE EFFECTS
    public delegate void VoidWithNoArguments();
    //public event VoidWithNoArguments CreaturePlayedEvent;
    //public event VoidWithNoArguments SpellPlayedEvent;
    //public event VoidWithNoArguments StartTurnEvent;
    public event VoidWithNoArguments EndTurnEvent;

    // ALL METHODS
    void Awake()
    {
        // obtain unique id from IDFactory
        PlayerID = IDFactory.GetUniqueID();
        mazo = GetComponentInChildren<Mazo>();
        mano = GetComponentInChildren<Mano>();
        mesa = GetComponentInChildren<Tablero>();
    }

    //TODO get mana from coin or other spells 
    public void ConseguirManaExtra(int amount)
    {
        bonusManaThisTurn += amount;
        ManaEnEsteTurno += amount;
        ManaRestante += amount;
    }

    //TODO ver si esta funcion seguira existiendo
    public void LeerInformacionPersonajeAsset()
    {
        Defensa = charAsset.MaxHealth;
        // change the visuals for portrait, hero power, etc...
        PArea.Personaje.charAsset = charAsset;
        PArea.Personaje.AplicarEstiloPersonajeAsset();
    }

    //TODO ver si esta funcion seguira aqui
    public void TransmitirInformacionAcercaJugadorVisual()
    {

        PArea.Personaje.gameObject.AddComponent<IDHolder>().UniqueID = PlayerID;
        if (GetComponent<TurnMaker>() is AITurnMaker)
        {
            // turn off turn making for this character
            PArea.PermitirControlJugador = false;
        }
        else
        {
            // allow turn making for this character
            PArea.PermitirControlJugador = true;
        }
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.D))
        // DibujarCartaMazo();
    }

    public void InicializarValores()
    {
        ManaEnEsteTurno = 0;
        ManaRestante = 0;
        LeerInformacionPersonajeAsset();
        TransmitirInformacionAcercaJugadorVisual();
        //TO
        PArea.mazoVisual.CartasEnMazo = mazo.CartasEnMazo.Count;
        // move both portraits to the center
        PArea.Personaje.transform.position = PArea.PosicionInicialPersonaje.position;
    }

    public void Morir() { }

    public void AñadirEnteMesa(int posicionMesa, Ente ente)
    {
        mesa.EntesEnTablero.Insert(posicionMesa, ente);
    }

    public void AñadirCartaMano(int posicionMano, Carta carta)
    {
        mano.CartasEnMano.Insert(posicionMano, carta);
    }

    public void AñadirCartaMazo(int posicionMazo, CardAsset carta)
    {
        mazo.CartasEnMazo.Insert(posicionMazo, carta);
    }

    public void EliminarEnteMesa(Ente ente)
    {
        mesa.EntesEnTablero.Remove(ente);
    }

    public void EliminarCartaMano(Carta carta)
    {
        mano.CartasEnMano.Remove(carta);
    }

    public void EliminarCartaMazo(int pos)
    {
        mazo.CartasEnMazo.RemoveAt(pos);
    }

    public Ente[] EntesEnLaMesa()
    {
        return mesa.EntesEnTablero.ToArray();
    }

    public Carta[] CartasEnLaMano()
    {
        return mano.CartasEnMano.ToArray();
    }

    public CardAsset[] CartasEnElMazo()
    {
        return mazo.CartasEnMazo.ToArray();
    }

    public int NumEntesEnLaMesa()
    {
        return mesa.EntesEnTablero.Count;
    }

    public int NumCartasMano()
    {
        return mano.CartasEnMano.Count;
    }

    public int NumCartasMazo()
    {
        return mazo.CartasEnMazo.Count;
    }

    //TODO
    public virtual void OnTurnStart()
    {
        // add one mana crystal to the pool;
        Debug.Log("In ONTURNSTART for " + gameObject.name);
        ManaEnEsteTurno++;
        ManaRestante = ManaEnEsteTurno;
        foreach (Ente cl in mesa.EntesEnTablero)
            cl.OnTurnStart();
    }
    //TODO
    public void OnTurnEnd()
    {
        if (EndTurnEvent != null)
            EndTurnEvent.Invoke();
        //ManaEnEsteTurno -= bonusManaThisTurn;
        //bonusManaThisTurn = 0;
        GetComponent<TurnMaker>().StopAllCoroutines();
    }
}
