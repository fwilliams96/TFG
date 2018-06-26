using UnityEngine;
using System.Collections;
using DG.Tweening;

public class MagicEffectVisual : EnteVisual
{
	/// <summary>
	/// Pone boca arriba el ente magico.
	/// </summary>
    public void ColocarMagicaBocaArriba()
    {
		RotarObjetoEjeY(gameObject.transform.Find("Cuerpo").gameObject, 0, ConfiguracionUsuario.Instance.CardTransitionTime);
		if(this.GetComponents<AudioSource>()[2] != null && this.GetComponents<AudioSource>()[2].clip != null && ConfiguracionUsuario.Instance.Musica)
			this.GetComponents<AudioSource>()[2].Play();
		StartCoroutine (TiempoVisible ());
    }

	private IEnumerator TiempoVisible()
	{
		yield return new WaitForSeconds(1f);
		Controlador.Instance.MuerteEnte (GetComponent<IDHolder> ().UniqueID);
	}

	/// <summary>
	/// Coloca la magica boca abajo.
	/// </summary>
	/// <param name="tiempo">Tiempo.</param>
    public void ColocarMagicaBocaAbajo(float tiempo)
    {
        RotarObjetoEjeY(this.gameObject.transform.Find("Cuerpo").gameObject, 180, tiempo);
    }

}
