using UnityEngine;
using System.Collections;
using DG.Tweening;

// this class should be attached to the deck
// generates new cards and places them into the hand
public class PlayerDeckVisual : MonoBehaviour {

    public AreaPosition owner;
    public float HeightOfOneCard = 0.012f;

    private int cartasEnMazo = 0;
    public int CartasEnMazo
    {
        get { return cartasEnMazo; }

        set
        {
            cartasEnMazo = value;
            transform.position = new Vector3(transform.position.x, transform.position.y, -HeightOfOneCard * value);
        }
    }

    void Start()
    {
    }

    
   
}
