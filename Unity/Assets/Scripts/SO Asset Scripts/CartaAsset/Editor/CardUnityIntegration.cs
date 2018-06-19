using UnityEngine;
using UnityEditor;

static class CardUnityIntegration 
{

	[MenuItem("Assets/Create/CartaAsset")]
	public static void CreateYourScriptableObject() {
		ScriptableObjectUtility2.CreateAsset<CartaAsset>();
	}

}
