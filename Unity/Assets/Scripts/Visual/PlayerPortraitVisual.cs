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
    void Awake()
    {
        if (charAsset != null)
            AplicarEstiloPersonajeAsset();
    }

    public void AplicarEstiloPersonajeAsset()
    {
		if (ConfiguracionUsuario.Instance.ConfiguracionBatalla.Equals (ConfiguracionUsuario.TIPO_CONFIGURACION.ENTERO)) {
			HealthText.text = charAsset.MaxHealth.ToString ();
		} else {
			HealthText.text = ConfiguracionUsuario.ObtenerPorcentaje (charAsset.MaxHealth, charAsset.MaxHealth);
		}
        ImagenPersonaje.sprite = charAsset.AvatarImage;
        //ImagenFondoPersonaje.sprite = charAsset.AvatarBGImage;

        //ImagenFondoPersonaje.color = charAsset.AvatarBGTint;

    }

	public void AumentarVida(int vidaActual, int vidaDespues){
		StartCoroutine (AumentarVidaProgresivamente(vidaActual,vidaDespues));

	}

	IEnumerator AumentarVidaProgresivamente(int vidaActual, int vidaDespues)
	{
		vidaActual += 1;
		while (vidaActual <= vidaDespues) {
			if (ConfiguracionUsuario.Instance.ConfiguracionBatalla.Equals (ConfiguracionUsuario.TIPO_CONFIGURACION.ENTERO)) {
				HealthText.text = vidaActual.ToString ();
			} else {
				HealthText.text = ConfiguracionUsuario.ObtenerPorcentaje (vidaActual, charAsset.MaxHealth);
			}
			yield return new WaitForSeconds(0.02f);
			vidaActual += 1;
		}
		Comandas.Instance.CompletarEjecucionComanda();
	}

    public void HacerDaño(int daño, int vida)
    {
        if (daño > 0)
        {
            DamageEffect.CreateDamageEffect(transform.position, vida,daño);
			if (ConfiguracionUsuario.Instance.ConfiguracionBatalla.Equals (ConfiguracionUsuario.TIPO_CONFIGURACION.ENTERO)) {
				HealthText.text = (vida-daño).ToString();
			} else {
				HealthText.text = ConfiguracionUsuario.ObtenerPorcentaje ((vida-daño),charAsset.MaxHealth);
			}

        }
    }

    public void Explotar()
    {
        Instantiate(ObjetosGenerales.Instance.ExplosionPrefab, transform.position, Quaternion.identity);
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
