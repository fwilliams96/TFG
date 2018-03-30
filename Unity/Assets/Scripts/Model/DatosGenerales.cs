using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DatosGenerales: MonoBehaviour 
{
    [Header("Players")]
    public Jugador TopPlayer;
    public Jugador LowPlayer;
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
    public GameObject CardInventario;
    public GameObject CardPrefab;
    public GameObject CardPrefabPC;
    public GameObject CriaturaPrefab;
    public GameObject CriaturaPrefabPC;
    public GameObject MagicaPrefab;
    public GameObject DamageEffectPrefab;
    public GameObject ExplosionPrefab;
    [Header("Other")]
    public Button EndTurnButton;
    public GameObject GameOverPanel;
    //public Sprite HeroPowerCrossMark;
    // SINGLETON
    public static DatosGenerales Instance;

    void Awake()
    {
        Instance = this;
    }

}
