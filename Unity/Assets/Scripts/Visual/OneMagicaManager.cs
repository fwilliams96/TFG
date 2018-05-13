using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OneMagicaManager : OneEnteManager 
{
    
    public override void LeerDatosAsset()
    {
        // Change the card graphic sprite
		CreatureGraphicImage.sprite = Resources.Load<Sprite>(CartaAsset.RutaImagenCarta);

        if (PreviewManager != null)
        {
            PreviewManager.CartaAsset = CartaAsset;
            PreviewManager.LeerDatosAsset();
        }
    }	

	public void HacerDaño()
    {
		DamageEffect.CreateDamageEffect (transform.position);
    }
}
