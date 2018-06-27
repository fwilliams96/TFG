using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PremioPartida : MonoBehaviour {

	public static PremioPartida Instance;
	public GameObject PremioPartidaPanel;
	public UnityEngine.UI.HorizontalLayoutGroup horizontalLayoutGroup; 
	public Text puntosExp;
	void Awake()
	{
		Instance = this;
		PremioPartidaPanel.SetActive(false);
	}

	/// <summary>
	/// Muestra los premios obtenidos en la partida para el jugador usuario.
	/// </summary>
	/// <param name="carta">Carta.</param>
	/// <param name="items">Items.</param>
	/// <param name="experiencia">Experiencia.</param>
	public void MostrarPremioPartida(Carta carta, List<Item> items,int experiencia){
		puntosExp.text = "ENHORABUENA, HAS OBTENIDO " + experiencia.ToString () + " PUNTOS DE EXP";
		PremioPartidaPanel.SetActive(true);
		if (carta != null) {
			CartaBase asset = carta.AssetCarta;
			int progresoTrebol = carta.Progreso.Piedra;
			int progresoPocion = carta.Progreso.Pocion;
			GameObject cartaGobj = Instantiate(ObjetosGenerales.Instance.CardInventario, transform);
			OneCardManager manager = cartaGobj.GetComponent<OneCardManager>();
			ProgresoVisual progreso = cartaGobj.GetComponent<ProgresoVisual>();
			manager.CartaAsset = asset;
			progreso.PorcentajeProgresoPiedra = progresoTrebol;
			progreso.PorcentajeProgresoPocion = progresoPocion;
			manager.LeerDatos();
			progreso.LeerProgreso ();
			cartaGobj.transform.SetParent (horizontalLayoutGroup.gameObject.transform);
		}
		GameObject itemGobj;
		foreach(Item item in items){
			itemGobj = Instantiate(ObjetosGenerales.Instance.ItemInventario, transform);
			OneItemManager manager = itemGobj.GetComponent<OneItemManager>();
			manager.Item = item;
			manager.LeerDatosItem();
			itemGobj.transform.SetParent (horizontalLayoutGroup.gameObject.transform);
		}
	}
		
}
