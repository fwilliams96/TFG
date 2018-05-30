using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Magica: Ente,ICharacter 
{
    // CONSTRUCTOR
    public Magica(string area,CartaAsset ca) : base(area,ca) { }

}
