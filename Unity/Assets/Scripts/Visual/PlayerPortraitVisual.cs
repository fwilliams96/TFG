using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerPortraitVisual : MonoBehaviour {

    public CharacterAsset charAsset;
    [Header("Text Component References")]
    public Text HealthText;
    [Header("Image References")]
    public Image ImagenPersonaje;
    public Image ImagenFondoPersonaje;

    void Awake()
    {
        if (charAsset != null)
            AplicarEstiloPersonajeAsset();
    }

    public void AplicarEstiloPersonajeAsset()
    {
		if (Settings.Instance.Batalla.Equals (Settings.TIPO_NUMERO.ENTERO)) {
			HealthText.text = charAsset.MaxHealth.ToString ();
		} else {
			HealthText.text = Settings.ObtenerPorcentaje (charAsset.MaxHealth, charAsset.MaxHealth);
		}
        ImagenPersonaje.sprite = charAsset.AvatarImage;
        ImagenFondoPersonaje.sprite = charAsset.AvatarBGImage;

        ImagenFondoPersonaje.color = charAsset.AvatarBGTint;

    }

    public void HacerDaño(int daño, int vida)
    {
        if (daño > 0)
        {
            DamageEffect.CreateDamageEffect(transform.position, vida,daño);
			if (Settings.Instance.Batalla.Equals (Settings.TIPO_NUMERO.ENTERO)) {
				HealthText.text = (vida-daño).ToString();
			} else {
				HealthText.text = Settings.ObtenerPorcentaje ((vida-daño),charAsset.MaxHealth);
			}

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
