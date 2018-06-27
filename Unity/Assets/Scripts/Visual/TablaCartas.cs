using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TablaCartas : MonoBehaviour {

	public UnityEngine.UI.GridLayoutGroup gridLayoutGroup;
		
	// MONOBEHAVIOUR SCRIPTS (mouse over collider detection)
	void Awake()
	{
	}

	/// <summary>
	/// Añade de forma visual la carta al scroll view adecuado.
	/// </summary>
	/// <param name="cartaGobj">Carta gobj.</param>
	public void AñadirCarta(GameObject cartaGobj){
		cartaGobj.tag = cartaGobj.tag.Equals("CartaDentroMazo") ? "CartaFueraMazo": "CartaDentroMazo";
		cartaGobj.transform.SetParent (gridLayoutGroup.gameObject.transform);
	}

	public int NumElementos(){
		return gridLayoutGroup.transform.childCount;
	}
		
	/// <summary>
	/// Devuelve los elementos que se encuentran en el scroll view actual.
	/// </summary>
	/// <returns>The elementos.</returns>
	public List<GameObject> ObtenerElementos(){
		List<GameObject> cartas = new List<GameObject> ();
		for (int i = 0; i < gridLayoutGroup.transform.childCount; i++) {
			cartas.Add (gridLayoutGroup.transform.GetChild (i).gameObject);
		}
		return cartas;
	}


	/// <summary>
	/// Callback recibido por el boton quitar o añadir.
	/// </summary>
	public void Accion(){
		if (tag.Equals ("TablaCartas"))
			ControladorMenu.Instance.AñadirElementoMazo (AccionBaraja.Instance.ElementoActual);
		else
			ControladorMenu.Instance.AñadirElementoCartas (AccionBaraja.Instance.ElementoActual);
		//HabilitarColliderElementos ();
	}

	public void CerrarAccion(){
		AccionBaraja.Instance.CerrarAccion ();
		//HabilitarColliderElementos ();
	}

	/// <summary>
	/// Muestra el mensaje de accion de la carta en funcion si es del mazo o no.
	/// </summary>
	/// <param name="carta">Carta.</param>
	public void MostrarAccion(GameObject carta){
		
		AccionBaraja.Instance.CerrarAccion ();
		if (tag.Equals ("TablaCartas"))
			AccionBaraja.Instance.MostrarAccion (carta,AccionBaraja.TIPO_ACCION.USAR, Accion);
		else
			AccionBaraja.Instance.MostrarAccion (carta,AccionBaraja.TIPO_ACCION.QUITAR, Accion);
		//DeshabilitarColliderElementos ();
	}

	private void DeshabilitarColliderElementos(){
		for(int i = 0; i < gridLayoutGroup.transform.childCount; i++){
			gridLayoutGroup.transform.GetChild (i).gameObject.GetComponent<BoxCollider2D> ().enabled = false;
		}
	}

	private void HabilitarColliderElementos(){
		for(int i = 0; i < gridLayoutGroup.transform.childCount; i++){
			gridLayoutGroup.transform.GetChild (i).gameObject.GetComponent<BoxCollider2D> ().enabled = true;
		}
	}
}
