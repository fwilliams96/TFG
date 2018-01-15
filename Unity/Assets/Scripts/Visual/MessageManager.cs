using UnityEngine;
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

    public void ShowMessage(string Message, float Duration)
    {
        StartCoroutine(ShowMessageCoroutine(Message, Duration));
    }

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
        Command.CommandExecutionComplete();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
            ShowMessage("Your turn", 3f);
        if (Input.GetKeyDown(KeyCode.E))
            ShowMessage("Enemy turn", 3f);
    }
}
