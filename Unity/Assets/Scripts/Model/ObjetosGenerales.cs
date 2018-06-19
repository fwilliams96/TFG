using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ObjetosGenerales: MonoBehaviour 
{
    [Header("Prefabs and Assets")]
	public GameObject ItemInventario;
    public GameObject CardInventario;
    public GameObject CardPrefab;
    public GameObject CriaturaPrefab;
    public GameObject MagicaPrefab;
    public GameObject DamageEffectPrefab;
    public GameObject ExplosionPrefab;
    // SINGLETON
    public static ObjetosGenerales Instance = null;

    void Awake()
    {
		if (Instance == null) {
			DontDestroyOnLoad(this.gameObject);
			Instance = this;
		}
    }

}
