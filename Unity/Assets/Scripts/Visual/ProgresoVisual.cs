using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class ProgresoVisual : MonoBehaviour
{
	public Slider ProgresoMaterial;
	public Slider ProgresoPocion;

	public void AñadirItem(TipoItem tipoItem, int cantidad){
		if(tipoItem.Equals(TipoItem.Material))
			ProgresoMaterial.value += cantidad > 100? 1f: cantidad/100f;
		else
			ProgresoPocion.value += cantidad > 100? 1f: cantidad/100f;
		if(ProgresoMaterial.value == 100 && ProgresoPocion.value == 100)
			GetComponent<OneCardManager> ().PuedeSerJugada = true;
	}

}
