﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour 
{
    public Text MessageText;
    public GameObject MessagePanel;

    public static MessageManager Instance;

    void Awake()
    {
        Instance = this;
        MessagePanel.SetActive(false);
    }

	/// <summary>
	/// Muestra un mensaje en la escena.
	/// </summary>
	/// <param name="Message">Message.</param>
	/// <param name="Duration">Duration.</param>
    public void ShowMessage(string Message, float Duration)
    {
        StartCoroutine(ShowMessageCoroutine(Message, Duration));
    }

	/// <summary>
	/// Courutine que permtie que el mensaje desaparezca cuando transcurra la duracion.
	/// </summary>
	/// <returns>The message coroutine.</returns>
	/// <param name="Message">Message.</param>
	/// <param name="Duration">Duration.</param>
    IEnumerator ShowMessageCoroutine(string Message, float Duration)
    {
        //Debug.Log("Showing some message. Duration: " + Duration);
        MessageText.text = Message;
        MessagePanel.SetActive(true);
        CanvasGroup cg = MessagePanel.GetComponent<CanvasGroup>();
        cg.alpha = 1f;
        yield return new WaitForSeconds(Duration);
        while (cg.alpha > 0)
        {
            cg.alpha -= 0.05f;
            yield return new WaitForSeconds(0.05f);
        }

        MessagePanel.SetActive(false);
        Comandas.Instance.CompletarEjecucionComanda();
    }

}
