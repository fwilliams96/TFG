using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TargetingOptions
{
    NoTarget,
    AllCreatures, 
    EnemyCreatures,
    YourCreatures, 
    AllCharacters, 
    EnemyCharacters,
    YourCharacters
}

public enum TipoCarta
{
    Fuego,
    Tierra,
    Electrica,
    Agua,
    Magica,
    Fusion,
    Ancestral
}

public class CardAsset : ScriptableObject 
{
    // this object will hold the info about the most general card
    [Header("General info")]
    public CharacterAsset AssetPersonaje;  // if this is null, it`s a neutral card
    [TextArea(2,3)]
    public string Descripcion;  // Description for spell or character
    public TipoCarta TipoDeCarta;
    public Sprite ImagenCarta;
    public int CosteMana;

    [Header("Carta no mágica")]
    public int Defensa;
    public int Ataque;
    public int AtaquesPorTurno = 1;
    public string Evolucion;
    public bool Taunt;
    public bool Charge;
    public string CreatureScriptName;
    public int specialCreatureAmount;

    [Header("Carta mágica")]
    public string SpellScriptName;
    public int specialSpellAmount;
    public TargetingOptions Targets;
}
