using UnityEngine;
using System.Collections;

public class EndTurnButton : MonoBehaviour {

    public void OnClick()
    {
            ControladorTurno.Instance.EndTurn();
    }

}
