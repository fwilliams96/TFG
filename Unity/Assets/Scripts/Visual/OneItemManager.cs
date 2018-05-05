using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OneItemManager : MonoBehaviour {
	public Item Item;

	[Header("Referencias")]
	public Image ItemImage;
	public Text CantidadText;


	void Awake()
	{
		if (Item != null)
			LeerDatosItem();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LeerDatosItem(){
		Settings settings = Settings.Instance;
		if (settings.Items.Equals (Settings.TIPO_NUMERO.ENTERO))
			CantidadText.text = Item.Cantidad.ToString ();
		else
			CantidadText.text = Settings.ObtenerFraccion (Item.Cantidad, 100);
		ItemImage.sprite = Resources.Load<Sprite>(Item.RutaImagen);
	}
}
