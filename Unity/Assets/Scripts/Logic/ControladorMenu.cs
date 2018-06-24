using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Determina la pantalla en que nos encontramos
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

	/// <summary>
	/// Permite iniciar la musica del menu.
	/// </summary>
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

	/// <summary>
	/// Permite agregar un item a la carta.
	/// </summary>
	/// <param name="idCarta">Identifier carta.</param>
	/// <param name="idItem">Identifier item.</param>
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
			int exp = AñadirExperienciaJugador ();
			new AñadirItemCartaPrevisualizadaCommand (item).AñadirAlaCola ();
			new AñadirItemCartaCommand (carta, item).AñadirAlaCola ();

			new ShowMessageCommand ("¡Obtienes " + exp + " puntos de experiencia!", 1f).AñadirAlaCola ();
			if (carta.Progreso.Piedra >= 100 && carta.Progreso.Pocion >= 100) {
				if (ExisteEvolucion (carta)) {
					new ShowMessageCommand ("¡Ya puedes evolucionar la carta!", 1f).AñadirAlaCola ();
				}
			}
			BaseDatos.Instance.ActualizarItemCarta (carta, item);
			BaseDatos.Instance.ActualizarNivelYExperienciaBaseDatos ();	

		} else {
			MessageManager.Instance.ShowMessage ("¡El progreso de este item está lleno!", 2f);
		}

	}

	/// <summary>
	/// Permite añadir experiencia al jugador cuando se usa un item.
	/// </summary>
	/// <returns>The experiencia jugador.</returns>
	private int AñadirExperienciaJugador(){
		ConfiguracionUsuario settings = ConfiguracionUsuario.Instance;
		int min = 0;
		int max = 0;
		if (settings.ConfiguracionItems.Equals (ConfiguracionUsuario.TIPO_CONFIGURACION.FRACCION)) {
			min = 20;
			max = 40;
		} else if (settings.ConfiguracionItems.Equals (ConfiguracionUsuario.TIPO_CONFIGURACION.PORCENTAJE)) {
			min = 10;
			max = 20;
		} else {
			min = 5;
			max = 10;
		}
		int exp = Random.Range (min, max);
		Jugador jugador = BaseDatos.Instance.Local;
		jugador.Experiencia += exp; 
		if (jugador.Experiencia >= 100) {
			jugador.Experiencia -= 100;
			jugador.Nivel += 1;
		}
		return exp;
	}

	/// <summary>
	/// Busca la carta a partir de su identificador
	/// </summary>
	/// <returns>The carta.</returns>
	/// <param name="idCarta">Identifier carta.</param>
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

	/// <summary>
	/// Busca el item a partir de su identificador.
	/// </summary>
	/// <returns>The item.</returns>
	/// <param name="idItem">Identifier item.</param>
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

	/// <summary>
	/// Devuelve si se puede evolucionar la carta o no.
	/// </summary>
	/// <returns><c>true</c>, if puede evolucionar was sed, <c>false</c> otherwise.</returns>
	/// <param name="idCarta">Identifier carta.</param>
	public bool SePuedeEvolucionar(int idCarta){
		Carta carta = BuscarCarta (idCarta);
		return ExisteEvolucion(carta) && carta.Progreso.Piedra >= 100f && carta.Progreso.Pocion >= 100f;
	}

	/// <summary>
	/// Devuelve si existe una evolucion para la carta.
	/// </summary>
	/// <returns><c>true</c>, if evolucion was existed, <c>false</c> otherwise.</returns>
	/// <param name="idCarta">Identifier carta.</param>
	public bool ExisteEvolucion(int idCarta){
		Carta carta = BuscarCarta (idCarta);
		return ExisteEvolucion (carta);
	}

	/// <summary>
	/// Devuelve si existe una evolucion para la carta.
	/// </summary>
	/// <returns><c>true</c>, if evolucion was existed, <c>false</c> otherwise.</returns>
	/// <param name="carta">Carta.</param>
	private bool ExisteEvolucion(Carta carta){
		Familia familia = carta.AssetCarta.Familia;
		int evolucionActual = carta.AssetCarta.Evolucion;
		int idEvolucion = carta.AssetCarta.IDEvolucion;
		KeyValuePair<string,CartaBase> evolucion = BuscarEvolucion (familia, evolucionActual,idEvolucion);
		return !"".Equals(evolucion.Key)  && null != evolucion.Value;
	}

	/// <summary>
	/// Busca la siguiente evolucion en base de datos.
	/// </summary>
	/// <returns>The evolucion.</returns>
	/// <param name="familia">Familia.</param>
	/// <param name="evolucionActual">Evolucion actual.</param>
	/// <param name="idEvolucion">Identifier evolucion.</param>
	public KeyValuePair<string,CartaBase> BuscarEvolucion(Familia familia, int evolucionActual, int idEvolucion){
		return BaseDatos.Instance.BuscarEvolucion (familia, evolucionActual, idEvolucion);
	}

	/// <summary>
	/// Permite evolucionar la carta.
	/// </summary>
	/// <param name="cartaG">Carta g.</param>
	public void EvolucionarCarta(GameObject cartaG){
		Carta carta = BuscarCarta (cartaG.GetComponent<IDHolder>().UniqueID);
		//Buscamos la evolución
		KeyValuePair<string,CartaBase> evolucion = BuscarEvolucion (carta.AssetCarta.Familia, carta.AssetCarta.Evolucion,carta.AssetCarta.IDEvolucion);
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

	/// <summary>
	/// Muestra la accion principal segun la pantalla que nos encontremos.
	/// </summary>
	/// <param name="carta">Carta.</param>
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

	/// <summary>
	/// Permite cerrar la accion principal segun la pantalla que nos encontremos.
	/// </summary>
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

	/// <summary>
	/// A la hora de editar el mazo de batalla, permite decir en que scroll view nos encontramos.
	/// </summary>
	/// <returns>The actual.</returns>
	/// <param name="gObj">G object.</param>
	public static TablaCartas TablaActual(GameObject gObj){
		TablaCartas tabla = null;
		if(gObj.tag.Equals("CartaFueraMazo")){
			tabla = GameObject.FindGameObjectWithTag("TablaCartas").GetComponent<TablaCartas>();
		}else{
			tabla = GameObject.FindGameObjectWithTag("TablaMazo").GetComponent<TablaCartas>();
		}
		return tabla;
	}

	/// <summary>
	/// Añade una carta de las cartas fuera del mazo al mazo.
	/// </summary>
	/// <param name="carta">Carta.</param>
	public void AñadirElementoMazo(GameObject carta){
		TablaCartas tabla = GameObject.FindGameObjectWithTag("TablaMazo").GetComponent<TablaCartas>();
		if (tabla.NumElementos () < 8)
			tabla.AñadirCarta (carta);
		else {
			MessageManager.Instance.ShowMessage ("El mazo está lleno", 3f);
		}
	}

	/// <summary>
	/// Añade una carta del mazo a las cartas fuera del mazo.
	/// </summary>
	/// <param name="carta">Carta.</param>
	public void AñadirElementoCartas(GameObject carta){
		TablaCartas tabla = GameObject.FindGameObjectWithTag("TablaCartas").GetComponent<TablaCartas>();
		tabla.AñadirCarta (carta);
	}

	/// <summary>
	/// Guarda el nuevo mazo.
	/// </summary>
	/// <returns>The nuevo mazo.</returns>
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

	/// <summary>
	/// Obtiene las cartas del scroll view del mazo y las guarda en el mazo.
	/// </summary>
	/// <param name="cartasMazo">Cartas mazo.</param>
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

	/// <summary>
	/// Determina si la carta pertence al mazo actual o no.
	/// </summary>
	/// <returns><c>true</c>, if fuera mazo was cartaed, <c>false</c> otherwise.</returns>
	/// <param name="carta">Carta.</param>
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

	/// <summary>
	/// Devuelve el nivel del jugador
	/// </summary>
	/// <returns>The nivel jugador.</returns>
	public string ObtenerNivelJugador(){
		return BaseDatos.Instance.Local.Nivel.ToString ();
	}

	/// <summary>
	/// Devuelve la experiencia del jugador.
	/// </summary>
	/// <returns>The experiencia jugador.</returns>
	public float ObtenerExperienciaJugador(){
		return BaseDatos.Instance.Local.Experiencia / 100f;
	}

	/// <summary>
	/// Funcion global que devuelve items, todas las cartas, las cartas del mazo o las cartas de fuera de lmazo.
	/// </summary>
	/// <returns>The elemento.</returns>
	/// <param name="tipoElementos">Tipo elementos.</param>
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

	/// <summary>
	/// Devuelve las cartas fuera del mazo.
	/// </summary>
	/// <returns>The cartas fuera mazo.</returns>
	private List<System.Object> BuscarCartasFueraMazo(){
		List<System.Object> cartasFueraMazo = new List<System.Object> ();
		foreach (Carta carta in BaseDatos.Instance.Local.Cartas()) {
			if (ControladorMenu.Instance.CartaFueraMazo (carta)) {
				cartasFueraMazo.Add (carta);
			}
		}
		return cartasFueraMazo;
	}

	/// <summary>
	/// Actualiza los ajustes de musica.
	/// </summary>
	/// <param name="musica">If set to <c>true</c> musica.</param>
	public void ActualizarMusica(bool musica){
		if (musica) {
			if(!Camera.main.GetComponent<AudioSource> ().isPlaying)
				Camera.main.GetComponent<AudioSource> ().Play ();
		} else {
			if(Camera.main.GetComponent<AudioSource> ().isPlaying)
				Camera.main.GetComponent<AudioSource> ().Pause ();
		}
	}

	/// <summary>
	/// Cierra sesión del jugador actual, eliminando todos los datos.
	/// </summary>
	public void CerrarSesión(){
		SesionUsuario.Instance.CerrarSesión ();
		IDFactory.ResetIDs ();
		GameObject objetosGenerales = GameObject.FindGameObjectWithTag ("ObjetosGenerales");
		GameObject confUsuario = GameObject.FindGameObjectWithTag ("ConfiguracionUsuario");
		Destroy (objetosGenerales);
		Destroy (confUsuario);
		SceneManager.LoadScene("Login");
	}

	public bool JugadorPuedeJugarBatalla(){
		return NumCartasEnElMazo () == 8;
	}

	public int NumCartasEnElMazo(){
		return BaseDatos.Instance.Local.CartasEnElMazo ().Count;
	}
}
