using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaceTransition : MonoBehaviour
{
    [SerializeField] Vector2 DoorSize;
    [SerializeField] Transform DoorPoint;
    [SerializeField] LayerMask PlayerMask;
    [SerializeField] bool DetectingPlayer;
    [SerializeField] bool InLevelOne;
    InputManager inputManager;
    bool HaveEnter;

    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
    }

    private void Update()
    {
        DetectingPlayer = Physics2D.OverlapBox(DoorPoint.position, DoorSize, 0, PlayerMask);

        if (DetectingPlayer)
        {
            if ((inputManager.inputMode == InputMode.Windows && inputManager.MainInput.Main.Interact.IsPressed ()) ||
             inputManager.inputMode == InputMode.Android && inputManager.isInteracting)
            {
                if(InLevelOne)
                {
                    if (ES3.KeyExists("FlowerTaken"))
                    {
                        if (ES3.Load<bool>("FlowerTaken") == false)
                        {
                            return;
                        }
                    }else
                    {
                        return;
                    }
                }

                if(!HaveEnter)
                {
                    HaveEnter = true;
                    FindObjectOfType<PlayerSideMovement>().StopMove();
                    GameObject.Find("Transition").GetComponent<Animator>().SetTrigger("FadeIn");
                    SaveManager.Instance.Save("HaveGoneInside", true);
                    Animator anim = Camera.main.GetComponent<Animator>();
                    if (anim) { anim.SetTrigger("FadeOut"); }
                    AudioManager.Instance.PlaySound("Door", 1, 1, false);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if(DoorPoint == null)
        {
            return;
        }

        Gizmos.DrawWireCube(DoorPoint.position, DoorSize);
    }
}
