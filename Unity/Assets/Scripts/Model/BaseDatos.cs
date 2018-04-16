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
    private Jugador [] jugadores;
    private int numJugadores;
    private string userIDActual;
    private DataSnapshot usuarioActual;
    private DataSnapshot assets;
    public delegate void CallBack();
    private SesionUsuario.CallBack callBack;
	public Dictionary<int, Carta> Cartas;
    #endregion

    private BaseDatos()
    {
        InitializeDataBase();
        this.assets = null;
        this.usuarioActual = null;
        this.numJugadores = 0;
        this.jugadores = new Jugador[2];
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

    public void Prueba()
    {
        Dictionary<string, System.Object> dict = new Dictionary<string, System.Object>();
        dict.Add("jugador", new Jugador().ToDictionary());
        reference.Child("pruebas2").Push().SetValueAsync(dict);
    }

    public void RecogerJugador(string userId, SesionUsuario.CallBack callback)
    {
        this.userIDActual = userId;
        reference.Child("users").Child(userId)
        .ValueChanged += RecogerUsuario;
        callBack = callback;
    }

    public void AñadirWelcomePackJugador(Jugador jugador)
    {
        //while (assets == null) ;
        //var json = JSONUtils.StringToJSON(assets.GetRawJsonValue());
        List<string> idCartasWelcomePack = ObtenerIdsAssetsAleatorios();
        AñadirCartasJugador(jugador, idCartasWelcomePack);
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
        AñadirJugador(new Jugador("Top"));
        //TODO aquí finalmente solo deberán añadirse las 8 cartas de welcome pack al jugador que se acaba de registrar. No a ambos.
        AñadirWelcomePackJugador(Local);
		AñadirWelcomePackJugador(Enemigo);
        AñadirJugadorBaseDatos(userId,Local);
        //AñadirWelcomePackJugador(Enemigo);
        callBack.Invoke();
    }

    private void AñadirCartasJugador(Jugador jugador, List<String> idCartas)
    {
        foreach (string idAsset in idCartas)
        {    
            AñadirCartaJugador(jugador, CrearCartaJugador(idAsset, null));
        }
        
    }

    private void AñadirCartasJugador(Jugador jugador, List<Carta> cartas)
    {
        foreach (Carta carta in cartas)
        {
            AñadirCartaJugador(jugador, carta);
        }
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

    private void AñadirCartaJugador(Jugador jugador, Carta carta)
    {
        jugador.AñadirCarta(carta);
    }

    private void AñadirJugadorBaseDatos(string userID, Jugador jugador)
    {
        reference.Child("users").Child(userID).SetValueAsync(jugador.ToDictionary());
    }

    public void RecogerUsuario(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        usuarioActual = args.Snapshot;
        ObtenerDatosJugador(usuarioActual);
    }

    private void ObtenerDatosJugador(DataSnapshot usuario)
    {
        //while (assets == null) ;
        AñadirJugador(new Jugador("Low"));
        AñadirJugador(new Jugador("Top"));
        int nivel = ObtenerNivelJugador(usuario);
        List<Carta> cartasJugador = ObtenerCartasJugador(usuario);
        AñadirCartasJugador(Local, cartasJugador);
        //TODO solo deberá ser al usuario que se ha logueado
        //AñadirCartasJugador(Enemigo, cartasJugador);
		AñadirWelcomePackJugador(Enemigo);
        callBack.Invoke();
    }

    private List<Carta> ObtenerCartasJugador(DataSnapshot usuario)
    {
        List<Carta> cartasJugador = new List<Carta>();
        var idCartas = usuario.Child("cartas").Value as Dictionary<string, object>;
        string raw = usuario.Child("cartas").GetRawJsonValue();
        var rawjson = JSONUtils.StringToJSON(raw);
        Debug.Log("num: " + rawjson.Count);
        Debug.Log("1: " + rawjson[0]["asset"]);
        Debug.Log(usuario.Child("cartas").Child("0").GetRawJsonValue());
        //var variable = usuario.Child("cartas").GetValue(true) as Dictionary<string, object>;
        for (int i = 0; i < rawjson.Count; i++)
        {
            string idAsset = (string)usuario.Child("cartas").Child(i.ToString()).Child("asset").GetValue(true);
            Progreso progreso = JsonUtility.FromJson<Progreso>(usuario.Child("cartas").Child(i.ToString()).Child("progreso").GetRawJsonValue());
            cartasJugador.Add(CrearCartaJugador(idAsset, progreso));
        }

        /*List<string> keyList = new List<string>(idCartas.Keys);
        foreach (string idCarta in keyList)
        {
            string idAsset = (string)usuario.Child("cartas").Child(idCarta.ToString()).Child("asset").GetValue(true);
            Progreso progreso = JsonUtility.FromJson<Progreso>(usuario.Child("cartas").Child(idCarta.ToString()).Child("progreso").GetRawJsonValue());
            cartasJugador.Add(CrearCartaJugador(idAsset, progreso));
        }*/
        return cartasJugador;
    }

    private int ObtenerNivelJugador(DataSnapshot usuario)
    {
        return Convert.ToInt32(usuario.Child("nivel").GetValue(true));
    }


    private void AñadirJugador(Jugador jugador)
    {
        jugadores[numJugadores] = jugador;
        numJugadores += 1;
    }

    public void GuardarCarta(string familia,CartaAsset asset)
    {
        //reference.Child("asset").Child(familia).SetRawJsonValueAsync(cartaJSON);
        reference.Child("assets").Push().SetRawJsonValueAsync(JsonUtility.ToJson(asset));
        Debug.Log("Guardar carta ok");
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
        return jugadores;
    }

    public bool BaseDatosInicializada
    {
        get
        {
            return assets != null;
        }
    }

}
