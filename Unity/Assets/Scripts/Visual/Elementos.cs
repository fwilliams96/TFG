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
		MostrarElementos ();
	}

	public void MostrarElementos () {
		VaciarElementos ();
		if (cartas) {
			RellenarConCartas ();
		} else {
			RellenarConItems ();
		}
    }

	private void VaciarElementos(){
		for(int i = 0; i < gridLayoutGroup.transform.childCount; i++){
			IDHolder.EliminarElemento (gridLayoutGroup.transform.GetChild(i).gameObject.GetComponent<IDHolder>());
			Destroy (gridLayoutGroup.transform.GetChild (i).gameObject);
		}
	}

	public void DeshabilitarColliderElementos(){
		for(int i = 0; i < gridLayoutGroup.transform.childCount; i++){
			gridLayoutGroup.transform.GetChild (i).gameObject.GetComponent<BoxCollider2D> ().enabled = false;
		}
	}

	public void HabilitarColliderElementos(){
		for(int i = 0; i < gridLayoutGroup.transform.childCount; i++){
			gridLayoutGroup.transform.GetChild (i).gameObject.GetComponent<BoxCollider2D> ().enabled = true;
		}
	}

	private void RellenarConCartas(){
		gridLayoutGroup.cellSize = new Vector2 (270f, 400f);
		List<System.Object> listaCartas = BaseDatos.Instance.Local.Cartas();
		GameObject elemento;
		foreach(Carta carta in listaCartas)
		{
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
		}
	}

	private void RellenarConItems(){
		if(consumible)
			gridLayoutGroup.cellSize = new Vector2 (200f, 200f);
		else
			gridLayoutGroup.cellSize = new Vector2 (300f, 300f);
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
