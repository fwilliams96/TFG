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
    public bool crearJugador;
    #endregion

    private BaseDatos()
    {
        InitializeDataBase();
        this.assets = null;
        this.usuarioActual = null;
        this.crearJugador = false;
        this.numJugadores = 0;
        this.jugadores = new Jugador[2];
        ObtenerAssets();
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

    private void InitializeDataBase()
    {
        //TODO quizas la parte de base de datos en el futuro la ponga en una clase aparte.
        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://battle-galaxy-cda70.firebaseio.com/");

        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private void ObtenerAssets()
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
            }
        });
    }

    public void InicializarJugador(string userID)
    {
        if (crearJugador)
        {
            while (assets == null) ;
            Debug.Log("Assets diferent null");
            //CrearJugador(userID);
        }
        else
        {
            this.userIDActual = userID;
            reference.Child("users").Child(userID)
            .ValueChanged += RecogerUsuario;
        }
        
    }

    public void CrearJugador(string UserID)
    {
        this.userIDActual = UserID;
        AñadirJugador(new Jugador());
        AñadirJugador(new Jugador());
        //TODO aquí finalmente solo deberán añadirse las 8 cartas de welcome pack al jugador que se acaba de registrar. No a ambos.
        while (assets == null) ;
        AñadirWelcomePackJugador(Local);
        AñadirJugadorBaseDatos(Local);
        //AñadirWelcomePackJugador(Enemigo);
    }

    public void Prueba()
    {
        Dictionary<string, System.Object> dict = new Dictionary<string, System.Object>();
        dict.Add("jugador", new Jugador().ToDictionary());
        reference.Child("pruebas2").Push().SetValueAsync(dict);
    }

    public void AñadirWelcomePackJugador(Jugador jugador)
    {
        while (assets == null) ;
        //var json = JSONUtils.StringToJSON(assets.GetRawJsonValue());
        List<string> idCartasWelcomePack = ObtenerIdsAssetsAleatorios();
        AñadirCartasJugador(jugador, idCartasWelcomePack);
    }

    private List<String> ObtenerIdsAssetsAleatorios()
    {
        var json = assets.Value as Dictionary<string, object>;
        List<string> keyList = new List<string>(json.Keys);
        List<string> idCartasWelcomePack = new List<string>();
        for (int i = 0; i < 8; i++)
        {
            System.Random rnd = new System.Random();
            int numCarta = rnd.Next(1, json.Count);
            string idAssetRandom = keyList[numCarta];
            idCartasWelcomePack.Add(idAssetRandom);
        }
        return idCartasWelcomePack;
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
        CartaAsset asset = JsonUtility.FromJson<CartaAsset>(assets.Child(idAsset).GetRawJsonValue());
        Carta carta = new Carta(idAsset, asset);
        if(progreso != null)
            carta.Progreso = progreso;
        return carta;
    }

    private void AñadirCartaJugador(Jugador jugador, Carta carta)
    {
        jugador.AñadirCarta(carta);
    }

    private void AñadirJugadorBaseDatos(Jugador jugador)
    {
        reference.Child("users").SetValueAsync(jugador.ToDictionary());
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
        while (assets == null) ;
        AñadirJugador(new Jugador());
        AñadirJugador(new Jugador());
        int nivel = ObtenerNivelJugador(usuario);
        List<Carta> cartasJugador = ObtenerCartasJugador(usuario);
        AñadirCartasJugador(Local, cartasJugador);
        //TODO solo deberá ser al usuario que se ha logueado
        AñadirCartasJugador(Enemigo, cartasJugador);
    }

    private List<Carta> ObtenerCartasJugador(DataSnapshot usuario)
    {
        List<Carta> cartasJugador = new List<Carta>();
        var idCartas = usuario.Child("cartas").Value as Dictionary<string, object>;
        List<string> keyList = new List<string>(idCartas.Keys);
        foreach (string idCarta in keyList)
        {
            string idAsset = (string)usuario.Child("cartas").Child(idCarta).Child("asset").GetValue(true);
            Progreso progreso = JsonUtility.FromJson<Progreso>(usuario.Child("cartas").Child(idCarta).Child("progreso").GetRawJsonValue());
            cartasJugador.Add(CrearCartaJugador(idAsset, progreso));
        }
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

}
