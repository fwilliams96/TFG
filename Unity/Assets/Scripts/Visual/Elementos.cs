using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elementos : MonoBehaviour {
    // Drag & Drop the vertical layout group here
    public UnityEngine.UI.GridLayoutGroup gridLayoutGroup;
    public List<GameObject> ListaElementos;
	public bool cartas;
	public bool consumible;
    // Use this for initialization
    void Start () {
       
		if (cartas) {
			RellenarConCartas ();
		} else {
			RellenarConItems ();
		}
    }

	private void RellenarConCartas(){
		List<System.Object> listaCartas = BaseDatos.Instance.Local.Cartas();
		GameObject elemento;
		foreach(Carta carta in listaCartas)
		{
			//elemento = Instantiate(DatosGenerales.Instance.CardInventario, transform.position, Quaternion.identity) as GameObject;
			CartaAsset asset = carta.assetCarta;
			float progresoTrebol = carta.Progreso.Material;
			float progresoPocion = carta.Progreso.Pocion;
			elemento = Instantiate(DatosGenerales.Instance.CardInventario, transform);
			if(consumible)
				elemento.tag = "CartaConsumible";
			elemento.GetComponent<BoxCollider2D> ().size = gridLayoutGroup.cellSize;
			IDHolder id = elemento.AddComponent<IDHolder>();
			id.UniqueID = carta.ID;
			OneCardManager manager = elemento.GetComponent<OneCardManager>();
			manager.CartaAsset = asset;
			manager.PorcentajeProgresoTrebol = progresoTrebol;
			manager.PorcentajeProgresoPocion = progresoPocion;
			manager.LeerDatos();

			ListaElementos.Add(elemento);
			elemento.transform.SetParent (gridLayoutGroup.gameObject.transform);
			//elemento.GetComponent<BoxCollider2D> ().size = new Vector2 (elemento.GetComponent<RectTransform> ().rect.width, elemento.GetComponent<RectTransform> ().rect.height);
		}
	}

	private void RellenarConItems(){
		List<System.Object> listaCartas = BaseDatos.Instance.Local.Items();
		GameObject elemento;
		foreach(Item item in listaCartas)
		{
			//elemento = Instantiate(DatosGenerales.Instance.CardInventario, transform.position, Quaternion.identity) as GameObject;
			elemento = Instantiate(DatosGenerales.Instance.ItemInventario, transform);
			if(consumible)
				elemento.tag = "ItemConsumible";
			elemento.GetComponent<BoxCollider2D> ().size = gridLayoutGroup.cellSize;
			IDHolder id = elemento.AddComponent<IDHolder>();
			id.UniqueID = item.ID;
			OneItemManager manager = elemento.GetComponent<OneItemManager>();
			manager.Item = item;
			manager.LeerDatosItem();

			ListaElementos.Add(elemento);
			elemento.transform.SetParent (gridLayoutGroup.gameObject.transform);


		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
