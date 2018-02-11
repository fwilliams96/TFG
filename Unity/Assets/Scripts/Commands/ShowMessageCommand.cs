using UnityEngine;
using System.Collections;
using System;

public class ShowMessageCommand : Comanda {

    string message;
    float duration;

    public ShowMessageCommand(string message, float duration)
    {
        this.message = message;
        this.duration = duration;
    }

    public override void EmpezarEjecucionComanda()
    {
        MessageManager.Instance.ShowMessage(message, duration);
    }
}
