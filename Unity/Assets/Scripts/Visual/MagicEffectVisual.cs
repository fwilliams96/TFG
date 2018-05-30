using UnityEngine;
using System.Collections;
using DG.Tweening;

public class MagicEffectVisual : EnteVisual
{

    public void ColocarMagicaBocaArriba()
    {
		RotarObjetoEjeY(gameObject.transform.Find("Cuerpo").gameObject, 0, Settings.Instance.CardTransitionTime);
		StartCoroutine (MuerteMagica ());
    }

	private IEnumerator MuerteMagica()
	{
		yield return new WaitForSeconds(3f);
		/*for(int i=0; i < 5; i++)
		{
			yield return new WaitForSeconds(0.05f);
		}*/
		Controlador.Instance.MuerteEnte (GetComponent<IDHolder> ().UniqueID);
	}

    public void ColocarMagicaBocaAbajo(float tiempo)
    {
        RotarObjetoEjeY(this.gameObject.transform.Find("Cuerpo").gameObject, 180, tiempo);
    }

}
