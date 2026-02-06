using XInputDotNetPure;
using UnityEngine;
using System.Collections;

public enum InputMode {
    Windows, Android
}

public class InputManager : MonoBehaviour
{
    public PlayerInput MainInput;
    public InputMode inputMode;
    public float XInput;
    public bool isInteracting;
    [SerializeField]InputButton leftButton, rightButton, interactButton;
    public Joystick joystick;
    PlayerIndex playerIndex;
    
    [Space()]
    [SerializeField]GameObject instruction;
    
    void Awake (){
        MainInput = new PlayerInput ();
        MainInput.Enable ();
    }

    void Start (){
        if (inputMode == InputMode.Android && instruction)
            instruction.SetActive (false);
    }

    private void Update() {
        if (leftButton.pressed && !rightButton.pressed)XInput = -1f;
        else if (rightButton.pressed && !leftButton.pressed)XInput = 1f;
        else if (!rightButton.pressed && !leftButton.pressed)XInput = 0f;
        else if (rightButton.pressed && leftButton.pressed)XInput = 0f;

        isInteracting = interactButton.pressed;
    }

    public void ShakeController()
    {
        StartCoroutine(ShakeControllerCoroutine());
    }

    public IEnumerator ShakeControllerCoroutine (){
        if (Application.platform != RuntimePlatform.Android)
        {
            print("Controller Vibrated!");
            GamePad.SetVibration(playerIndex, 0.1f, 0.1f);
            yield return new WaitForSeconds(0.1f);
            GamePad.SetVibration(playerIndex, 0f, 0f);
        }
    }

    public void StartShakeController()
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            print("Controller Started Vibrating!");
            GamePad.SetVibration(playerIndex, 0.1f, 0.1f);
        }
    }

    public void StopShakeController()
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            print("Controller Stopped Vibrating!");
            GamePad.SetVibration(playerIndex, 0f, 0f);
        }
    }
}
