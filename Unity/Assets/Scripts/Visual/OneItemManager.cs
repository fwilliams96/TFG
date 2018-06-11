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
		ConfiguracionUsuario settings = ConfiguracionUsuario.Instance;
		if (settings.Items.Equals (ConfiguracionUsuario.TIPO_NUMERO.ENTERO))
			CantidadText.text = ConfiguracionUsuario.ObtenerEntero (Item.Cantidad);
		else if (settings.Items.Equals (ConfiguracionUsuario.TIPO_NUMERO.FRACCION)) {
			CantidadText.text = ConfiguracionUsuario.ObtenerFraccion (Item.Cantidad, 100);
		} else {
			CantidadText.text = ConfiguracionUsuario.ObtenerPorcentaje (Item.Cantidad, 100);
		}
			
		ItemImage.sprite = Resources.Load<Sprite>(Item.RutaImagen);
	}
}
