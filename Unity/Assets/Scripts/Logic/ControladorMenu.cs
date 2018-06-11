using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
		IniciarMusica ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void IniciarMusica(){
		if (!ConfiguracionUsuario.Instance.Musica) {
			Camera.main.GetComponent<AudioSource> ().Pause ();
		}
	}

	public PANTALLA_MENU PantallaActual{
		get{
			return pantallaActual;
		}
		set{ 
			pantallaActual = value;
			TouchManagerMenu.Instance.ObjetoActual = null;
		}
	}

	public void AgregarItemCarta(int idCarta, int idItem){

		Carta carta = BuscarCarta (idCarta);
		Item item = BuscarItem (idItem);
		bool progresoLleno = false;
		if (item.GetType() == typeof(Piedra)) {
			if (carta.Progreso.Piedra >= 100) {
				progresoLleno = true;
			} else {
				carta.AñadirPiedra (item.Cantidad);
			}
		}
		else {
			if (carta.Progreso.Pocion >= 100) {
				progresoLleno = true;
			} else {
				carta.AñadirPocion (item.Cantidad);
			}
		}
		if (!progresoLleno) {
			new AñadirItemCartaPrevisualizadaCommand (item).AñadirAlaCola ();
			new AñadirItemCartaCommand (carta, item).AñadirAlaCola ();
			BaseDatos.Instance.ActualizarItemCarta (carta, item);
			if (carta.Progreso.Piedra >= 100 && carta.Progreso.Pocion >= 100) {
				MessageManager.Instance.ShowMessage ("¡Ya puedes evolucionar la carta!", 2f);
			} else {
				MessageManager.Instance.ShowMessage ("Item añadido con éxito", 1f);
			}

		} else {
			MessageManager.Instance.ShowMessage ("¡El progreso de este item está lleno!", 2f);
		}

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
		return ExisteEvolucion(carta) && carta.Progreso.Piedra >= 100f && carta.Progreso.Pocion >= 100f;
	}

	private bool ExisteEvolucion(Carta carta){
		Familia familia = carta.AssetCarta.Familia;
		int evolucionActual = carta.AssetCarta.Evolucion;
		int idEvolucion = carta.AssetCarta.IDEvolucion;
		KeyValuePair<string,CartaAsset> evolucion = BuscarEvolucion (familia, evolucionActual,idEvolucion);
		return !"".Equals(evolucion.Key)  && null != evolucion.Value;
	}

	public KeyValuePair<string,CartaAsset> BuscarEvolucion(Familia familia, int evolucionActual, int idEvolucion){
		return BaseDatos.Instance.BuscarEvolucion (familia, evolucionActual, idEvolucion);
	}

	public void EvolucionarCarta(GameObject cartaG){
		Carta carta = BuscarCarta (cartaG.GetComponent<IDHolder>().UniqueID);
		//Buscamos la evolución
		KeyValuePair<string,CartaAsset> evolucion = BuscarEvolucion (carta.AssetCarta.Familia, carta.AssetCarta.Evolucion,carta.AssetCarta.IDEvolucion);
		//modificamos el progreso de la carta restando las 100 unidades necesarias para evolucionar
		carta.Progreso.Piedra -= 100;
		carta.Progreso.Pocion -= 100;
		carta.IdAsset = evolucion.Key;
		carta.AssetCarta = evolucion.Value;
		OneCardManager manager = cartaG.GetComponent<OneCardManager>();
		ProgresoVisual progreso = cartaG.GetComponent<ProgresoVisual>();
		//modificamos el asset de la carta seleccionada actual y lo cambiamos por la evolución
		manager.CartaAsset = evolucion.Value;
		//actualizamos en la carta visual el progreso
		progreso.PorcentajeProgresoPiedra = carta.Progreso.Piedra > 100 ? 100: carta.Progreso.Piedra;
		progreso.PorcentajeProgresoPocion = carta.Progreso.Pocion > 100 ? 100: carta.Progreso.Pocion;
		manager.LeerDatos();
		progreso.LeerProgreso ();
		MessageManager.Instance.ShowMessage ("¡Una nueva carta ha aparecido!", 2f);
		BaseDatos.Instance.ActualizarCartaBaseDatos (carta);
	}

	public void MostrarAccion(GameObject carta){
		switch (pantallaActual) {
			case PANTALLA_MENU.INVENTARIO:
				Acciones.Instance.MostrarAcciones (carta);
				break;
			case PANTALLA_MENU.MAZO:
				TablaCartas tabla = TablaActual (carta);
				tabla.MostrarAccion (carta);
				break;
			default:
				break;
		}
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

	public float ObtenerExperienciaJugador(){
		return BaseDatos.Instance.Local.Experiencia / 100f;
	}

	public List<System.Object> RecogerElemento(Elementos.TIPO_ELEMENTOS tipoElementos){
		List<System.Object> elementos;
		switch (tipoElementos) {
		case Elementos.TIPO_ELEMENTOS.CARTAS:
			elementos = BaseDatos.Instance.Local.Cartas ();
				if (elementos.Count == 0)
					MessageManager.Instance.ShowMessage ("Al parecer no tienes cartas... ¡Combate para ganar premios!", 2f);
				break;
			case Elementos.TIPO_ELEMENTOS.CARTAS_FUERAMAZO:
				elementos = BuscarCartasFueraMazo ();
				break;
			case Elementos.TIPO_ELEMENTOS.ITEMS:
				elementos = BaseDatos.Instance.Local.Items ();
				if (elementos.Count == 0)
					MessageManager.Instance.ShowMessage ("Al parecer no tienes items... ¡Combate para ganar premios!", 2f);
				break;
			case Elementos.TIPO_ELEMENTOS.MAZO:
				elementos = BaseDatos.Instance.Local.CartasEnElMazo ();
				break;
		default:
			elementos = null;
			break;
		}
		return elementos;
	}

	private List<System.Object> BuscarCartasFueraMazo(){
		List<System.Object> cartasFueraMazo = new List<System.Object> ();
		foreach (Carta carta in BaseDatos.Instance.Local.Cartas()) {
			if (ControladorMenu.Instance.CartaFueraMazo (carta)) {
				cartasFueraMazo.Add (carta);
			}
		}
		return cartasFueraMazo;
	}

	public void ActualizarMusica(bool musica){
		if (musica) {
			if(!Camera.main.GetComponent<AudioSource> ().isPlaying)
				Camera.main.GetComponent<AudioSource> ().Play ();
		} else {
			if(Camera.main.GetComponent<AudioSource> ().isPlaying)
				Camera.main.GetComponent<AudioSource> ().Pause ();
		}
	}

	public void CerrarSesión(){
		SesionUsuario.Instance.CerrarSesión ();
		GameObject objetosGenerales = GameObject.FindGameObjectWithTag ("ObjetosGenerales");
		GameObject confUsuario = GameObject.FindGameObjectWithTag ("ConfiguracionUsuario");
		Destroy (objetosGenerales);
		Destroy (confUsuario);
		SceneManager.LoadScene("Login");
	}
}
