using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TablaCartas : MonoBehaviour {

	private BoxCollider2D col;
	private bool cursorSobreEstaTabla = false;
	public UnityEngine.UI.GridLayoutGroup gridLayoutGroup;
	private bool tablaActiva;

	public bool TablaActiva{
		get{
			return tablaActiva;
		}set{
			tablaActiva = value;
		}
	}

	public static TablaCartas tablaActual(){
		if (CursorSobreAlgunaTabla) {
			TablaCartas[] bothTables = GameObject.FindObjectsOfType<TablaCartas>();
			if (bothTables [0].CursorSobreEstaTabla) {
				return bothTables [0];
			} else {
				return bothTables [0];
			}
		} else {
			return null;
		}
	}

	public static bool CursorSobreAlgunaTabla
	{
		get
		{
			TablaCartas[] bothTables = GameObject.FindObjectsOfType<TablaCartas>();
			return (bothTables[0].CursorSobreEstaTabla || bothTables[1].CursorSobreEstaTabla);
		}
	}

	// returns true only if we are hovering over this table`s collider
	public bool CursorSobreEstaTabla
	{
		get { return cursorSobreEstaTabla; }
	}

	// MONOBEHAVIOUR SCRIPTS (mouse over collider detection)
	void Awake()
	{
		col = GetComponent<BoxCollider2D>();
		tablaActiva = false;
	}
		
	void Update()
	{
		// we need to Raycast because OnMouseEnter, etc reacts to colliders on cards and cards "cover" the table
		if (Input.touchCount <= 0)
			return;
		RaycastHit[] hits;
		hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.GetTouch(0).position), 30f);
		bool haPasadoPorColider = false;
		foreach (RaycastHit h in hits)
		{
			if (h.collider == col)
				haPasadoPorColider = true;
		}
		cursorSobreEstaTabla = haPasadoPorColider;
	}

	public void AñadirCarta(GameObject cartaGobj){
		//int idCarta = cartaGobj.GetComponent<IDHolder> ().UniqueID;
		cartaGobj.tag = cartaGobj.tag.Equals("CartaDentroMazo") ? "CartaFueraMazo": "CartaDentroMazo";
		cartaGobj.transform.SetParent (gridLayoutGroup.gameObject.transform);
	}

	public int NumElementos(){
		return gridLayoutGroup.transform.childCount;
	}
		
	public List<GameObject> ObtenerElementos(){
		List<GameObject> cartas = new List<GameObject> ();
		for (int i = 0; i < gridLayoutGroup.transform.childCount; i++) {
			cartas.Add (gridLayoutGroup.transform.GetChild (i).gameObject);
		}
		return cartas;
	}


	public void Accion(){
		if (tag.Equals ("TablaCartas"))
			ControladorMenu.Instance.AñadirElementoMazo (TouchManager2.Instance.ObjetoActual);
		else
			ControladorMenu.Instance.AñadirElementoCartas (TouchManager2.Instance.ObjetoActual);
		TouchManager2.Instance.ObjetoActual = null;
		//HabilitarColliderElementos ();
	}

	public void CerrarAccion(){
		AccionBaraja.Instance.CerrarAccion ();
		tablaActiva = false;
		//HabilitarColliderElementos ();
	}

	public void MostrarAccion(){
		DesactivarTablas ();
		if (tag.Equals ("TablaCartas"))
			AccionBaraja.Instance.MostrarAccion (AccionBaraja.TIPO_ACCION.USAR, Accion);
		else
			AccionBaraja.Instance.MostrarAccion (AccionBaraja.TIPO_ACCION.QUITAR, Accion);
		tablaActiva = true;
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

	public static void DesactivarTablas(){
		TablaCartas[] bothTables = GameObject.FindObjectsOfType<TablaCartas>();
		foreach (TablaCartas tabla in bothTables) {
			if (tabla.TablaActiva) {
				tabla.CerrarAccion ();
			}
		}
	}
}
