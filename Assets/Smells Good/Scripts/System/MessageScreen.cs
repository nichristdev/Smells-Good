using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageScreen : MonoBehaviour
{
    [HideInInspector] public float BlipTime;
    [SerializeField] GameObject CreepyMsg;
    [SerializeField] TextMeshProUGUI CreepyText;
    [SerializeField] GameObject SelfMsg;
    [SerializeField] TextMeshProUGUI SelfText;

    private void Awake()
    {
        CreepyMsg.SetActive(false);
        SelfMsg.SetActive(false);
    }

    public void BlipCreepy(string Message)
    {
        StartCoroutine(BlipCoroutine(Message, MsgType.Creepy));
    }

    public void BlipSelf(string Message)
    {
        StartCoroutine(BlipCoroutine(Message, MsgType.Self));
    }

    IEnumerator BlipCoroutine(string Message, MsgType msgType)
    {
        switch (msgType)
        {
            case MsgType.Creepy:
                CreepyMsg.SetActive(true);
                CreepyText.text = Message;
                break;
            case MsgType.Self:
                SelfMsg.SetActive(true);
                SelfText.text = Message;
                break;
        }

        yield return new WaitForSeconds(BlipTime);

        switch (msgType)
        {
            case MsgType.Creepy:
                CreepyMsg.SetActive(false);
                break;
            case MsgType.Self:
                SelfMsg.SetActive(false);
                break;
        }
    }
}

public enum MsgType
{
    Creepy,
    Self
}
