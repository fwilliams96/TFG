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

    public static void AñadirCartasAFirebase()
    {
		SubirTodasCartas();
    }

	/// <summary>
	/// Permite subir una carta XML a la base de datos Firebase a partir del nombre de la familia y su nombre.
	/// </summary>
	/// <param name="familia">Familia.</param>
	/// <param name="nombreCartaXML">Nombre carta XM.</param>
	public static void AñadirCartaAFirebase(string familia, string nombreCartaXML){
		Dictionary<string, Dictionary<string, SimpleJSON.JSONNode>> cartas = new Dictionary<string, Dictionary<string, SimpleJSON.JSONNode>>();
		string cardPath = Application.streamingAssetsPath + "/XML" + "/" + familia + "/" + nombreCartaXML + ".xml";
		var json = JSONUtils.XMLFileToJSON(cardPath);
		Dictionary<string, SimpleJSON.JSONNode> diccionarioTemp = new Dictionary<string, SimpleJSON.JSONNode>();
		diccionarioTemp.Add(nombreCartaXML, json);
		cartas.Add(familia, diccionarioTemp);
		CrearAssetsCartas (cartas);
	}

	/// <summary>
	/// Permite subir todas las cartas de los XML que se encuentren en StreamingAssets/XML.
	/// </summary>
    private static void SubirTodasCartas()
    {
		Dictionary<string, Dictionary<string, SimpleJSON.JSONNode>> cartas = new Dictionary<string, Dictionary<string, SimpleJSON.JSONNode>>();
        Dictionary<string, SimpleJSON.JSONNode> diccionarioTemp;

        string filePath = Application.streamingAssetsPath + "/XML";
        string cardPath;
        foreach (string familia in CartasXML.CARTAS.FAMILIAS)
        {
            var prop = typeof(CartasXML.CARTAS.FAMILIA).GetField(familia.ToUpper());

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

	/// <summary>
	/// Permite crear assets de las cartas y subirlas a Firebase.
	/// </summary>
	/// <param name="cartas">Cartas.</param>
	private static void CrearAssetsCartas(Dictionary<string, Dictionary<string, SimpleJSON.JSONNode>> cartas)
    {
        foreach(string familia in CartasXML.CARTAS.FAMILIAS)
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

                }
            }
            
        }

    }

	/// <summary>
	/// Se obtiene el formato del nombre del directorio correcto.
	/// </summary>
	/// <returns>The formato nombre correcto directorio.</returns>
	/// <param name="carpeta">Carpeta.</param>
    private static string obtenerFormatoNombreCorrectoDirectorio(string carpeta)
    {
		if(carpeta.Length > 1)
        	return carpeta.Substring(0,1).ToUpper() + carpeta.Substring(1, carpeta.Length-1) ;
		return carpeta;
    }

	/// <summary>
	/// Permite devolver la ruta de la imagen a partir de su nombre, nombre de carpeta y familia.
	/// </summary>
	/// <returns>The ruta imagen.</returns>
	/// <param name="familia">Familia.</param>
	/// <param name="carpetaCarta">Carpeta carta.</param>
	/// <param name="nombreImagen">Nombre imagen.</param>
    private static string obtenerRutaImagen(string familia, string carpetaCarta, string nombreImagen)
    {
        return obtenerRutaFamiliaImagen(familia) + carpetaCarta + "/" + nombreImagen;
    }
		
	/// <summary>
	/// Devuelve la ruta de la familia de la carta.
	/// </summary>
	/// <returns>The ruta familia imagen.</returns>
	/// <param name="familia">Familia.</param>
    private static string obtenerRutaFamiliaImagen(string familia)
    {
        return "Sprites/Cartas/" + obtenerCarpetaFamilia(familia);
    }

	/// <summary>
	/// Devuelve el nombre de la carpeta de la familia de la carta.
	/// </summary>
	/// <returns>The carpeta familia.</returns>
	/// <param name="familia">Familia.</param>
    private static string obtenerCarpetaFamilia(string familia)
    {
        string carpetaFamilia = "";
        switch (familia.ToLower())
        {
            case CartasXML.CARTAS.TIPO_CARTA.AGUA:
                carpetaFamilia = "Agua/";
                break;
			case CartasXML.CARTAS.TIPO_CARTA.AIRE:
				carpetaFamilia = "Aire/";
				break;
            case CartasXML.CARTAS.TIPO_CARTA.FUEGO:
                carpetaFamilia = "Fuego/";
                break;
            case CartasXML.CARTAS.TIPO_CARTA.TIERRA:
                carpetaFamilia = "Tierra/";
                break;
            case CartasXML.CARTAS.TIPO_CARTA.MAGICA:
                carpetaFamilia = "Magica/";
                break;
            case CartasXML.CARTAS.TIPO_CARTA.ANCESTRAL:
                carpetaFamilia = "Ancestral/";
                break;
            default:
                carpetaFamilia = "";
                break;
        }
        return carpetaFamilia;
    }

	/// <summary>
	/// Obtiene el efecto de la magica.
	/// </summary>
	/// <returns>The efecto.</returns>
	/// <param name="nombreEfecto">Nombre efecto.</param>
	private static Efecto obtenerEfecto(string nombreEfecto){
		nombreEfecto = nombreEfecto.ToLower ();
		Efecto efecto;
		switch (nombreEfecto) {
			case CartasXML.MAGICA.TIPO_EFECTO.Destructor:
				efecto = Efecto.Destructor;
				break;
			case CartasXML.MAGICA.TIPO_EFECTO.Espejo:
				efecto = Efecto.Espejo;
				break;
			case CartasXML.MAGICA.TIPO_EFECTO.Mana:
				efecto = Efecto.Mana;
				break;
			case CartasXML.MAGICA.TIPO_EFECTO.Vida:
				efecto = Efecto.Vida;
				break;
			default:
				efecto = Efecto.Ninguno;
				break;
		}

		return efecto;
	}

	/// <summary>
	/// Devuelve el tipo de familia de la carta.
	/// </summary>
	/// <returns>The tipo carta.</returns>
	/// <param name="familia">Familia.</param>
    private static Familia obtenerTipoCarta(string familia)
    {
        Familia tipo = Familia.Magica;
        switch (familia.ToLower())
        {
			case CartasXML.CARTAS.TIPO_CARTA.AIRE:
				tipo = Familia.Aire;
				break;
            case CartasXML.CARTAS.TIPO_CARTA.AGUA:
                tipo = Familia.Agua;
                break;
            case CartasXML.CARTAS.TIPO_CARTA.FUEGO:
                tipo = Familia.Fuego;
                break;
            case CartasXML.CARTAS.TIPO_CARTA.TIERRA:
                tipo = Familia.Tierra;
                break;
            case CartasXML.CARTAS.TIPO_CARTA.MAGICA:
                tipo = Familia.Magica;
                break;
            case CartasXML.CARTAS.TIPO_CARTA.ANCESTRAL:
                tipo = Familia.Ancestral;
                break;
            default:
				tipo = Familia.Ancestral;
                break;
        }
        return tipo;
    }

	/// <summary>
	/// Permite subir a firebase una carta.
	/// </summary>
	/// <param name="familia">Familia.</param>
	/// <param name="asset">Asset.</param>
    public static void GuardarAssetBaseDatos(string familia,CartaBase asset)
    {
        BaseDatos.Instance.GuardarCarta(familia, asset);
    }
}