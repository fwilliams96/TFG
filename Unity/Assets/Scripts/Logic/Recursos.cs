using System.IO;
using UnityEngine;

public class Recursos  {
    public static void InicializarCartas()
    {
        //string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "/JSON");

        string filePath = Application.streamingAssetsPath +"/XML";
        string cardPath;
        foreach (string familia in Global.CARTAS.FAMILIAS)
        {
            var prop = typeof(Global.CARTAS.FAMILIA).GetField(familia.ToUpper());

            string[] arrayFamilia = (string[])prop.GetValue(null);      

            foreach (string carta in arrayFamilia)
            {
                cardPath = filePath + "/" + familia + "/" + carta + ".xml";
                if (File.Exists(cardPath))
                {
                    var json = Util.XMLFileToJSON(cardPath);
                    Debug.Log(">>>>>>>>>> FAMILIA: " + familia + " <<<<<<<<<<");
                    Debug.Log("CARTA: " + carta);
                    Debug.Log(json.ToString());
                }
            }
        }

    }

    

}