using UnityEngine;
using System.Collections;
using DG.Tweening;

public class MagicEffectVisual : EnteVisual
{

    public void ColocarMagicaBocaArriba()
    {
		RotarObjetoEjeY(gameObject.transform.Find("Cuerpo").gameObject, 0, ConfiguracionUsuario.Instance.CardTransitionTime);
		this.GetComponents<AudioSource>()[1].Play();
		StartCoroutine (MuerteMagica ());
    }

	private IEnumerator MuerteMagica()
	{
		yield return new WaitForSeconds(1.5f);
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
