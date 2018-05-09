using UnityEngine;
using System.Collections;

//this class will take all decisions for AI. 

public class AITurnMaker: TurnMaker {

    /*public override void OnTurnStart()
    {
        base.OnTurnStart();
        // dispay a message that it is enemy`s turn
        new ShowMessageCommand("Enemy`s Turn!", 2.0f).AñadirAlaCola();
        p.DibujarCartaMazo();
        StartCoroutine(MakeAITurn());
    }

    // THE LOGIC FOR AI
    IEnumerator MakeAITurn()
    {
        bool strategyAttackFirst = false;
        if (Random.Range(0, 2) == 0)
            strategyAttackFirst = true;

        while (MakeOneAIMove(strategyAttackFirst))
        {
            yield return null;
        }

        InsertarRetraso(1f);

        Controlador.Instance.EndTurn();
    }

    bool MakeOneAIMove(bool attackFirst)
    {
        if (Comandas.Instance.ComandasDeDibujoCartaPendientes())
            return true;
        else if (attackFirst)
            return AttackWithACreature() || PlayACardFromHand();
        else
            return PlayACardFromHand() || AttackWithACreature();
    }

    bool PlayACardFromHand()
    {
        foreach (CardLogic c in p.CartasEnLaMano())
        {
            if (c.PuedeSerJugada)
            {
                if (c.AssetCarta.Defensa == 0)
                {
                    // code to play a spell from hand
                    // TODO: depending on the targeting options, select a random target.
                    if (c.AssetCarta.Targets == TargetingOptions.NoTarget)
                    {
                        p.JugarSpellMano(c, null);
                        InsertarRetraso(1.5f);
                        //Debug.Log("Card: " + c.ca.name + " can be played");
                        return true;
                    }                        
                }
                else
                {
                    // it is a creature card
                    p.JugarCartaMano(c, 0, true);
                    InsertarRetraso(1.5f);
                    return true;
                }

            }
            //Debug.Log("Card: " + c.ca.name + " can NOT be played");
        }
        return false;
    }

    bool AttackWithACreature()
    {
        foreach (CreatureLogic cl in p.CriaturasEnLaMesa())
        {
            if (cl.AtaquesRestantesEnTurno > 0)
            {
                // attack a random target with a creature
                if (p.otroJugador.NumCriaturasEnLaMesa() > 0)
                {
                    int index = Random.Range(0, p.otroJugador.NumCriaturasEnLaMesa());
                    CreatureLogic targetCreature = p.otroJugador.CriaturasEnLaMesa()[index];
                    cl.AtacarCriatura(targetCreature);
                }                    
                else
                    cl.GoFace();
                
                InsertarRetraso(1f);
                //Debug.Log("AI attacked with creature");
                return true;
            }
        }
        return false;
    }

    void InsertarRetraso(float delay)
    {
        new DelayCommand(delay).AñadirAlaCola();
    }*/

}
