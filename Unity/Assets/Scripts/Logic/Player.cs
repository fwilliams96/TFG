using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour, ICharacter
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
    private Deck mazo;
    private Hand mano;
    private Table mesa;

    // a static array that will store both players, should always have 2 players
    public Players jugadores;
    #endregion Atributos

    // PROPERTIES 
    // this property is a part of interface ICharacter
    public int ID
    {
        get{ return PlayerID; }
    }

    // opponent player
    public Player otroJugador
    {
        get
        {
            if (jugadores[0] == this)
                return jugadores[1];
            else
                return jugadores[0];
        }
    }

    // total mana crystals that this player has this turn
    private int manaEnEsteTurno;
    public int ManaEnEsteTurno
    {
        get{ return manaEnEsteTurno;}
        set
        {
            manaEnEsteTurno = value;
            //PArea.ManaBar.TotalCrystals = manaThisTurn;
            new UpdateManaCrystalsCommand(this, manaEnEsteTurno, manaRestante).AñadirAlaCola();
        }
    }

    // full mana crystals available right now to play cards / use hero power 
    private int manaRestante;
    public int ManaRestante
    {
        get
        { return manaRestante;}
        set
        {
            manaRestante = value;
            //PArea.ManaBar.AvailableCrystals = manaLeft;
            new UpdateManaCrystalsCommand(this, ManaEnEsteTurno, manaRestante).AñadirAlaCola();
            //Debug.Log(ManaLeft);
            if (ControladorTurno.Instance.jugadorActual == this)
                MostrarCartasJugables();
        }
    }

    private int health;
    public int Vida
    {
        get { return health;}
        set
        {
            health = value;
            if (value <= 0)
                Morir(); 
        }
    }

    // CODE FOR EVENTS TO LET CREATURES KNOW WHEN TO CAUSE EFFECTS
    public delegate void VoidWithNoArguments();
    //public event VoidWithNoArguments CreaturePlayedEvent;
    //public event VoidWithNoArguments SpellPlayedEvent;
    //public event VoidWithNoArguments StartTurnEvent;
    public event VoidWithNoArguments EndTurnEvent;



    // ALL METHODS
    void Awake()
    {
        // find all scripts of type Player and store them in Players array
        // (we should have only 2 players in the scene)
        jugadores = Players.Instance;
        jugadores.Add(this);
        // obtain unique id from IDFactory
        PlayerID = IDFactory.GetUniqueID();
        mazo = GetComponentInChildren<Deck>();
        mano = GetComponentInChildren<Hand>();
        mesa = GetComponentInChildren<Table>();
    }

    public virtual void OnTurnStart()
    {
        // add one mana crystal to the pool;
        Debug.Log("In ONTURNSTART for "+ gameObject.name);
        ManaEnEsteTurno++;
        ManaRestante = ManaEnEsteTurno;
        foreach (CreatureLogic cl in mesa.CreaturesOnTable)
            cl.OnTurnStart();
    }

    public void OnTurnEnd()
    {
        if(EndTurnEvent != null)
            EndTurnEvent.Invoke();
        //ManaEnEsteTurno -= bonusManaThisTurn;
        //bonusManaThisTurn = 0;
        GetComponent<TurnMaker>().StopAllCoroutines();
    }

    // STUFF THAT OUR PLAYER CAN DO

    // get mana from coin or other spells 
    public void ConseguirManaExtra(int amount)
    {
        bonusManaThisTurn += amount;
        ManaEnEsteTurno += amount;
        ManaRestante += amount;
    }

    public void DibujarCartasMazo(int numCartas, bool fast = false)
    {
        for(int i = 0; i < numCartas; i++)
        {
            DibujarCartaMazo(fast);
        }
    }


    // draw a single card from the deck
    public void DibujarCartaMazo(bool fast = false)
    {
        if (mazo.cartas.Count > 0)
        {
            if (mano.CartasEnMano.Count < PArea.manoVisual.slots.Children.Length)
            {
                // 1) logic: add card to hand
                CardLogic newCard = new CardLogic(mazo.cartas[0]);
                newCard.jugador = this;
                mano.CartasEnMano.Insert(0, newCard);
                // Debug.Log(hand.CardsInHand.Count);
                // 2) logic: remove the card from the deck
                mazo.cartas.RemoveAt(0);
                // 2) create a command
                new DrawACardCommand(mano.CartasEnMano[0], this, fast, fromDeck: true).AñadirAlaCola(); 
            }
        }
        else
        {
            // there are no cards in the deck, take fatigue damage.
        }
       
    }

    // get card NOT from deck (a token or a coin)
    //TODO
    public void DibujarCartaFueraMazo(CardAsset assetCarta)
    {
        if (mano.CartasEnMano.Count < PArea.manoVisual.slots.Children.Length)
        {
            // 1) logic: add card to hand
            CardLogic newCard = new CardLogic(assetCarta);
            newCard.jugador = this;
            mano.CartasEnMano.Insert(0, newCard);
            // 2) send message to the visual Deck
            new DrawACardCommand(mano.CartasEnMano[0], this, fast: true, fromDeck: false).AñadirAlaCola(); 
        }
        // no removal from deck because the card was not in the deck
    }

    // 2 METHODS FOR PLAYING SPELLS
    // 1st overload - takes ids as arguments
    // it is cnvenient to call this method from visual part
    public void JugarSpellMano(int idCartaSpell, int idObjetivo)
    {
        // TODO: !!!
        // if TargetUnique ID < 0 , for example = -1, there is no target.
        if (idObjetivo < 0)
            JugarSpellMano(Recursos.CartasCreadasEnElJuego[idCartaSpell], null);
        //TODO estas dos siguientes condiciones sobraran puesto que no se podrá ir de cara al personaje
        else if (idObjetivo == ID)
        {
            JugarSpellMano(Recursos.CartasCreadasEnElJuego[idCartaSpell], this);
        }
        else if (idObjetivo == otroJugador.ID)
        {
            JugarSpellMano(Recursos.CartasCreadasEnElJuego[idCartaSpell], this.otroJugador);
        }
        else
        {
            // target is a creature
            JugarSpellMano(Recursos.CartasCreadasEnElJuego[idCartaSpell], Recursos.CriaturasCreadasEnElJuego[idObjetivo]);
        }
          
    }

    // 2nd overload - takes CardLogic and ICharacter interface - 
    // this method is called from Logic, for example by AI
    public void JugarSpellMano(CardLogic playedCard, ICharacter target)
    {
        ManaRestante -= playedCard.CosteManaActual;
        // cause effect instantly:
        if (playedCard.efecto != null)
            playedCard.efecto.ActivateEffect(playedCard.assetCarta.specialSpellAmount, target);
        else
        {
            Debug.LogWarning("No effect found on card " + playedCard.assetCarta.name);
        }
        // no matter what happens, move this card to PlayACardSpot
        new PlayASpellCardCommand(this, playedCard).AñadirAlaCola();
        // remove this card from hand
        mano.CartasEnMano.Remove(playedCard);
        // check if this is a creature or a spell
    }

    // METHODS TO PLAY CREATURES 
    // 1st overload - by ID
    public void JugarCartaMano(int UniqueID, int tablePos, bool posicionAtaque)
    {
        JugarCartaMano(Recursos.CartasCreadasEnElJuego[UniqueID], tablePos, posicionAtaque);
    }

    // 2nd overload - by logic units
    public void JugarCartaMano(CardLogic cartaJugada, int tablePos, bool posicionAtaque)
    {
        // Debug.Log(ManaLeft);
        // Debug.Log(playedCard.CurrentManaCost);
        ManaRestante -= cartaJugada.CosteManaActual;
        // Debug.Log("Mana Left after played a creature: " + ManaLeft);
        // create a new creature object and add it to Table
        CreatureLogic newCreature = new CreatureLogic(this, cartaJugada.assetCarta);
        mesa.CreaturesOnTable.Insert(tablePos, newCreature);
        // no matter what happens, move this card to PlayACardSpot
        new PlayACreatureCommand(cartaJugada, this, tablePos, posicionAtaque, newCreature.idCriatura).AñadirAlaCola();
        //causa battlecry effect
        if (newCreature.efecto != null)
            newCreature.efecto.WhenACreatureIsPlayed();
        // remove this card from hand
        mano.CartasEnMano.Remove(cartaJugada);
        MostrarCartasJugables();
    }

    public void Morir()
    {
        // game over
        // block both players from taking new moves 
        PArea.ControlActivado = false;
        otroJugador.PArea.ControlActivado = false;
        ControladorTurno.Instance.StopTheTimer();
        new GameOverCommand(this).AñadirAlaCola();
    }

    // METHOD TO SHOW GLOW HIGHLIGHTS
    public void MostrarCartasJugables(bool quitarTodasRemarcadas = false)
    {
        //Debug.Log("HighlightPlayable remove: "+ removeAllHighlights);
        foreach (CardLogic cl in mano.CartasEnMano)
        {
            GameObject g = IDHolder.GetGameObjectWithID(cl.idCarta);
            if (g!=null)
                g.GetComponent<OneCardManager>().PuedeSerJugada = (cl.CosteManaActual <= ManaRestante) && !quitarTodasRemarcadas;
        }

        foreach (CreatureLogic crl in mesa.CreaturesOnTable)
        {
            GameObject g = IDHolder.GetGameObjectWithID(crl.idCriatura);
            if(g!= null)
                g.GetComponent<OneCreatureManager>().PuedeAtacar = (crl.AtaquesRestantesEnTurno > 0) && !quitarTodasRemarcadas;
        }   
    }

    // START GAME METHODS
    //TODO
    public void LeerInformacionPersonajeAsset()
    {
        Vida = charAsset.MaxHealth;
        // change the visuals for portrait, hero power, etc...
        PArea.Personaje.charAsset = charAsset;
        PArea.Personaje.AplicarEstiloPersonajeAsset();
    }

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
        if (Input.GetKeyDown(KeyCode.D))
            DibujarCartaMazo();
    }

    public void InicializarValores()
    {
        ManaEnEsteTurno = 0;
        ManaRestante = 0;
        LeerInformacionPersonajeAsset();
        TransmitirInformacionAcercaJugadorVisual();
        PArea.mazoVisual.CartasEnMazo = mazo.cartas.Count;
        // move both portraits to the center
        PArea.Personaje.transform.position = PArea.PosicionInicialPersonaje.position;
    }

    public int NumCriaturasEnLaMesa()
    {
        return mesa.CreaturesOnTable.Count;
    }

    public void EliminarCriaturaMesa(CreatureLogic criatura)
    {
        mesa.CreaturesOnTable.Remove(criatura);
    }

    public CreatureLogic[] CriaturasEnLaMesa()
    {
        return mesa.CreaturesOnTable.ToArray();
    }

    public int NumCartasMano()
    {
        return mano.CartasEnMano.Count;
    }

    public CardLogic[] CartasEnLaMano()
    {
        return mano.CartasEnMano.ToArray();
    }
}
