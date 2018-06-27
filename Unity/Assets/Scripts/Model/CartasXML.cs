using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class CartasXML {

    public static class CARTAS
    {
        public static string[] FAMILIAS = { "Agua", "Fuego", "Tierra", "Aire", "Magica", "Ancestral" };
		//Nombre de las cartas para cada familia.
        public static class FAMILIA
        {
			public static string[] AIRE = { "Grifo", "Arpia","Arpia2","Tengu"};
            public static string[] AGUA = { "Basilisco", "Sirena","Sirena2","Triton","Triton2"};
			public static string[] FUEGO = { "Furaribi" ,"Khalkotauroi"};
            public static string[] TIERRA = { "Ciclope", "Enano", "Elfo", "Elfo2"};
            public static string[] MAGICA = { "Destructor", "Espejo" ,"Vida", "Mana" };
            public static string[] ANCESTRAL = { "Dragon"};
        }

        public static class TIPO_CARTA
        {
            public const string FUEGO = "fuego";
            public const string TIERRA = "tierra";
			public const string AIRE = "aire";
            public const string AGUA = "agua";
            public const string MAGICA = "magica";
            public const string ANCESTRAL = "ancestral";
        }
    }

	public static class MAGICA
	{
		public static class TIPO_EFECTO
		{
			public const string Destructor = "destructor";
			public const string Espejo = "espejo";
			public const string Mana = "mana";
			public const string Vida = "vida";
		}
	}
}
