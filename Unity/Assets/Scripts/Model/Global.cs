using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Global {

    public static class CARTAS
    {
        public static string[] FAMILIAS = { "Agua", "Fuego", "Tierra", "Aire","Electricidad", "Magica", "Fusion", "Ancestral" };

        public static class FAMILIA
        {
			public static string[] AIRE = { "Hipogrifo", "Arpia"};
            public static string[] AGUA = { "Basilisco", "Sirenita"};
            public static string[] FUEGO = { "Dragon1" };
            public static string[] TIERRA = { "Ciclope", "Enano", "Elfo"};
            public static string[] ELECTRICIDAD = { };
            public static string[] MAGICA = { "Destructor" };
            public static string[] FUSION = {  };
            public static string[] ANCESTRAL = { };
        }

        public static class TIPO_CARTA
        {
            public const string FUEGO = "fuego";
            public const string TIERRA = "tierra";
			public const string AIRE = "aire";
            public const string AGUA = "agua";
            public const string ELECTRICIDAD = "electricidad";
            public const string FUSION = "fusion";
            public const string MAGICA = "magica";
            public const string ANCESTRAL = "ancestral";
        }




    }

	public static class MAGICA
	{
		public static class TIPO_EFECTO
		{
			public const string Destructor = "Destructor";
			public const string Espejo = "Espejo";
			public const string Mana = "Mana";
			public const string Vida = "Vida";
		}
	}
}
