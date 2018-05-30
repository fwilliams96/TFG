using UnityEngine;
using System.Collections;

public class EfectoMagica
{
	public virtual void ActivateEffect(int specialAmount = 0,Ente target = null)
    {
        Debug.Log("No Spell effect with this name found! Check for typos in CardAssets");
    }
        
}
