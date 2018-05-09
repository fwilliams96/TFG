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
    private int numJugadores;
    private string userIDActual;
    private DataSnapshot usuarioActual;
    private DataSnapshot assets;
    public delegate void CallBack();
    //private SesionUsuario.CallBack callBack;
	public Dictionary<int, Carta> Cartas;
    #endregion

    private BaseDatos()
    {
        InitializeDataBase();
        this.assets = null;
        this.usuarioActual = null;
        this.numJugadores = 0;
		this.jugadores = new List<Jugador>();
		Cartas = new Dictionary<int, Carta>();
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
        ObtenerAssets(callback);
    }

    private void InitializeDataBase()
    {
        //TODO quizas la parte de base de datos en el futuro la ponga en una clase aparte.
        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://battle-galaxy-cda70.firebaseio.com/");

        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private void ObtenerAssets(CallBack callback)
    {
        reference.Child("assets").GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                Debug.Log("Error al recoger los assets");
                assets = null;
            }
            else if (task.IsCompleted)
            {
                //Assigno los assets a una variable global
                assets = task.Result;
                callback.Invoke();
            }
        });
    }
	public void RecogerJugador(string userId, SesionUsuario.CallBack callback)
    {
        this.userIDActual = userId;
		reference.Child("users").Child(userId).GetValueAsync().ContinueWith(task => {
			if (task.IsFaulted)
			{
				Debug.Log("Error al recoger el jugador");
			}
			else if (task.IsCompleted)
			{
				//Recojo los datos del jugador
				usuarioActual = task.Result;
				ObtenerDatosJugador(callback,usuarioActual);
			}
		});
        //callBack = callback;
    }


    /*public void RecogerJugador(string userId, SesionUsuario.CallBack callback)
    {
        this.userIDActual = userId;
        reference.Child("users").Child(userId)
        .ValueChanged += RecogerUsuario;
        callBack = callback;
    }*/

    public void AñadirWelcomePackJugador(Jugador jugador)
    {
        List<string> idCartasWelcomePack = ObtenerIdsAssetsAleatorios();
        AñadirCartasJugador(jugador, idCartasWelcomePack);
		AñadirItemsJugador (jugador);
    }

    private List<String> ObtenerIdsAssetsAleatorios()
    {
        var json = assets.Value as Dictionary<string, object>;
        List<string> keyList = new List<string>(json.Keys);
        List<string> idCartasWelcomePack = new List<string>();
		System.Random rnd = new System.Random();
        for (int i = 0; i < 8; i++)
        {
			int numCarta = rnd.Next(0, json.Count);
			string idAssetRandom = keyList[numCarta];
            idCartasWelcomePack.Add(idAssetRandom);
        }
        return idCartasWelcomePack;
    }

    public void CrearJugador(string userId, SesionUsuario.CallBack callBack)
    {
        Debug.Log("Crear jugador");
        this.userIDActual = userId;
        AñadirJugador(new Jugador("Low"));
        AñadirWelcomePackJugador(Local);
        AñadirJugadorBaseDatos(userId,Local);
        callBack.Invoke();
    }
		
	/*public void RecogerUsuario(object sender, ValueChangedEventArgs args)
	{
		if (args.DatabaseError != null)
		{
			Debug.LogError(args.DatabaseError.Message);
			return;
		}
		usuarioActual = args.Snapshot;
		ObtenerDatosJugador(usuarioActual);
	}*/

	private void ObtenerDatosJugador(SesionUsuario.CallBack callBack,DataSnapshot usuario)
	{
		Debug.Log("Obtener jugador");
		AñadirJugador(new Jugador("Low"));
		int nivel = ObtenerNivelJugador(usuario);
		List<Carta> cartasJugador = ObtenerCartasJugador(usuario);
		List<Item> itemsJugador = ObtenerItemsJugador (usuario);
		AñadirCartasJugador(Local, cartasJugador);
		AñadirItemsJugador (Local, itemsJugador);
		callBack.Invoke();
	}

    private void AñadirCartasJugador(Jugador jugador, List<String> idCartas)
    {
        foreach (string idAsset in idCartas)
        {    
            AñadirCartaJugador(jugador, CrearCartaJugador(idAsset, null));
        }
        
    }

	private void AñadirItemsJugador(Jugador jugador)
	{
		System.Random rnd = new System.Random();
		for(int i = 0; i < 10; i++)
		{    
			AñadirItemJugador(jugador, GenerarItemAleatorio(rnd));
		}

	}

    private void AñadirCartasJugador(Jugador jugador, List<Carta> cartas)
    {
        foreach (Carta carta in cartas)
        {
            AñadirCartaJugador(jugador, carta);
        }
    }

	private void AñadirItemsJugador(Jugador jugador, List<Item> items)
	{
		foreach (Item item in items)
		{
			AñadirItemJugador(jugador, item);
		}
	}

	private Item GenerarItemAleatorio(System.Random rnd){
		TipoItem tipoItem = (TipoItem)rnd.Next(0, 2);
		int cantidad = rnd.Next (50, 80);
		string rutaImagen;
		//TODO mejorar este hardcode
		if (tipoItem.Equals (TipoItem.Material)) {
			rutaImagen = "Sprites/Recursos/Componentes/trebol";
		} else {
			rutaImagen = "Sprites/Recursos/Componentes/Poción_azul";
		}
		return new Item (tipoItem, rutaImagen,cantidad);
	}

    private Carta CrearCartaJugador(string idAsset, Progreso progreso)
    {
        string assetJSON = assets.Child(idAsset).GetRawJsonValue();
        CartaAsset asset = JsonUtility.FromJson<CartaAsset>(assetJSON);
        Carta carta = new Carta(idAsset, asset);
		Cartas.Add (carta.ID, carta);
        if(progreso != null)
            carta.Progreso = progreso;
        return carta;
    }

	private Item CrearItemJugador(int tipoItem, string rutaImagen, int cantidad)
	{
		TipoItem tipo = (TipoItem)tipoItem;
		Item item = new Item (tipo,rutaImagen,cantidad);
		return item;
	}

    private void AñadirCartaJugador(Jugador jugador, Carta carta)
    {
        jugador.AñadirCarta(carta);
    }

	private void AñadirItemJugador(Jugador jugador, Item item)
	{
		jugador.AñadirItem(item);
	}

    private void AñadirJugadorBaseDatos(string userID, Jugador jugador)
    {
        reference.Child("users").Child(userID).SetValueAsync(jugador.ToDictionary());
    }

    private List<Carta> ObtenerCartasJugador(DataSnapshot usuario)
    {
        List<Carta> cartasJugador = new List<Carta>();
		var rawjson = JSONUtils.StringToJSON(usuario.Child("cartas").GetRawJsonValue());
        for (int i = 0; i < rawjson.Count; i++)
        {
            string idAsset = (string)usuario.Child("cartas").Child(i.ToString()).Child("asset").GetValue(true);
			var progresoJSON = JSONUtils.StringToJSON (usuario.Child ("cartas").Child (i.ToString ()).Child ("progreso").GetRawJsonValue ());
			Progreso progreso = new Progreso ();
			progreso.Material = Int32.Parse (progresoJSON ["material"]);
			progreso.Pocion = Int32.Parse (progresoJSON ["pocion"]);
            //Progreso progreso = JsonUtility.FromJson<Progreso>(usuario.Child("cartas").Child(i.ToString()).Child("progreso").GetRawJsonValue());
            cartasJugador.Add(CrearCartaJugador(idAsset, progreso));
        }
        return cartasJugador;
    }

	private List<Item> ObtenerItemsJugador(DataSnapshot usuario)
	{
		List<Item> itemsJugador = new List<Item>();
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

    private int ObtenerNivelJugador(DataSnapshot usuario)
    {
        return Convert.ToInt32(usuario.Child("nivel").GetValue(true));
    }


    private void AñadirJugador(Jugador jugador)
    {
		jugadores.Add (jugador);
    }

	private void EliminarEnemigo(){
		foreach (Carta carta in Enemigo.Cartas()) {
			Cartas.Remove (carta.ID);
		}
		jugadores.Remove (Enemigo);
	}

	public void ActualizarItemCarta(Carta carta,Item item){
		Local.EliminarItem (item);
		ActualizarItemsBaseDatos ();
		ActualizarCartaBaseDatos (carta);
	}

	public void ActualizarCartaBaseDatos(Carta carta){
		int indiceCarta = Local.BuscarPosicionCarta (carta);
		ReferenciaCartas().Child (indiceCarta.ToString ()).SetValueAsync (carta.ToDictionary ());
	}

	private void ActualizarItemsBaseDatos(){
		
		ReferenciaItems().SetValueAsync (Local.ItemsToDictionary ());
	}

	private void ActualizarCartasBaseDatos(){

		ReferenciaCartas().SetValueAsync (Local.CartasToDictionary ());
	}

    public void GuardarCarta(string familia,CartaAsset asset)
    {
		ReferenciaAssets().Push().SetRawJsonValueAsync(JsonUtility.ToJson(asset));
        Debug.Log("Guardar carta ok");
    }

	public KeyValuePair<string,CartaAsset> BuscarEvolucion(Familia familia, int evolucion){
		
		var json = assets.Value as Dictionary<string, object>;
		List<string> keyList = new List<string>(json.Keys);
		KeyValuePair<string,CartaAsset> evolucionEncontrada = new KeyValuePair<string, CartaAsset> ("",null);
		foreach(string idAsset in keyList)
		{
			string assetJSON = assets.Child(idAsset).GetRawJsonValue();
			CartaAsset asset = JsonUtility.FromJson<CartaAsset>(assetJSON);
			if (asset.Evolucion == (evolucion+1) && asset.Familia.Equals (familia)) {
				evolucionEncontrada = new KeyValuePair<string, CartaAsset> (idAsset,asset);
			}

		}
		return evolucionEncontrada;
	}

    //public bool Carga

    public Jugador Enemigo
    {
        get
        {
            return jugadores[1];
        }
    }

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

	public void CrearJugadorEnemigo(){
		AñadirJugador(new Jugador("Top"));
		AñadirWelcomePackJugador(Enemigo);
	}

	public void Clear(){
		Local.Reset ();
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

	private DatabaseReference ReferenciaItems(){
		return reference.Child ("users").Child (userIDActual).Child ("items");
	}

}
