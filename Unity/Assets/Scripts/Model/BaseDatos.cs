using UnityEngine;
using System.Collections;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections.Generic;

public class BaseDatos 
{
    #region Atributos
    private static BaseDatos instance;
    private DatabaseReference reference;
	private List<Jugador> jugadores;
    private string userIDActual;
    private DataSnapshot usuarioActual;
    private DataSnapshot assets;
	private bool existsConnection;
	public delegate void CallBack(string message);
    #endregion

    private BaseDatos()
    {
        this.assets = null;
        this.usuarioActual = null;
		this.jugadores = new List<Jugador>();
    }

    public static BaseDatos Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new BaseDatos();
            }
            return instance;

        }
    }

    public void InicializarBase(CallBack callback)
    {
		InitializeDataBase();
        ObtenerAssets(callback);
    }

	/// <summary>
	/// Inicializa la referencia a la base de datos.
	/// </summary>
    private void InitializeDataBase()
    {
        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://battle-galaxy-cda70.firebaseio.com/");
        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

	/// <summary>
	/// Obtiene todas las cartas de la base de datos y las almacena en assets.
	/// </summary>
	/// <param name="callback">Callback.</param>
    private void ObtenerAssets(CallBack callback)
    {
        reference.Child("assets").GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
				Debug.Log("Excepcion: "+task.Exception);
				callback.Invoke(SesionUsuario.GetErrorMessage(task.Exception));
                assets = null;
            }
			else if(task.IsCanceled){
				Debug.Log("Excepcion: "+task.Exception);
				callback.Invoke(SesionUsuario.GetErrorMessage(task.Exception));
				assets = null;
			}
            else if (task.IsCompleted)
            {
                //Assigno los assets a una variable global
                assets = task.Result;
                callback.Invoke("");
            }
        });
    }

	/// <summary>
	/// Recoge la información del jugador de base de datos.
	/// </summary>
	/// <param name="userId">User identifier.</param>
	/// <param name="callback">Callback.</param>
	public void RecogerJugador(string userId, SesionUsuario.CallBack callback)
    {
        this.userIDActual = userId;
		reference.Child("users").Child(userId).GetValueAsync().ContinueWith(task => {
			if (task.IsFaulted)
			{
				Debug.Log("Excepcion: "+task.Exception);
				callback.Invoke("Ha habido algun error al recoger el usuario");
			}
			else if(task.IsCanceled){
				Debug.Log("Excepcion: "+task.Exception);
				callback.Invoke("Ha habido algun error al recoger el usuario");
			}
			else if (task.IsCompleted)
			{
				//Recojo los datos del jugador
				usuarioActual = task.Result;
				ObtenerDatosJugador(callback,usuarioActual);
			}
		});
    }

	/// <summary>
	/// Añade items y cartas como welcome pack al nuevo jugador.
	/// </summary>
	/// <param name="jugador">Jugador.</param>
    public void AñadirWelcomePackJugador(Jugador jugador)
    {
        List<Carta> cartasWelcomePack = GenerarCartasAleatorias(8);
		//List<Carta> cartasWelcomePack = GenerarTodasCartas();
		AñadirCartasJugador(jugador, cartasWelcomePack);
		List<Item> itemsAleatorios = GenerarItemsAleatorios (8);
		AñadirItemsJugador (jugador,itemsAleatorios);
		List<int> idCartasMazo;
		/*if (jugador.TipoJugador.Equals(Jugador.TIPO_JUGADOR.AUTOMÁTICO))
			idCartasMazo = Local.IDCartasMazo ();
		else
			idCartasMazo = GenerarIDCartasMazo ();*/
		idCartasMazo = GenerarIDCartasMazo ();
		AñadirMazoJugador (jugador,idCartasMazo);

    }

	/// <summary>
	/// Devuelve todas las cartas del juego.
	/// </summary>
	/// <returns>The todas cartas.</returns>
	public List<Carta> GenerarTodasCartas()
	{
		var json = assets.Value as Dictionary<string, object>;
		List<string> keyList = new List<string>(json.Keys);
		List<Carta> todasCartas = new List<Carta>();
		System.Random rnd = new System.Random();
		for (int i = 0; i < assets.ChildrenCount; i++)
		{
			string idAsset = keyList[i];
			todasCartas.Add(CrearCartaJugador(idAsset, null));
		}
		return todasCartas;
	}

	/// <summary>
	/// Permite generar cartas aleatorias.
	/// </summary>
	/// <returns>The cartas aleatorias.</returns>
	/// <param name="numCartas">Number cartas.</param>
	public List<Carta> GenerarCartasAleatorias(int numCartas)
    {
        var json = assets.Value as Dictionary<string, object>;
        List<string> keyList = new List<string>(json.Keys);
		List<Carta> cartasAleatorias = new List<Carta>();
		System.Random rnd = new System.Random();
		for (int i = 0; i < numCartas; i++)
        {
			int numCarta = rnd.Next(0, json.Count);
			string idAssetRandom = keyList[numCarta];
			cartasAleatorias.Add(CrearCartaJugador(idAssetRandom, null));
        }
		return cartasAleatorias;
    }

	/// <summary>
	/// Permite generar items aleatorios.
	/// </summary>
	/// <returns>The items aleatorios.</returns>
	/// <param name="numItems">Number items.</param>
	public List<Item> GenerarItemsAleatorios(int numItems){
		System.Random rnd = new System.Random();
		List<Item> itemsAleatorios = new List<Item> ();
		for (int i = 0; i < numItems; i++) {
			int tipoItem = rnd.Next(0, 2);
			int cantidad = rnd.Next (50, 80);
			string rutaImagen;
			if (tipoItem == 1) {
				rutaImagen = "Sprites/Recursos/Componentes/item_piedra";
			} else {
				rutaImagen = "Sprites/Recursos/Componentes/item_pocion";
			}
			Item item = null;
			if (tipoItem == 0) {
				item = new Pocion (rutaImagen, cantidad);
			} else {
				item = new Piedra (rutaImagen, cantidad);
			}
			itemsAleatorios.Add (item);
		}

		return itemsAleatorios;
	}

	/// <summary>
	/// Permite crear el jugador.
	/// </summary>
	/// <param name="userId">User identifier.</param>
	/// <param name="callBack">Call back.</param>
    public void CrearJugador(string userId, SesionUsuario.CallBack callBack)
    {
        Debug.Log("Crear jugador");
        this.userIDActual = userId;
		AñadirJugador(new Jugador(Jugador.TIPO_JUGADOR.MANUAL));
        AñadirWelcomePackJugador(Local);
        AñadirJugadorBaseDatos(userId,Local);
        callBack.Invoke("");
    }

	/// <summary>
	/// Permite obtener los datos del jugador.
	/// </summary>
	/// <param name="callBack">Call back.</param>
	/// <param name="usuario">Usuario.</param>
	private void ObtenerDatosJugador(SesionUsuario.CallBack callBack,DataSnapshot usuario)
	{
		Debug.Log("Obtener jugador");
		AñadirJugador(new Jugador(Jugador.TIPO_JUGADOR.MANUAL));
		int nivel = ObtenerNivelJugador(usuario);
		int experiencia = ObtenerExperienciaJugador(usuario);
		List<Carta> cartasJugador = ObtenerCartasJugador(usuario);
		List<Item> itemsJugador = ObtenerItemsJugador (usuario);
		AñadirCartasJugador(Local, cartasJugador);
		List<int> idCartasMazo = ObtenerIDCartasMazo (usuario);
		AñadirMazoJugador (Local, idCartasMazo);
		AñadirItemsJugador (Local, itemsJugador);
		AñadirExperienciaNivelJugador (Local, nivel,experiencia);
		callBack.Invoke("");
	}

	/// <summary>
	/// Añade las cartas al jugador.
	/// </summary>
	/// <param name="jugador">Jugador.</param>
	/// <param name="cartas">Cartas.</param>
    private void AñadirCartasJugador(Jugador jugador, List<Carta> cartas)
    {
		foreach (Carta carta in cartas)
        {    
			AñadirCartaJugador(jugador,carta);
        }
        
    }

	/// <summary>
	/// Añade el mazo al jugador.
	/// </summary>
	/// <param name="jugador">Jugador.</param>
	/// <param name="idCartasMazo">Identifier cartas mazo.</param>
	public void AñadirMazoJugador(Jugador jugador,List<int> idCartasMazo){
		jugador.ClearMazo ();
		jugador.IDCartasMazo ().Clear ();
		foreach(int idCarta in idCartasMazo) {
			jugador.AñadirIDCartaMazo (idCarta);
		}
		jugador.InicializarMazo ();
	}

	/// <summary>
	/// Añade los items al jugador.
	/// </summary>
	/// <param name="jugador">Jugador.</param>
	/// <param name="items">Items.</param>
	private void AñadirItemsJugador(Jugador jugador, List<Item> items)
	{
		foreach (Item item in items)
		{
			AñadirItemJugador(jugador, item);
		}
	}

	/// <summary>
	/// Añade experiencia y nuvel al jugador.
	/// </summary>
	/// <param name="jugador">Jugador.</param>
	/// <param name="nivel">Nivel.</param>
	/// <param name="experiencia">Experiencia.</param>
	private void AñadirExperienciaNivelJugador(Jugador jugador, int nivel, int experiencia){
		jugador.Nivel = nivel;
		jugador.Experiencia = experiencia;
	}

	/// <summary>
	/// Instancia una carta, con o sin progreso.
	/// </summary>
	/// <returns>The carta jugador.</returns>
	/// <param name="idAsset">Identifier asset.</param>
	/// <param name="progreso">Progreso.</param>
    private Carta CrearCartaJugador(string idAsset, Progreso progreso)
    {
        string assetJSON = assets.Child(idAsset).GetRawJsonValue();
        CartaBase asset = JsonUtility.FromJson<CartaBase>(assetJSON);
        Carta carta = new Carta(idAsset, asset);
        if(progreso != null)
            carta.Progreso = progreso;
        return carta;
    }

	/// <summary>
	/// Instancia un item piedra o poción.
	/// </summary>
	/// <returns>The item jugador.</returns>
	/// <param name="tipoItem">Tipo item.</param>
	/// <param name="rutaImagen">Ruta imagen.</param>
	/// <param name="cantidad">Cantidad.</param>
	private Item CrearItemJugador(int tipoItem, string rutaImagen, int cantidad)
	{
		Item item = null;
		if (tipoItem == 0) {
			item = new Pocion (rutaImagen, cantidad);
		} else {
			item = new Piedra (rutaImagen, cantidad);
		}
		return item;
	}

	/// <summary>
	/// Añade una carta al jugador.
	/// </summary>
	/// <param name="jugador">Jugador.</param>
	/// <param name="carta">Carta.</param>
    private void AñadirCartaJugador(Jugador jugador, Carta carta)
    {
        jugador.AñadirCarta(carta);
    }

	/// <summary>
	/// Añade un item al jugador.
	/// </summary>
	/// <param name="jugador">Jugador.</param>
	/// <param name="item">Item.</param>
	private void AñadirItemJugador(Jugador jugador, Item item)
	{
		jugador.AñadirItem(item);
	}

	/// <summary>
	/// Añade el jugador a la base de datos.
	/// </summary>
	/// <param name="userID">User I.</param>
	/// <param name="jugador">Jugador.</param>
    private void AñadirJugadorBaseDatos(string userID, Jugador jugador)
    {
        reference.Child("users").Child(userID).SetValueAsync(jugador.ToDictionary());
    }

	/// <summary>
	/// Obtiene las cartas del jugador.
	/// </summary>
	/// <returns>The cartas jugador.</returns>
	/// <param name="usuario">Usuario.</param>
    private List<Carta> ObtenerCartasJugador(DataSnapshot usuario)
    {
		List<Carta> cartasJugador = new List<Carta>();
		if (!usuario.HasChild ("cartas"))
			return cartasJugador;
		var rawjson = JSONUtils.StringToJSON(usuario.Child("cartas").GetRawJsonValue());
        for (int i = 0; i < rawjson.Count; i++)
        {
            string idAsset = (string)usuario.Child("cartas").Child(i.ToString()).Child("asset").GetValue(true);
			var progresoJSON = JSONUtils.StringToJSON (usuario.Child ("cartas").Child (i.ToString ()).Child ("progreso").GetRawJsonValue ());
			Progreso progreso = new Progreso ();
			progreso.Piedra = Int32.Parse (progresoJSON ["material"]);
			progreso.Pocion = Int32.Parse (progresoJSON ["pocion"]);
            cartasJugador.Add(CrearCartaJugador(idAsset, progreso));
        }
        return cartasJugador;
    }

	/// <summary>
	/// Obtiene los items del jugador.
	/// </summary>
	/// <returns>The items jugador.</returns>
	/// <param name="usuario">Usuario.</param>
	private List<Item> ObtenerItemsJugador(DataSnapshot usuario)
	{
		List<Item> itemsJugador = new List<Item>();
		if (!usuario.HasChild ("items"))
			return itemsJugador;
		
		var rawjson = JSONUtils.StringToJSON(usuario.Child("items").GetRawJsonValue());
		for (int i = 0; i < rawjson.Count; i++)
		{
			int tipoItem = Convert.ToInt32 ((long)usuario.Child ("items").Child (i.ToString ()).Child ("tipoItem").GetValue (true));
			string rutaImagenItem = (string)usuario.Child("items").Child(i.ToString()).Child("rutaImagen").GetValue(true);
			int cantidad = Convert.ToInt32((long)usuario.Child("items").Child(i.ToString()).Child("cantidad").GetValue(true));
			itemsJugador.Add(CrearItemJugador(tipoItem, rutaImagenItem,cantidad));
		}
		return itemsJugador;
	}

	/// <summary>
	/// Obtiene los identificadores de las cartas del mazo.
	/// </summary>
	/// <returns>The identifier cartas mazo.</returns>
	/// <param name="usuario">Usuario.</param>
	private List<int> ObtenerIDCartasMazo(DataSnapshot usuario){	
		List<int> idsCartasMazo = new List<int> ();
		if (!usuario.HasChild ("cartas"))
			return idsCartasMazo;
		string [] idCartas = ((string)usuario.Child("mazo").GetValue(true)).Split (',');
		foreach (string id in idCartas) {
			idsCartasMazo.Add (Int32.Parse (id));
		}
		return idsCartasMazo;
	}

	/// <summary>
	/// Genera identificadores de las cartas para el mazo.
	/// </summary>
	/// <returns>The identifier cartas mazo.</returns>
	private List<int> GenerarIDCartasMazo(){
		List<int> idsCartasMazo = new List<int> ();
		for (int i = 0; i < 8; i++) {
			idsCartasMazo.Add (i);
		}
		return idsCartasMazo;
	}

	/// <summary>
	/// Obtiene el nivel del jugador almacenado en Firebase.
	/// </summary>
	/// <returns>The nivel jugador.</returns>
	/// <param name="usuario">Usuario.</param>
    private int ObtenerNivelJugador(DataSnapshot usuario)
    {
        return Convert.ToInt32(usuario.Child("nivel").GetValue(true));
    }

	/// <summary>
	/// Obtiene la experiencia del jugador en base de datos.
	/// </summary>
	/// <returns>The experiencia jugador.</returns>
	/// <param name="usuario">Usuario.</param>
	private int ObtenerExperienciaJugador(DataSnapshot usuario)
	{
		return Convert.ToInt32(usuario.Child("experiencia").GetValue(true));
	}

	/// <summary>
	/// Añade el jugador a la lista de jugadores creados en esta instancia.
	/// </summary>
	/// <param name="jugador">Jugador.</param>
    private void AñadirJugador(Jugador jugador)
    {
		jugadores.Add (jugador);
    }

	/// <summary>
	/// Elimina el jugador enemigo de los jugadores de esta instancia.
	/// </summary>
	private void EliminarEnemigo(){
		jugadores.Remove (Enemigo);
	}

	/// <summary>
	/// Actualiza los items del jugador alojado en base de datos.
	/// </summary>
	/// <param name="carta">Carta.</param>
	/// <param name="item">Item.</param>
	public void ActualizarItemCarta(Carta carta,Item item){
		Local.EliminarItem (item);
		ActualizarItemsBaseDatos ();
		ActualizarCartaBaseDatos (carta);
	}

	/// <summary>
	/// Actualiza la carta del jugador en base de datos.
	/// </summary>
	/// <param name="carta">Carta.</param>
	public void ActualizarCartaBaseDatos(Carta carta){
		int indiceCarta = Local.BuscarPosicionCarta (carta);
		ReferenciaCartas().Child (indiceCarta.ToString ()).SetValueAsync (carta.ToDictionary ());
	}

	/// <summary>
	/// Actualiza todo el jugador de base de datos.
	/// </summary>
	/// <param name="cambioCartas">If set to <c>true</c> cambio cartas.</param>
	public void ActualizarJugadorBaseDatos(bool cambioCartas){
		ActualizarItemsBaseDatos ();
		if(cambioCartas)
			ActualizarCartasBaseDatos ();
		ActualizarNivelBaseDatos ();
		ActualizarExperienciaBaseDatos ();
	}

	/// <summary>
	/// Actualiza los items del jugador en base de datos.
	/// </summary>
	private void ActualizarItemsBaseDatos(){
		
		ReferenciaItems().SetValueAsync (Local.ItemsToDictionary ());
	}

	/// <summary>
	/// Actualiza las cartas del jugador en base de datos.
	/// </summary>
	private void ActualizarCartasBaseDatos(){

		ReferenciaCartas().SetValueAsync (Local.CartasToDictionary ());
	}

	/// <summary>
	/// Actualiza nivel y experiencia del jugador en base de datos.
	/// </summary>
	public void ActualizarNivelYExperienciaBaseDatos(){
		ActualizarExperienciaBaseDatos ();
		ActualizarNivelBaseDatos ();
	}

	/// <summary>
	/// Actualiza solo experiencia en base de datos.
	/// </summary>
	private void ActualizarExperienciaBaseDatos(){
		ReferenciaExperiencia ().SetValueAsync (Local.Experiencia);
	}

	/// <summary>
	/// Actualiza solo nivel en base de datos.
	/// </summary>
	private void ActualizarNivelBaseDatos(){
		ReferenciaNivel().SetValueAsync (Local.Nivel);
	}

	/// <summary>
	/// Actualiza el mazo del jugador en base de datos.
	/// </summary>
	public void ActualizarMazoBaseDatos(){
		ReferenciaMazo().SetValueAsync (Local.MazoToDictionary());
	}

	/// <summary>
	/// Añade una carta al conjunto de cartas creadas en el juego en Firebase.
	/// </summary>
	/// <param name="familia">Familia.</param>
	/// <param name="asset">Asset.</param>
    public void GuardarCarta(string familia,CartaBase asset)
    {
		ReferenciaAssets().Push().SetRawJsonValueAsync(JsonUtility.ToJson(asset));
        Debug.Log("Guardar carta ok");
    }

	/// <summary>
	/// Busca en la base de datos Firebase si existe una evolucion con los parametros dados.
	/// </summary>
	/// <returns>The evolucion.</returns>
	/// <param name="familia">Familia.</param>
	/// <param name="evolucion">Evolucion.</param>
	/// <param name="idEvolucion">Identifier evolucion.</param>
	public KeyValuePair<string,CartaBase> BuscarEvolucion(Familia familia, int evolucion, int idEvolucion){
		
		var json = assets.Value as Dictionary<string, object>;
		List<string> keyList = new List<string>(json.Keys);
		KeyValuePair<string,CartaBase> evolucionEncontrada = new KeyValuePair<string, CartaBase> ("",null);
		foreach(string idAsset in keyList)
		{
			string assetJSON = assets.Child(idAsset).GetRawJsonValue();
			CartaBase asset = JsonUtility.FromJson<CartaBase>(assetJSON);
			if   (asset.Familia.Equals (familia) && asset.IDEvolucion ==  idEvolucion && asset.Evolucion == (evolucion+1)) {
				evolucionEncontrada = new KeyValuePair<string, CartaBase> (idAsset,asset);
			}

		}
		return evolucionEncontrada;
	}
		
	//Jugador enemigo solo existente en las batallas.
    public Jugador Enemigo
    {
        get
        {
            return jugadores[1];
        }
    }

	//Jugador local, el que dispone de la sesión abierta.
    public Jugador Local
    {
        get
        {
            return jugadores[0];
        }
    }

    public Jugador [] GetPlayers()
    {
		return jugadores.ToArray ();
    }

    public bool BaseDatosInicializada
    {
        get
        {
            return assets != null;
        }
    }

	/// <summary>
	/// Crea el jugador enemigo de la batalla.
	/// </summary>
	public void CrearJugadorEnemigo(){
		AñadirJugador(new Jugador(Jugador.TIPO_JUGADOR.AUTOMÁTICO));
		AñadirWelcomePackJugador(Enemigo);
	}

	/// <summary>
	/// Cierra la sesión del usuario, eliminando los datos necesarios en esta instancia.
	/// </summary>
	public void CerrarSesión(){
		jugadores.Clear ();
		userIDActual = "";
		usuarioActual = null;
	}

	public void Clear(){
		//Local.Reset ();
		EliminarEnemigo ();
	}

	private DatabaseReference ReferenciaAssets(){
		return reference.Child ("assets");
	}

	private DatabaseReference ReferenciaJugador(){
		return reference.Child ("users").Child (userIDActual);
	}

	private DatabaseReference ReferenciaCartas(){
		return reference.Child ("users").Child (userIDActual).Child ("cartas");
	}

	private DatabaseReference ReferenciaMazo(){
		return reference.Child ("users").Child (userIDActual).Child ("mazo");
	}

	private DatabaseReference ReferenciaItems(){
		return reference.Child ("users").Child (userIDActual).Child ("items");
	}

	private DatabaseReference ReferenciaNivel(){
		return reference.Child ("users").Child (userIDActual).Child ("nivel");
	}

	private DatabaseReference ReferenciaExperiencia(){
		return reference.Child ("users").Child (userIDActual).Child ("experiencia");
	}

}
