using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class MessageScreenTrigger : MonoBehaviour
{
    public string Message;
    public float TimeToBlip = 0.3f;
    public MsgType msgType;
    [SerializeField] bool AutoAppear;
    [ShowIf("AutoAppear", true)]
    public float WaitTime;
    bool HaveTriggered;

    private void Start()
    {
        if(AutoAppear)
        {
            StartCoroutine(WaitAndShow());
        }
    }

    IEnumerator WaitAndShow()
    {
        yield return new WaitForSeconds(WaitTime);

        ShowMessage();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if (!HaveTriggered)
            {
                HaveTriggered = true;
                ShowMessage();
            }
        }
    }

    public void ShowMessage()
    {
        InputManager input = FindObjectOfType<InputManager>();

        if (input)
        {
            input.ShakeController();
        }

        MessageScreen msgScreen = FindObjectOfType<MessageScreen>();
        msgScreen.BlipTime = TimeToBlip;

        switch (msgType)
        {
            case MsgType.Creepy:
                msgScreen.BlipCreepy(Message);
                break;
            case MsgType.Self:
                msgScreen.BlipSelf(Message);
                break;
        }
    }
}
