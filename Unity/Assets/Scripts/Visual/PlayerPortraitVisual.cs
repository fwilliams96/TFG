using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerPortraitVisual : MonoBehaviour {

    // TODO : get ID from players when game starts

    //public GameObject Explosion;
    public CharacterAsset charAsset;
    [Header("Text Component References")]
    //public Text NameText;
    public Text HealthText;
    [Header("Image References")]
    //public Image HeroPowerIconImage;
    //public Image HeroPowerBackgroundImage;
    public Image ImagenPersonaje;
    public Image ImagenFondoPersonaje;

    void Awake()
    {
        if (charAsset != null)
            AplicarEstiloPersonajeAsset();
    }

    public void AplicarEstiloPersonajeAsset()
    {
        HealthText.text = charAsset.MaxHealth.ToString();
        ImagenPersonaje.sprite = charAsset.AvatarImage;
        ImagenFondoPersonaje.sprite = charAsset.AvatarBGImage;

        ImagenFondoPersonaje.color = charAsset.AvatarBGTint;

    }

    public void HacerDaño(int daño, int vida)
    {
        if (daño > 0)
        {
            DamageEffect.CreateDamageEffect(transform.position, vida,daño);
			HealthText.text = (vida-daño).ToString();
        }
    }

    public void Explotar()
    {
        Instantiate(DatosGenerales.Instance.ExplosionPrefab, transform.position, Quaternion.identity);
		Sequence s = DOTween.Sequence();
		s.PrependInterval(2f);
		s.OnComplete(() => Comandas.Instance.CompletarEjecucionComanda ());
	}

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }



}
