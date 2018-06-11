using UnityEngine;
using System.Collections;
using DG.Tweening;

public class MagicEffectVisual : EnteVisual
{

    public void ColocarMagicaBocaArriba()
    {
		RotarObjetoEjeY(gameObject.transform.Find("Cuerpo").gameObject, 0, ConfiguracionUsuario.Instance.CardTransitionTime);
		this.GetComponents<AudioSource>()[1].Play();
		StartCoroutine (TiempoVisible ());
    }

	private IEnumerator TiempoVisible()
	{
		yield return new WaitForSeconds(1f);
		Controlador.Instance.MuerteEnte (GetComponent<IDHolder> ().UniqueID);
	}

    public void ColocarMagicaBocaAbajo(float tiempo)
    {
        RotarObjetoEjeY(this.gameObject.transform.Find("Cuerpo").gameObject, 180, tiempo);
    }

}
