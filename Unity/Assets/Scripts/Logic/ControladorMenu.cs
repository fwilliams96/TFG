using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PANTALLA_MENU
{
	INVENTARIO,
	BATALLA,
	PERFIL,
	MAZO
}


public class ControladorMenu : MonoBehaviour {

	public static ControladorMenu Instance;

	private PANTALLA_MENU pantallaActual;

	void Awake(){
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		pantallaActual = PANTALLA_MENU.INVENTARIO;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public PANTALLA_MENU PantallaActual{
		get{
			return pantallaActual;
		}
		set{ 
			pantallaActual = value;
			TouchManager2.Instance.ObjetoActual = null;
		}
	}

	public void AgregarItemCarta(int idCarta, int idItem){

		Carta carta = BuscarCarta (idCarta);
		Item item = BuscarItem (idItem);
		if (item.Tipo.Equals (TipoItem.Material))
			carta.AñadirMaterial (item.Cantidad);
		//else if(item.Tipo.Equals (TipoItem.Pocion))
		else
			carta.AñadirPocion (item.Cantidad);
		new AñadirItemCartaPrevisualizadaCommand (item).AñadirAlaCola ();
		new AñadirItemCartaCommand (carta,item).AñadirAlaCola ();
		BaseDatos.Instance.ActualizarItemCarta (carta,item);
	}

	private Carta BuscarCarta(int idCarta){
		bool trobat = false;
		int i = 0;
		Carta carta = null;
		List<System.Object> cartasJugador = BaseDatos.Instance.Local.Cartas ();
		while (i < cartasJugador.Count && !trobat) {
			carta = (Carta)cartasJugador [i];
			if (carta.ID == idCarta)
				trobat = true;
			else
				i += 1;
		}
		return carta;
	}

	private Item BuscarItem(int idItem){
		bool trobat = false;
		int i = 0;
		Item item = null;
		List<System.Object> itemsJugador = BaseDatos.Instance.Local.Items ();
		while (i < itemsJugador.Count && !trobat) {
			item = (Item)itemsJugador [i];
			if (item.ID == idItem)
				trobat = true;
			else
				i += 1;
		}
		return item;
	}

	public bool SePuedeEvolucionar(int idCarta){
		Carta carta = BuscarCarta (idCarta);
		return ExisteEvolucion(carta) && carta.Progreso.Material >= 100f && carta.Progreso.Pocion >= 100f;
	}

	private bool ExisteEvolucion(Carta carta){
		Familia familia = carta.AssetCarta.Familia;
		int evolucionActual = carta.AssetCarta.Evolucion;
		KeyValuePair<string,CartaAsset> evolucion = BuscarEvolucion (familia, evolucionActual);
		return !"".Equals(evolucion.Key)  && null != evolucion.Value;
	}

	public KeyValuePair<string,CartaAsset> BuscarEvolucion(Familia familia, int evolucionActual){
		return BaseDatos.Instance.BuscarEvolucion (familia, evolucionActual);
	}

	public void EvolucionarCarta(GameObject cartaG){
		Carta carta = BuscarCarta (cartaG.GetComponent<IDHolder>().UniqueID);
		//Buscamos la evolución
		KeyValuePair<string,CartaAsset> evolucion = BuscarEvolucion (carta.AssetCarta.Familia, carta.AssetCarta.Evolucion);
		//modificamos el progreso de la carta restando las 100 unidades necesarias para evolucionar
		carta.Progreso.Material -= 100;
		carta.Progreso.Pocion -= 100;
		carta.IdAsset = evolucion.Key;
		carta.AssetCarta = evolucion.Value;
		OneCardManager manager = cartaG.GetComponent<OneCardManager>();
		//modificamos el asset de la carta seleccionada actual y lo cambiamos por la evolución
		manager.CartaAsset = evolucion.Value;
		//actualizamos en la carta visual el progreso
		manager.PorcentajeProgresoTrebol = carta.Progreso.Material > 100 ? 100: carta.Progreso.Material;
		manager.PorcentajeProgresoPocion = carta.Progreso.Pocion > 100 ? 100: carta.Progreso.Pocion;
		manager.LeerDatos();
		MessageManager.Instance.ShowMessage ("¡Una nueva carta ha aparecido!", 2f);
		BaseDatos.Instance.ActualizarCartaBaseDatos (carta);
	}

	public void MostrarAccion(GameObject carta){
		TablaCartas tabla = TablaActual (carta);
		tabla.MostrarAccion (carta);
	}

	public void CerrarAccion(){
		switch (pantallaActual) {
			case PANTALLA_MENU.INVENTARIO:
				Acciones.Instance.CerrarMenu ();
				break;
			case PANTALLA_MENU.MAZO:
				AccionBaraja.Instance.CerrarAccion ();
				break;
			default:
				break;
		}
	}

	public static TablaCartas TablaActual(GameObject gObj){
		TablaCartas tabla = null;
		if(gObj.tag.Equals("CartaFueraMazo")){
			tabla = GameObject.FindGameObjectWithTag("TablaCartas").GetComponent<TablaCartas>();
		}else{
			tabla = GameObject.FindGameObjectWithTag("TablaMazo").GetComponent<TablaCartas>();
		}
		return tabla;
	}

	public void AñadirElementoMazo(GameObject carta){
		TablaCartas tabla = GameObject.FindGameObjectWithTag("TablaMazo").GetComponent<TablaCartas>();
		if (tabla.NumElementos () < 8)
			tabla.AñadirCarta (carta);
		else {
			MessageManager.Instance.ShowMessage ("El mazo está lleno", 3f);
		}
	}

	public void AñadirElementoCartas(GameObject carta){
		TablaCartas tabla = GameObject.FindGameObjectWithTag("TablaCartas").GetComponent<TablaCartas>();
		tabla.AñadirCarta (carta);
	}

	public int GuardarNuevoMazo(){
		int result = 0;
		TablaCartas tabla = GameObject.FindGameObjectWithTag("TablaMazo").GetComponent<TablaCartas>();
		if (tabla.NumElementos () == 8) {
			ModificarMazo (tabla.ObtenerElementos ());
			MessageManager.Instance.ShowMessage ("¡Mazo guardado con éxito!", 2f);
		} else {
			MessageManager.Instance.ShowMessage ("El mazo debe tener 8 cartas", 2f);
			result = -1;
		}
		return result;
	}

	private void ModificarMazo(List<GameObject> cartasMazo){
		Jugador jugador = BaseDatos.Instance.Local;
		List<int> idCartasMazo = new List<int>();
		foreach (GameObject  cartaGobj in cartasMazo) {
			Carta carta = BuscarCarta (cartaGobj.GetComponent<IDHolder>().UniqueID);
			int indice = jugador.BuscarPosicionCarta (carta);
			idCartasMazo.Add(indice);
		}
		BaseDatos.Instance.AñadirMazoJugador(jugador,idCartasMazo);
		BaseDatos.Instance.ActualizarMazoBaseDatos ();
	}

	public bool CartaFueraMazo(Carta carta){
		Jugador jugador = BaseDatos.Instance.Local;
		int indice = jugador.BuscarPosicionCarta (carta);
		bool trobat = false;
		foreach (int id in jugador.IDCartasMazo()) {
			if (id == indice) {
				trobat = true;
			}
		}
		return !trobat;
	}

	public string ObtenerNivelJugador(){
		return BaseDatos.Instance.Local.Nivel.ToString ();
	}
}
