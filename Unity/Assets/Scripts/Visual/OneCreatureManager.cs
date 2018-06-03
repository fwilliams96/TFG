using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OneCreatureManager : OneEnteManager 
{
    
    public override void LeerDatosAsset()
    {
        // Change the card graphic sprite
		CreatureGraphicImage.sprite = Resources.Load<Sprite>(CartaAsset.RutaImagenCarta);

		AttackText.text = CartaAsset.Ataque.ToString();
		if (Settings.Instance.Batalla.Equals (Settings.TIPO_NUMERO.ENTERO)) {
			HealthText.text = CartaAsset.Defensa.ToString ();
		}else {
			HealthText.text = Settings.ObtenerPorcentaje (CartaAsset.Defensa, CartaAsset.Defensa);
		}
			
        if (PreviewManager != null)
        {
            PreviewManager.CartaAsset = CartaAsset;
            PreviewManager.LeerDatosAsset();
        }
    }	

	public void HacerDaño(int daño, int vida)
    {
		if (vida > 0)
        {
			DamageEffect.CreateDamageEffect (transform.position, vida, daño);
			if (Settings.Instance.Batalla.Equals (Settings.TIPO_NUMERO.ENTERO)) {
				HealthText.text = (vida - daño).ToString ();	
			} else {
				HealthText.text = Settings.ObtenerPorcentaje ((vida-daño),CartaAsset.Defensa);
			}
        }
    }
}
