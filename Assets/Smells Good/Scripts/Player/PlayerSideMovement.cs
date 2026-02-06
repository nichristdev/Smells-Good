using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerSideMovement : MonoBehaviour
{
    SideMovementManager controller;
    public float Speed;
    bool CanMove = true;
    [SerializeField] bool FacingRight = true;
    float MoveInput;
    [SerializeField] public bool BeingControlled;

    [HideInInspector] public Rigidbody2D rb;
    Animator anim;
    SpriteRenderer spriteRenderer;
    AudioSource WalkSfx;
    InputManager inputManager;

    void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
        CanMove = true;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        controller = GetComponent <SideMovementManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        WalkSfx = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CanMove)
        {
            MoveInput = 
                controller.movable ? 
                    (inputManager.inputMode == InputMode.Windows 
                        ? inputManager.MainInput.Main.Walk.ReadValue <Vector2> ().x 
                    : inputManager.XInput) 
                : 0;

            if (MoveInput > 0)
            {
                FacingRight = true;
            }
            else if (MoveInput < 0)
            {
                FacingRight = false;
            }

            if (MoveInput != 0)
            {
                if (!WalkSfx.isPlaying)
                {
                    WalkSfx.Play();
                    WalkSfx.pitch = Random.Range(0.9f, 1.2f);
                }
            }
            else
            {
                WalkSfx.Stop();
            }

            if (!BeingControlled) { spriteRenderer.flipX = !FacingRight; }
        }else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        if (CanMove && !BeingControlled) 
        { 
            rb.linearVelocity = new Vector2(MoveInput * Speed * Time.fixedDeltaTime, rb.linearVelocity.y); 
            anim.SetBool("Walking", rb.linearVelocity != Vector2.zero && CanMove);
        
        }

    }

    public void StartMove()
    {
        CanMove = true;
    }
    public void StopMove()
    {
        CanMove = false;
        anim.SetBool("Walking", false);
    }
}
