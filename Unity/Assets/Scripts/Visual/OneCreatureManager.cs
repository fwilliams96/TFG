using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OneCreatureManager : OneEnteManager 
{
    /// <summary>
    /// Lee los datos de la carta base.
    /// </summary>
    public override void LeerDatosAsset()
    {
        // Change the card graphic sprite
		CreatureGraphicImage.sprite = Resources.Load<Sprite>(CartaAsset.RutaImagenCarta);

		AttackText.text = CartaAsset.Ataque.ToString();
		if (ConfiguracionUsuario.Instance.ConfiguracionBatalla.Equals (ConfiguracionUsuario.TIPO_CONFIGURACION.ENTERO)) {
			HealthText.text = CartaAsset.Defensa.ToString ();
		}else {
			HealthText.text = ConfiguracionUsuario.ObtenerPorcentaje (CartaAsset.Defensa, CartaAsset.Defensa);
		}
			
        if (PreviewManager != null)
        {
            PreviewManager.CartaAsset = CartaAsset;
            PreviewManager.LeerDatosAsset();
        }
    }	

	/// <summary>
	/// Crea el daño visual en la criatura.
	/// </summary>
	/// <param name="daño">Daño.</param>
	/// <param name="vida">Vida.</param>
	public void HacerDaño(int daño, int vida)
    {
		if (vida > 0)
        {
			DamageEffect.CreateDamageEffect (transform.position, vida, daño);
			if (ConfiguracionUsuario.Instance.ConfiguracionBatalla.Equals (ConfiguracionUsuario.TIPO_CONFIGURACION.ENTERO)) {
				HealthText.text = (vida - daño).ToString ();	
			} else {
				HealthText.text = ConfiguracionUsuario.ObtenerPorcentaje ((vida-daño),CartaAsset.Defensa);
			}
        }
    }
}
