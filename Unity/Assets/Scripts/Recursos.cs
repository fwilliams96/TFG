using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Recursos  {

    private static Dictionary<string, Dictionary<string, SimpleJSON.JSONNode>> cartas;

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
                    string carpetaCarta = entrada.Key;
                    Debug.Log(entrada.Value);
                    
                    string nombre = entrada.Value["carta"]["delante"]["titulo"];
                    TipoCarta tipoCarta = obtenerTipoCarta(entrada.Value["carta"]["delante"]["tipo"]);
                    string descripcion = entrada.Value["carta"]["delante"]["descripcion"];
                    int mana = System.Int32.Parse(entrada.Value["carta"]["delante"]["mana"]);
                    string rutaImagen = obtenerRutaImagen(familia, carpetaCarta, entrada.Value["carta"]["delante"]["nombreImagen"]);
                    int defensa = System.Int32.Parse(entrada.Value["carta"]["delante"]["defensa"]);
                    int ataque = System.Int32.Parse(entrada.Value["carta"]["delante"]["ataque"]);
                    int evolucion = -1;
                    if(!tipoCarta.Equals(TipoCarta.Ancestral))
                        evolucion = System.Int32.Parse(entrada.Value["carta"]["delante"]["evolucion"]);

                    var asset = ScriptableObject.CreateInstance<CardAsset>();
                    asset.Descripcion = descripcion;
                    asset.TipoDeCarta = tipoCarta;
                    //Cargar imagen a partir de la rutaImagen y setearla en el Sprite de CardAsset
                    asset.ImagenCarta = Resources.Load<Sprite>(rutaImagen);
                    asset.CosteMana = mana;
                    asset.Defensa = defensa;
                    asset.Ataque = ataque;
                    if (evolucion != -1)
                        asset.Evolucion = evolucion;

                    AssetDatabase.CreateAsset(asset, "Assets/Game Assets/"+ obtenerCarpetaFamilia(familia)+ "/" +nombre+".asset");
                }
            }
            
        }

    }

    private static string obtenerRutaImagen(string familia, string carpetaCarta, string nombreImagen)
    {
        string carpetaFamilia = obtenerCarpetaFamilia(familia);
  
        return "Sprites/Cartas/" + carpetaFamilia + "/" + carpetaCarta + "/" + nombreImagen;
    }

    private static string obtenerCarpetaFamilia(string familia)
    {
        string carpetaFamilia = "";
        switch (familia.ToLower())
        {
            case Global.CARTAS.TIPO_CARTA.AGUA:
                carpetaFamilia = "Agua";
                break;
            case Global.CARTAS.TIPO_CARTA.FUEGO:
                carpetaFamilia = "Fuego";
                break;
            case Global.CARTAS.TIPO_CARTA.TIERRA:
                carpetaFamilia = "Tierra";
                break;
            case Global.CARTAS.TIPO_CARTA.ELECTRICIDAD:
                carpetaFamilia = "Electricidad";
                break;
            case Global.CARTAS.TIPO_CARTA.MAGICA:
                carpetaFamilia = "Magica";
                break;
            case Global.CARTAS.TIPO_CARTA.FUSION:
                carpetaFamilia = "Fusion";
                break;
            case Global.CARTAS.TIPO_CARTA.ANCESTRAL:
                carpetaFamilia = "Ancestral";
                break;
            default:
                carpetaFamilia = "";
                break;
        }
        return carpetaFamilia;
    }

    private static TipoCarta obtenerTipoCarta(string familia)
    {
        TipoCarta tipo = TipoCarta.Spell;
        switch (familia.ToLower())
        {
            case Global.CARTAS.TIPO_CARTA.AGUA:
                tipo = TipoCarta.Agua;
                break;
            case Global.CARTAS.TIPO_CARTA.FUEGO:
                tipo = TipoCarta.Agua;
                break;
            case Global.CARTAS.TIPO_CARTA.TIERRA:
                tipo = TipoCarta.Agua;
                break;
            case Global.CARTAS.TIPO_CARTA.ELECTRICIDAD:
                tipo = TipoCarta.Agua;
                break;
            case Global.CARTAS.TIPO_CARTA.MAGICA:
                tipo = TipoCarta.Agua;
                break;
            case Global.CARTAS.TIPO_CARTA.FUSION:
                tipo = TipoCarta.Agua;
                break;
            case Global.CARTAS.TIPO_CARTA.ANCESTRAL:
                tipo = TipoCarta.Agua;
                break;
            default:
                tipo = TipoCarta.Spell;
                break;
        }
        return tipo;
    }
}