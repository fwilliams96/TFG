using UnityEditor;

static class CartaUnityIntegration 
{

	[MenuItem("Assets/Create/CardAsset")]
	public static void CreateYourScriptableObject() {
		ScriptableObjectUtility2.CreateAsset<CartaAsset>();
	}

}
