using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;

public class Recursos  {

	public static Dictionary<int, CartaPartida> CartasCreadasEnElJuego = new Dictionary<int, CartaPartida>();

    public static Dictionary<int, Ente> EntesCreadosEnElJuego = new Dictionary<int, Ente>();

    public static void InicializarCartas()
    {
		SubirTodasCartas();
    }

	public static void SubirCarta(string familia, string nombreCartaXML){
		Dictionary<string, Dictionary<string, SimpleJSON.JSONNode>> cartas = new Dictionary<string, Dictionary<string, SimpleJSON.JSONNode>>();
		string cardPath = Application.streamingAssetsPath + "/XML" + "/" + familia + "/" + nombreCartaXML + ".xml";
		var json = JSONUtils.XMLFileToJSON(cardPath);
		Dictionary<string, SimpleJSON.JSONNode> diccionarioTemp = new Dictionary<string, SimpleJSON.JSONNode>();
		diccionarioTemp.Add(nombreCartaXML, json);
		cartas.Add(familia, diccionarioTemp);
		CrearAssetsCartas (cartas);
	}

    private static void SubirTodasCartas()
    {
		Dictionary<string, Dictionary<string, SimpleJSON.JSONNode>> cartas = new Dictionary<string, Dictionary<string, SimpleJSON.JSONNode>>();
        Dictionary<string, SimpleJSON.JSONNode> diccionarioTemp;

        string filePath = Application.streamingAssetsPath + "/XML";
        string cardPath;
        foreach (string familia in Global.CARTAS.FAMILIAS)
        {
            var prop = typeof(Global.CARTAS.FAMILIA).GetField(familia.ToUpper());

            string[] arrayFamilia = (string[])prop.GetValue(null);
            diccionarioTemp = new Dictionary<string, SimpleJSON.JSONNode>();
			Debug.Log(">>>>>>>>>> FAMILIA: " + familia + " <<<<<<<<<<");
            foreach (string carta in arrayFamilia)
            {
                //Vigilar que carta tambien es sensible a mayusculas y minisculas
                cardPath = filePath + "/" + familia + "/" + carta + ".xml";

                if (File.Exists(cardPath))
                {

                    var json = JSONUtils.XMLFileToJSON(cardPath);
                    Debug.Log("CARTA: " + carta);
                    Debug.Log(json.ToString());
                    diccionarioTemp.Add(carta, json);
                }
            }
            cartas.Add(familia, diccionarioTemp);
        }
		CrearAssetsCartas (cartas);
    }

	private static void CrearAssetsCartas(Dictionary<string, Dictionary<string, SimpleJSON.JSONNode>> cartas)
    {
        foreach(string familia in Global.CARTAS.FAMILIAS)
        {
            if (cartas.ContainsKey(familia))
            {
                foreach(KeyValuePair<string, SimpleJSON.JSONNode> entrada in cartas[familia])
                {
					CartaBase asset = new CartaBase();
                    string carpetaCarta = obtenerFormatoNombreCorrectoDirectorio(entrada.Key);
                    string nombre = entrada.Value["carta"]["delante"]["titulo"];
					asset.Nombre = nombre;

                    Familia tipoCarta = obtenerTipoCarta(entrada.Value["carta"]["delante"]["tipo"]);
					asset.Familia = tipoCarta;

					if (asset.Familia.Equals (Familia.Magica))
						asset.Efecto = obtenerEfecto (entrada.Value ["carta"] ["delante"] ["efecto"]);

                    string descripcion = entrada.Value["carta"]["delante"]["descripcion"];
					asset.Descripcion = descripcion;

					asset.InfoCarta = entrada.Value["carta"]["delante"]["infoCarta"];

                    int mana = System.Int32.Parse(entrada.Value["carta"]["delante"]["mana"]);
					asset.CosteMana = mana;

                    string rutaImagen = obtenerRutaImagen(familia, carpetaCarta, entrada.Value["carta"]["delante"]["nombreImagen"]);
					asset.RutaImagenCarta = rutaImagen;

					int defensa = 0;
					int ataque = 0;
					if (!asset.Familia.Equals (Familia.Magica)) {
						defensa = System.Int32.Parse(entrada.Value["carta"]["delante"]["defensa"]);
						ataque = System.Int32.Parse(entrada.Value["carta"]["delante"]["ataque"]);
					}
					asset.Defensa = defensa;
					asset.Ataque = ataque;

					int evolucion = System.Int32.Parse(entrada.Value["carta"]["delante"]["evolucion"]);
					asset.Evolucion = evolucion;

					int idEvolucion = System.Int32.Parse(entrada.Value["carta"]["delante"]["idEvolucion"]);
					asset.IDEvolucion = idEvolucion;

                    GuardarAssetBaseDatos(familia, asset);
                    //GuardarJSONApartirCartaAsset(asset, obtenerRutaJSON(familia, carpetaCarta),nombre+".json");

                }
            }
            
        }

    }

    private static string obtenerFormatoNombreCorrectoDirectorio(string carpeta)
    {
		if(carpeta.Length > 1)
        	return carpeta.Substring(0,1).ToUpper() + carpeta.Substring(1, carpeta.Length-1) ;
		return carpeta;
    }

    private static string obtenerRutaImagen(string familia, string carpetaCarta, string nombreImagen)
    {
        return obtenerRutaFamiliaImagen(familia) + carpetaCarta + "/" + nombreImagen;
    }

    private static string obtenerRutaJSON(string familia, string carpetaCarta, string nombreJSON)
    {
        return "files/"+obtenerCarpetaFamilia(familia) + carpetaCarta + "/" + nombreJSON;
    }

    private static string obtenerRutaJSON(string familia, string carpetaCarta)
    {
        return "files/" + obtenerCarpetaFamilia(familia) + carpetaCarta+"/";
    }
    private static string obtenerRutaFamiliaImagen(string familia)
    {
        return "Sprites/Cartas/" + obtenerCarpetaFamilia(familia);
    }

    private static string obtenerCarpetaFamilia(string familia)
    {
        string carpetaFamilia = "";
        switch (familia.ToLower())
        {
            case Global.CARTAS.TIPO_CARTA.AGUA:
                carpetaFamilia = "Agua/";
                break;
			case Global.CARTAS.TIPO_CARTA.AIRE:
				carpetaFamilia = "Aire/";
				break;
            case Global.CARTAS.TIPO_CARTA.FUEGO:
                carpetaFamilia = "Fuego/";
                break;
            case Global.CARTAS.TIPO_CARTA.TIERRA:
                carpetaFamilia = "Tierra/";
                break;
            case Global.CARTAS.TIPO_CARTA.MAGICA:
                carpetaFamilia = "Magica/";
                break;
            case Global.CARTAS.TIPO_CARTA.ANCESTRAL:
                carpetaFamilia = "Ancestral/";
                break;
            default:
                carpetaFamilia = "";
                break;
        }
        return carpetaFamilia;
    }

	private static Efecto obtenerEfecto(string nombreEfecto){
		nombreEfecto = nombreEfecto.ToLower ();
		nombreEfecto = nombreEfecto.Replace(nombreEfecto[0],nombreEfecto[0].ToString().ToUpper()[0]);
		Efecto efecto;
		switch (nombreEfecto) {
			case Global.MAGICA.TIPO_EFECTO.Destructor:
				efecto = Efecto.Destructor;
				break;
			case Global.MAGICA.TIPO_EFECTO.Espejo:
				efecto = Efecto.Espejo;
				break;
			case Global.MAGICA.TIPO_EFECTO.Mana:
				efecto = Efecto.Mana;
				break;
			case Global.MAGICA.TIPO_EFECTO.Vida:
				efecto = Efecto.Vida;
				break;
			default:
				efecto = Efecto.Ninguno;
				break;
		}

		return efecto;
	}

    private static Familia obtenerTipoCarta(string familia)
    {
        Familia tipo = Familia.Magica;
        switch (familia.ToLower())
        {
			case Global.CARTAS.TIPO_CARTA.AIRE:
				tipo = Familia.Aire;
				break;
            case Global.CARTAS.TIPO_CARTA.AGUA:
                tipo = Familia.Agua;
                break;
            case Global.CARTAS.TIPO_CARTA.FUEGO:
                tipo = Familia.Fuego;
                break;
            case Global.CARTAS.TIPO_CARTA.TIERRA:
                tipo = Familia.Tierra;
                break;
            case Global.CARTAS.TIPO_CARTA.MAGICA:
                tipo = Familia.Magica;
                break;
            case Global.CARTAS.TIPO_CARTA.ANCESTRAL:
                tipo = Familia.Ancestral;
                break;
            default:
				tipo = Familia.Ancestral;
                break;
        }
        return tipo;
    }

    public static void GuardarJSONApartirCartaAsset(CartaBase asset, string rutaArchivo, string nombreArchivo)
    {
        //string path = Application.persistentDataPath;
        string path = (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer ? Application.persistentDataPath : Application.dataPath);
        string ruta = Path.Combine(path, rutaArchivo);
        bool dirExists = Directory.Exists(ruta);
        if (!dirExists)
            Directory.CreateDirectory(ruta);
        Debug.Log("Guardar json");
        string json = JsonUtility.ToJson(asset);
        ruta = Path.Combine(ruta, nombreArchivo);
        //BaseDatos.Instance.GuardarCartaJugador(SesionUsuario.Instance.UserID,json);
        Debug.Log("Ruta: " + ruta);
        File.WriteAllText(ruta, json);
        Debug.Log("Asset guardado con exito");
    }

    public static void GuardarAssetBaseDatos(string familia,CartaBase asset)
    {
        BaseDatos.Instance.GuardarCarta(familia, asset);
    }
}