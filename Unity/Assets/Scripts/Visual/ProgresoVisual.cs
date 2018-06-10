using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class ProgresoVisual : MonoBehaviour
{
	public float PorcentajeProgresoPiedra = 0;
	public float PorcentajeProgresoPocion = 0;
	public Slider ProgresoPiedra;
	public Slider ProgresoPocion;

	public void LeerProgreso()
	{
		if(ProgresoPiedra != null)
			ProgresoPiedra.value = PorcentajeProgresoPiedra/100f;
		if(ProgresoPocion != null)
			ProgresoPocion.value = PorcentajeProgresoPocion/100f;
	}

	public void AñadirItem(TipoItem tipoItem, int cantidad){
		if(tipoItem.Equals(TipoItem.Piedra))
			ProgresoPiedra.value += cantidad > 100? 1f: cantidad/100f;
		else
			ProgresoPocion.value += cantidad > 100? 1f: cantidad/100f;
		//if(ProgresoMaterial.value == 1f && ProgresoPocion.value == 1f)
			//GetComponent<OneCardManager> ().PuedeSerJugada = true;
	}

}
