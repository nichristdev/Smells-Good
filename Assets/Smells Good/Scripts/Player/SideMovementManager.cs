using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

public class SideMovementManager : MonoBehaviour
{
    [SerializeField] Vector2 OutsideSpawnPoint;
    [ShowInInspector] public bool FlowerTaken;
    CinemachineVirtualCamera highestPriorityCamera;

    public LayerMask interactableLayer;
    public float detectionDistance = 1f;
    [HideInInspector]public bool movable = true;
    [SerializeField] bool OutSide;
    bool HaveGoneInside;
    [SerializeField] bool InLevelOne;
    GameObject IntroCutscene;
    Animator anim;
    InputManager inputManager;
        
    void Awake (){
        anim = GetComponent<Animator>();
        IntroCutscene = GameObject.Find("Intro Cutscene");
        inputManager = FindObjectOfType<InputManager>();

        if(IntroCutscene)
        {
            IntroCutscene.SetActive(false);
        }
    }

    private void Start() {
        FlowerTaken = SaveManager.Instance.Load<bool>("FlowerTaken", false);
        HaveGoneInside = SaveManager.Instance.Load<bool>("HaveGoneInside", false);

        anim.Play (FlowerTaken ? "Idle_Side_Flower" : "Walking_Idle");

        if(HaveGoneInside && OutSide)
        {
            transform.position = OutsideSpawnPoint;
            GameObject Intro = GameObject.Find("IntroTransition");


            if (Intro is not null) { Intro.SetActive(false); }
            StartCoroutine(SetCamera());
        }
    }

    IEnumerator SetCamera()
    {
        yield return null;
        // Find all Cinemachine 2D cameras in the scene
        CinemachineVirtualCamera[] allCameras = FindObjectsOfType<CinemachineVirtualCamera>();

        // Compare priorities and find the highest priority camera
        foreach (CinemachineVirtualCamera camera in allCameras)
        {
            if (highestPriorityCamera is null || camera.Priority > highestPriorityCamera.Priority)
            {
                highestPriorityCamera = camera;
            }
        }

        // Reference the highest priority camera to a variable
        if (highestPriorityCamera is not null)
        {
            print(highestPriorityCamera.name);
            // Set the current camera's position to the position of the highest priority camera
            CinemachineBrain Brain = Camera.main.GetComponent<CinemachineBrain>();
            Brain.enabled = false;
            Transform currentCameraTransform = Camera.main.transform;
            currentCameraTransform.position = highestPriorityCamera.transform.position;
            yield return null;
            Brain.enabled = true;
        }
        else
        {
            Debug.Log("No Cinemachine 2D cameras found in the scene.");
        }
    }

    private void Update() {
        CheckInteractable ();
    }

    private void CheckInteractable (){
        bool interactableDetected = Physics2D.OverlapCircle (transform.position, detectionDistance, interactableLayer);
        if ((inputManager.inputMode == InputMode.Windows 
                ? inputManager.MainInput.Main.Interact.IsPressed () 
            : inputManager.isInteracting) && interactableDetected){
            if (!FlowerTaken && OutSide)
            {
                GetComponent<PlayerSideMovement>().StopMove();
                SaveManager.Instance.Save("FlowerTaken", true);
                FlowerTaken = true;

                GoodEndingManager GDM = FindObjectOfType<GoodEndingManager>();
                if(GDM)
                {
                    GDM.CancelGoodEnding();
                }

                if(InLevelOne)
                {
                    if(IntroCutscene)
                    {
                        IntroCutscene.SetActive(true);
                        return;
                    }    
                }

                anim.SetTrigger ("TakeFlower");
            } else if (FlowerTaken && !OutSide)
            {
                SaveManager.Instance.Save("FlowerTaken", false);
                FlowerTaken = false;
                GameObject.Find("CutsceneController").GetComponent<PlayableDirector>().Play();
                Camera.main.GetComponent<Animator>().SetTrigger("FadeOut");
                movable = false;
            }
        }
    }

    public void DestroyFlower()
    {
        ObjectSaveDestroyer[] Flowers = FindObjectsOfType<ObjectSaveDestroyer>();

        for (int i = 0; i < Flowers.Length; i++)
        {
            if(Flowers[i].CompareTag("Flower"))
            {
                Flowers[i].DestroyNow();
                AudioManager.Instance.PlaySound("Flower Whip", 1, 1, false);
                break;
            }
        }
    }

    public void DisableWallCollisions (){

        Collider2D[] colliders = GameObject.FindGameObjectWithTag ("Room Environment").GetComponents <Collider2D>();
        for (int i = 0; i < colliders.Length; i++) {
            colliders[i].enabled = false;
        }
    }
}
