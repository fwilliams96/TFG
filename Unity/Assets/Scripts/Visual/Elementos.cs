using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elementos : MonoBehaviour {
    // Drag & Drop the vertical layout group here
    public UnityEngine.UI.GridLayoutGroup gridLayoutGroup;
	public enum TIPO_ELEMENTOS
	{
		CARTAS,
		CARTAS_FUERAMAZO,
		ITEMS,
		MAZO
	}
	public TIPO_ELEMENTOS tipoElementos;
	public enum TIPO_TAG{
		ORIGINAL,
		CONSUMIR,
		BARAJA
	}
	void Awake(){
	}
	public TIPO_TAG tipoTag;
    // Use this for initialization
	void Start () {
		
	}

	void OnEnable(){
		MostrarElementos ();
	}

	void OnDisable(){
	}

	public void MostrarElementos () {
		VaciarElementos ();
		if (tipoElementos.Equals (TIPO_ELEMENTOS.CARTAS)) {
			RellenarConCartas ();
		} else if (tipoElementos.Equals (TIPO_ELEMENTOS.ITEMS)) {
			RellenarConItems ();
		}else if(tipoElementos.Equals(TIPO_ELEMENTOS.CARTAS_FUERAMAZO)){
			RellenarConCartasFueraMazo ();
		} else {
			RellenarConMazo ();
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

	private void RellenarConCartasFueraMazo(){
		List<System.Object> listaCartas = ControladorMenu.Instance.RecogerElemento (tipoElementos);
		GameObject elemento;
		foreach(Carta carta in listaCartas)
		{
			elemento = CrearCarta (carta);
			if (tipoTag.Equals (TIPO_TAG.CONSUMIR))
				elemento.tag = "CartaConsumible";
			else if (tipoTag.Equals (TIPO_TAG.BARAJA)) {
				elemento.tag = "CartaFueraMazo";
			}
		}
	}

	private void RellenarConCartas(){
		gridLayoutGroup.cellSize = new Vector2 (270f, 400f);
		List<System.Object> listaCartas = ControladorMenu.Instance.RecogerElemento (tipoElementos);
		GameObject elemento;
		foreach(Carta carta in listaCartas)
		{
			elemento = CrearCarta (carta);
			if (tipoTag.Equals (TIPO_TAG.CONSUMIR))
				elemento.tag = "CartaConsumible";
			else if (tipoTag.Equals (TIPO_TAG.BARAJA)) {
				elemento.tag = "CartaFueraMazo";
			}
		}
	}

	private void RellenarConMazo(){
		List<System.Object> listaCartas = ControladorMenu.Instance.RecogerElemento (tipoElementos);
		GameObject elemento;
		foreach(Carta carta in listaCartas)
		{
			elemento = CrearCarta (carta);
			if (tipoTag.Equals (TIPO_TAG.BARAJA)) {
				elemento.tag = "CartaDentroMazo";
			}
		}
	}

	private void RellenarConItems(){
		if(tipoTag.Equals(TIPO_TAG.CONSUMIR))
			gridLayoutGroup.cellSize = new Vector2 (200f, 200f);
		else
			gridLayoutGroup.cellSize = new Vector2 (300f, 300f);
		List<System.Object> listaCartas = ControladorMenu.Instance.RecogerElemento (tipoElementos);
		GameObject elemento;
		foreach(Item item in listaCartas)
		{
			elemento = CrearItem(item);
			if(tipoTag.Equals(TIPO_TAG.CONSUMIR))
				elemento.tag = "ItemConsumible";
		}
	}
		
	private GameObject CrearItem(Item item){
		GameObject elemento = Instantiate(ObjetosGenerales.Instance.ItemInventario, transform);
		elemento.GetComponent<BoxCollider2D> ().size = gridLayoutGroup.cellSize;
		IDHolder id = elemento.AddComponent<IDHolder>();
		id.UniqueID = item.ID;
		OneItemManager manager = elemento.GetComponent<OneItemManager>();
		manager.Item = item;
		manager.LeerDatosItem();
		elemento.transform.SetParent (gridLayoutGroup.gameObject.transform);
		return elemento;
	}

	private GameObject CrearCarta(Carta carta){
		CartaAsset asset = carta.AssetCarta;
		int progresoPiedra = carta.Progreso.Piedra;
		int progresoPocion = carta.Progreso.Pocion;
		GameObject elemento = Instantiate(ObjetosGenerales.Instance.CardInventario, transform);
		elemento.GetComponent<BoxCollider2D> ().size = gridLayoutGroup.cellSize;
		IDHolder id = elemento.AddComponent<IDHolder>();
		id.UniqueID = carta.ID;
		OneCardManager manager = elemento.GetComponent<OneCardManager>();
		ProgresoVisual progreso = elemento.GetComponent<ProgresoVisual>();
		manager.CartaAsset = asset;

		progreso.PorcentajeProgresoPiedra = progresoPiedra;
		progreso.PorcentajeProgresoPocion = progresoPocion;
		manager.LeerDatos();
		progreso.LeerProgreso ();

		elemento.transform.SetParent (gridLayoutGroup.gameObject.transform);
		return elemento;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
