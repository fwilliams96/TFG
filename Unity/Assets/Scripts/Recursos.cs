using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Recursos  {

    private static Dictionary<string, Dictionary<string, SimpleJSON.JSONNode>> cartas;

    public static Dictionary<int, Carta> CartasCreadasEnElJuego = new Dictionary<int, Carta>();

    public static Dictionary<int, Criatura> CriaturasCreadasEnElJuego = new Dictionary<int, Criatura>();

    public static List<CartaAsset> AssetsCreadosCartas = new List<CartaAsset>();

    public static void InicializarJugadores()
    {
        Players.Instance.Add(DatosGenerales.Instance.TopPlayer);
        Players.Instance.Add(DatosGenerales.Instance.LowPlayer);
    }

    public static void InicializarCartas()
    {
        //string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "/JSON");
        LeerInformacionCartas();
        CrearAssetsCartas();

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

            foreach (string carta in arrayFamilia)
            {
                //Vigilar que carta tambien es sensible a mayusculas y minisculas
                cardPath = filePath + "/" + familia + "/" + carta + ".xml";
                if (File.Exists(cardPath))
                {

                    var json = Util.XMLFileToJSON(cardPath);
                    Debug.Log(">>>>>>>>>> FAMILIA: " + familia + " <<<<<<<<<<");
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
                    Debug.Log(entrada.Value);
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

                    CartaAsset asset = ScriptableObject.CreateInstance<CartaAsset>();
                    asset.Descripcion = descripcion;
                    asset.Familia = tipoCarta;
                    //Cargar imagen a partir de la rutaImagen y setearla en el Sprite de CardAsset
                    /*if (File.Exists(rutaImagen))
                    {
                        asset.ImagenCarta = Resources.Load<Sprite>(rutaImagen);
                    }*/
                    asset.ImagenCarta = Resources.Load<Sprite>(rutaImagen);
                    asset.CosteMana = mana;
                    if (!"".Equals(fondo))
                    {
                        string rutaImagenFondo = obtenerRutaFamiliaImagen(familia) + fondo;
                        if (File.Exists(rutaImagenFondo))
                        {
                            asset.Fondo = Resources.Load<Sprite>(rutaImagenFondo);
                        }

                    }
                    asset.Defensa = defensa;
                    asset.Ataque = ataque;
                    if (evolucion != -1)
                        asset.Evolucion = evolucion;

                    //AssetDatabase.CreateAsset(asset, "Assets/Game Assets/"+ obtenerCarpetaFamilia(familia) + nombre+".asset");
                    AssetDatabase.CreateAsset(asset, obtenerRutaAsset(familia, carpetaCarta, nombre + ".asset"));
                    AssetsCreadosCartas.Add(asset);
                }
            }
            
        }

    }

    private static string obtenerFormatoNombreCorrectoDirectorio(string carpeta)
    {
        return carpeta.Substring(0,1).ToUpper() + carpeta.Substring(1, carpeta.Length-1) ;
    }

    private static string obtenerRutaImagen(string familia, string carpetaCarta, string nombreImagen)
    {
        return obtenerRutaFamiliaImagen(familia) + carpetaCarta + "/" + nombreImagen;
    }

    private static string obtenerRutaAsset(string familia, string carpetaCarta, string nombreAsset)
    {
        return obtenerRutaFamiliaAsset(familia) + carpetaCarta + "/" + nombreAsset;
    }

    private static string obtenerRutaFamiliaImagen(string familia)
    {
        return "Sprites/Cartas/" + obtenerCarpetaFamilia(familia);
    }

    private static string obtenerRutaFamiliaAsset(string familia)
    {
        return "Assets/Game Assets/" + obtenerCarpetaFamilia(familia);
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
}