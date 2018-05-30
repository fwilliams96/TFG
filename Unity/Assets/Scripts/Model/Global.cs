using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Global {

    public static class CARTAS
    {
        public static string[] FAMILIAS = { "Agua", "Fuego", "Tierra", "Aire", "Magica", "Fusion", "Ancestral" };

        public static class FAMILIA
        {
			public static string[] AIRE = { "Hipogrifo", "Arpia","Tengu"};
            public static string[] AGUA = { "Basilisco", "Sirenita","Triton"};
			public static string[] FUEGO = { "Furaibi" ,"Khalkotauroi"};
            public static string[] TIERRA = { "Ciclope", "Enano", "Elfo"};
            public static string[] MAGICA = { "Destructor", "Espejo" };
            public static string[] FUSION = {  };
            public static string[] ANCESTRAL = { "Dragon"};
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
