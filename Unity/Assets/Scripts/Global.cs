using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Global {

    public static class CARTAS
    {
        public static string[] FAMILIAS = { "Agua", "Fuego", "Tierra", "Electricidad", "Magica", "Fusion", "Ancestral" };

        public static class FAMILIA
        {
            public static string[] AGUA = { "Dragon1", "Dragon2","Dragon3" };
            public static string[] FUEGO = { "Dragon1", "Dragon2", "Dragon3" };
            public static string[] TIERRA = { "Dragon1", "Dragon2", "Dragon3" , "Soldado", "Caballero", "Principe"};
            public static string[] ELECTRICIDAD = { "Dragon1", "Dragon2", "Dragon3" };
            public static string[] MAGICA = { "Dragon" };
            public static string[] FUSION = { "Dragon" };
            public static string[] ANCESTRAL = { "Dragon" };
        }

        public static class TIPO_CARTA
        {
            public const string FUEGO = "fuego";
            public const string TIERRA = "tierra";
            public const string AGUA = "agua";
            public const string ELECTRICIDAD = "electricidad";
            public const string FUSION = "fusion";
            public const string MAGICA = "magica";
            public const string ANCESTRAL = "ancestral";
        }


    }
}
