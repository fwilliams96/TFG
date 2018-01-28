using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Global {

    public static class TIPO_CARTA
    {
        public const int Fuego = 0;
        public const int Tierra = 1;
        public const int Agua = 2;
        public const int Electrica = 3;
        public const int Fusion = 4;
        public const int Magica = 5;
        public const int Ancestral = 6;
    }

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

        
    }
}
