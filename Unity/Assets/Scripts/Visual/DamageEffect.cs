using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// This class will show damage dealt to creatures or payers
/// </summary>

public class DamageEffect : MonoBehaviour {

    // an array of sprites with different blood splash graphics
    public Sprite[] Splashes;

    // a UI image to show the blood splashes
    public Image DamageImage;

    // CanvasGropup should be attached to the Canvas of this damage effect
    // It is used to fade away the alpha value of this effect
    public CanvasGroup cg;

    // The text component to show the amount of damage taken by target like: "-2"
    public Text AmountText;

    void Awake()
    {
        // pick a random image
        DamageImage.sprite = Splashes[Random.Range(0, Splashes.Length)];  
    }

    // A Coroutine to control the fading of this damage effect
    private IEnumerator ShowDamageEffect()
    {
        // make this effect non-transparent
        cg.alpha = 1f;
        // wait for 1 second before fading
        yield return new WaitForSeconds(3.5f);
        // gradually fade the effect by changing its alpha value
        while (cg.alpha > 0)
        {
            cg.alpha -= 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
        // after the effect is shown it gets destroyed.
        Destroy(this.gameObject);
    }
    /// <summary>
    /// Creates the damage effect.
    /// This is a static method, so it should be called like this: DamageEffect.CreateDamageEffect(transform.position, 5);
    /// </summary>
    /// <param name="position">Position.</param>
    /// <param name="amount">Amount.</param>
   
    public static void CreateDamageEffect(Vector3 position, int health, int amount)
    {
		GameObject newDamageEffect = InstanciarPrefab (position);
        // Get DamageEffect component in this new game object
        DamageEffect de = newDamageEffect.GetComponent<DamageEffect>();
        // Change the amount text to reflect the amount of damage dealt
		ConfiguracionUsuario settings = ConfiguracionUsuario.Instance;

		if (settings.ConfiguracionBatalla.Equals (ConfiguracionUsuario.TIPO_CONFIGURACION.ENTERO))
			de.AmountText.text = "-" + amount.ToString ();
		//de.AmountText.text = health.ToString()+"-"+amount.ToString();
		else if (settings.ConfiguracionBatalla.Equals (ConfiguracionUsuario.TIPO_CONFIGURACION.FRACCION)) {
			de.AmountText.text = "-"+ConfiguracionUsuario.ObtenerFraccion(amount,health)+"x"+health.ToString();
		} else {
			de.AmountText.text = "-" + ConfiguracionUsuario.ObtenerPorcentaje (amount, health);
		}



			//de.AmountText.text = health.ToString()+"-"+Settings.ObtenerFraccion(amount,health)+"x"+health.ToString();
        // start a coroutine to fade away and delete this effect after a certain time
        de.StartCoroutine(de.ShowDamageEffect());
        //Comandas.Instance.CompletarEjecucionComanda();
    }

	public static void CreateDamageEffect(Vector3 position)
	{
		GameObject newDamageEffect = InstanciarPrefab (position);
		// Get DamageEffect component in this new game object
		DamageEffect de = newDamageEffect.GetComponent<DamageEffect>();
		// Change the amount text to reflect the amount of damage dealt
		de.AmountText.text = "";
		// start a coroutine to fade away and delete this effect after a certain time
		de.StartCoroutine(de.ShowDamageEffect());
		//Comandas.Instance.CompletarEjecucionComanda();
	}

	private static GameObject InstanciarPrefab(Vector3 position){
		// Instantiate a DamageEffect from prefab
		GameObject newDamageEffect = new GameObject();
		//TODO = GameObject.Instantiate(GlobalSettings.Instance.DamageEffectPrefab, position, Quaternion.identity) as GameObject;
		newDamageEffect = GameObject.Instantiate(ObjetosGenerales.Instance.DamageEffectPrefab, position, Quaternion.identity) as GameObject;
		return newDamageEffect;
	}
}
