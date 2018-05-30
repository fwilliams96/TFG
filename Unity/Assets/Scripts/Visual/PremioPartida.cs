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

	public void MostrarPremioPartida(Carta carta, List<Item> items,int experiencia){
		puntosExp.text = "ENHORABUENA, HAS OBTENIDO " + experiencia.ToString () + " PUNTOS DE EXP";
		PremioPartidaPanel.SetActive(true);
		if (carta != null) {
			CartaAsset asset = carta.AssetCarta;
			float progresoTrebol = carta.Progreso.Material;
			float progresoPocion = carta.Progreso.Pocion;
			GameObject cartaGobj = Instantiate(DatosGenerales.Instance.CardInventario, transform);
			OneCardManager manager = cartaGobj.GetComponent<OneCardManager>();
			manager.CartaAsset = asset;
			manager.PorcentajeProgresoTrebol = progresoTrebol;
			manager.PorcentajeProgresoPocion = progresoPocion;
			manager.LeerDatos();
			cartaGobj.transform.SetParent (horizontalLayoutGroup.gameObject.transform);
		}
		GameObject itemGobj;
		foreach(Item item in items){
			itemGobj = Instantiate(DatosGenerales.Instance.ItemInventario, transform);
			OneItemManager manager = itemGobj.GetComponent<OneItemManager>();
			manager.Item = item;
			manager.LeerDatosItem();
			itemGobj.transform.SetParent (horizontalLayoutGroup.gameObject.transform);
		}
	}
		
}
