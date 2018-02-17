using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DatosGenerales: MonoBehaviour 
{
    [Header("Players")]
    public Player TopPlayer;
    public Player LowPlayer;
    [Header("Colors")]
    public Color32 CardBodyStandardColor;
    public Color32 CardRibbonsStandardColor;
    public Color32 CardGlowColor;
    [Header("Numbers and Values")]
    public float CardPreviewTime = 1f;
    public float CardTransitionTime= 1f;
    public float CardPreviewTimeFast = 0.2f;
    public float CardTransitionTimeFast = 0.5f;
    public int NumMaximoCriaturasMesa = 7;
    [Header("Prefabs and Assets")]
    public GameObject NoTargetSpellCardPrefab;
    public GameObject TargetedSpellCardPrefab;
    public GameObject CreatureCardPrefab;
    public GameObject CreaturePrefab;
    public GameObject CriaturaPrefab;
    public GameObject DamageEffectPrefab;
    public GameObject ExplosionPrefab;
    [Header("Other")]
    public Button EndTurnButton;
    public CardAsset CoinCard;
    public GameObject GameOverPanel;
    //public Sprite HeroPowerCrossMark;
    // SINGLETON
    public static DatosGenerales Instance;

    void Awake()
    {
        Instance = this;
    }

}
