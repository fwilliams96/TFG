using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class ProgresoVisual : MonoBehaviour
{
	public int PorcentajeProgresoPiedra = 0;
	public int PorcentajeProgresoPocion = 0;
	public Slider ProgresoPiedra;
	public Text ProgresoPiedraText;
	public Slider ProgresoPocion;
	public Text ProgresoPocionText;

	public void LeerProgreso()
	{
		if (ProgresoPiedra != null) {
			ProgresoPiedra.value = PorcentajeProgresoPiedra > 100? 1f : PorcentajeProgresoPiedra/100f;
			AñadirCantidad (ProgresoPiedraText,PorcentajeProgresoPiedra);
		}
			
		if (ProgresoPocion != null) {
			ProgresoPocion.value = PorcentajeProgresoPocion > 100? 1f : PorcentajeProgresoPocion/100f;
			AñadirCantidad (ProgresoPocionText,PorcentajeProgresoPocion);
		}
			
	}

	private void AñadirCantidad(Text cantidadText, int cantidad){
		ConfiguracionUsuario settings = ConfiguracionUsuario.Instance;
		switch (settings.Items) {
		case ConfiguracionUsuario.TIPO_NUMERO.ENTERO:
			cantidadText.text = ConfiguracionUsuario.ObtenerEntero (cantidad);
			break;
		case ConfiguracionUsuario.TIPO_NUMERO.FRACCION:
			cantidadText.text = ConfiguracionUsuario.ObtenerFraccion (cantidad, 100);
			break;
		case ConfiguracionUsuario.TIPO_NUMERO.PORCENTAJE:
			cantidadText.text = ConfiguracionUsuario.ObtenerPorcentaje (cantidad, 100);
			break;
		}
	}

	public void AñadirItem(int tipoItem, int cantidad){
		if (tipoItem == 1)
			PorcentajeProgresoPiedra += cantidad;
		else
			PorcentajeProgresoPocion += cantidad;
		LeerProgreso();
	}

}
