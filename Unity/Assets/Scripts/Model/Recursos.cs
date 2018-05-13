using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Recursos  {

    private static Dictionary<string, Dictionary<string, SimpleJSON.JSONNode>> cartas;

    public static Dictionary<int, Carta> CartasCreadasEnElJuego = new Dictionary<int, Carta>();

    public static Dictionary<int, Ente> EntesCreadosEnElJuego = new Dictionary<int, Ente>();

    public static List<CartaAsset> AssetsCreadosCartas = new List<CartaAsset>();

    public static void InicializarJugadores()
    {
        //Players.Instance.Add(BaseDatos.Instance.Local);
        //Players.Instance.Add(BaseDatos.Instance.Enemigo);
    }

    public static void InicializarCartas()
    {
        //BaseDatos.Instance.RecuperarCarta();
        //BaseDatos.Instance.Prueba();
        LeerInformacionCartas();
        CrearAssetsCartas();
        //var asset = LeerCartaAssetApartirJSON("Asset2.json");

    }

    private static void LeerInformacionCartas()
    {
        cartas = new Dictionary<string, Dictionary<string, SimpleJSON.JSONNode>>();
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
    }

    private static void CrearAssetsCartas()
    {
        foreach(string familia in Global.CARTAS.FAMILIAS)
        {
            if (cartas.ContainsKey(familia))
            {
                foreach(KeyValuePair<string, SimpleJSON.JSONNode> entrada in cartas[familia])
                {
                    //Leer todos los datos comunes, si la carta es ancestral no hay evolucion, si tiene ataque y defensa no es magica. En caso de no cumplirse estas cosas se lanzara una excepcion y no se creara el asset
                    //Modelo:
                    //extraerDatosComunes(entrada);
                    //extraerDatosNoComunes(entrada);
                    string carpetaCarta = obtenerFormatoNombreCorrectoDirectorio(entrada.Key);
                    //Debug.Log(entrada.Value);
                    string nombre = entrada.Value["carta"]["delante"]["titulo"];
                    Familia tipoCarta = obtenerTipoCarta(entrada.Value["carta"]["delante"]["tipo"]);
                    string descripcion = entrada.Value["carta"]["delante"]["descripcion"];
                    int mana = System.Int32.Parse(entrada.Value["carta"]["delante"]["mana"]);
                    string rutaImagen = obtenerRutaImagen(familia, carpetaCarta, entrada.Value["carta"]["delante"]["nombreImagen"]);
                    int defensa = System.Int32.Parse(entrada.Value["carta"]["delante"]["defensa"]);
                    int ataque = System.Int32.Parse(entrada.Value["carta"]["delante"]["ataque"]);
                    string fondo = entrada.Value["carta"]["delante"]["fondo"];
                    int evolucion = -1;
                    if(!tipoCarta.Equals(TipoCarta.Ancestral))
                        evolucion = System.Int32.Parse(entrada.Value["carta"]["delante"]["evolucion"]);

                    CartaAsset asset = new CartaAsset();
                    asset.Nombre = nombre;
                    asset.Descripcion = descripcion;
                    asset.Familia = tipoCarta;
					if (asset.Familia.Equals (Familia.Magica))
						asset.Efecto = obtenerEfecto (entrada.Value ["carta"] ["delante"] ["efecto"]);
					asset.RutaImagenCarta = rutaImagen;
                    asset.CosteMana = mana;
                    asset.Defensa = defensa;
                    asset.Ataque = ataque;
                    if (evolucion != -1)
                        asset.Evolucion = evolucion;
                    GuardarAssetBaseDatos(familia, asset);
                    //GuardarJSONApartirCartaAsset(asset, obtenerRutaJSON(familia, carpetaCarta),nombre+".json");
                    AssetsCreadosCartas.Add(asset);

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
            case Global.CARTAS.TIPO_CARTA.FUEGO:
                carpetaFamilia = "Fuego/";
                break;
            case Global.CARTAS.TIPO_CARTA.TIERRA:
                carpetaFamilia = "Tierra/";
                break;
            case Global.CARTAS.TIPO_CARTA.ELECTRICIDAD:
                carpetaFamilia = "Electricidad/";
                break;
            case Global.CARTAS.TIPO_CARTA.MAGICA:
                carpetaFamilia = "Magica/";
                break;
            case Global.CARTAS.TIPO_CARTA.FUSION:
                carpetaFamilia = "Fusion/";
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
		nombreEfecto = nombreEfecto.Replace(nombreEfecto[0],nombreEfecto[0].ToString().ToLower()[0]);
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
				efecto = Efecto.Destructor;
				break;
		}

		return efecto;
	}

    private static Familia obtenerTipoCarta(string familia)
    {
        Familia tipo = Familia.Magica;
        switch (familia.ToLower())
        {
            case Global.CARTAS.TIPO_CARTA.AGUA:
                tipo = Familia.Agua;
                break;
            case Global.CARTAS.TIPO_CARTA.FUEGO:
                tipo = Familia.Fuego;
                break;
            case Global.CARTAS.TIPO_CARTA.TIERRA:
                tipo = Familia.Tierra;
                break;
            case Global.CARTAS.TIPO_CARTA.ELECTRICIDAD:
                tipo = Familia.Electrica;
                break;
            case Global.CARTAS.TIPO_CARTA.MAGICA:
                tipo = Familia.Magica;
                break;
            case Global.CARTAS.TIPO_CARTA.FUSION:
                tipo = Familia.Fusion;
                break;
            case Global.CARTAS.TIPO_CARTA.ANCESTRAL:
                tipo = Familia.Ancestral;
                break;
            default:
                tipo = Familia.Magica;
                break;
        }
        return tipo;
    }

    public static CartaAsset2 LeerCartaAssetApartirJSON(string rutaArchivo)
    {
        string path = (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer ? Application.persistentDataPath : Application.dataPath);
        string ruta = Path.Combine(path, rutaArchivo);
        Debug.Log("Leer json");
        Debug.Log("Ruta: " + ruta);
        if (File.Exists(ruta))
        {
            var json = File.ReadAllText(ruta);
            CartaAsset2 carta = ScriptableObject.CreateInstance<CartaAsset2>();
            JsonUtility.FromJsonOverwrite(json, carta);
            Debug.Log("Asset cargado con exito");
            return carta;
        }
        throw new System.Exception();
        
    }

    public static void GuardarJSONApartirCartaAsset(CartaAsset asset, string rutaArchivo, string nombreArchivo)
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

    public static void GuardarAssetBaseDatos(string familia,CartaAsset asset)
    {
        BaseDatos.Instance.GuardarCarta(familia, asset);
    }
}