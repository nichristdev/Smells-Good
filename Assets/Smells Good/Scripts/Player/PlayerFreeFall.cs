using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Sirenix.OdinInspector;
public class PlayerFreeFall : MonoBehaviour
{
    [Title("Health")]
    public int Health = 5;

    [Title("Min-Max Borders")]
    [SerializeField] float Y_Top;
    [SerializeField] float Y_Bottom;

    [Title("Movement")]
    public float LeftRightSpeed;
    float FirstLeftRightSpeed;
    public float FloatSpeed;
    float FirstFloatSpeed;
    public float MoveDownSpeed;
    float FirstMoveDownSpeed;

    [HideInInspector] public bool CanBeHurt = true;
    bool Damaged;
    bool UsingShield;
    bool UsingSpeed;
    float NextShieldTime;
    float NextSpeedTime;
    [HideInInspector] public bool CanMove = true;

    SpriteRenderer spriteRenderer;
    ProgressionManager progression;
    FreeFallUI freeFallUI;
    Rigidbody2D rb;
    Animator anim;
    float MoveInput;
    bool ShutingDownAudio;
    InputManager inputManager;

    private void Awake()
    {
        //Refrence variable
        rb = GetComponent<Rigidbody2D>();
        freeFallUI = FindObjectOfType<FreeFallUI>();
        anim = GetComponent<Animator>();
        progression = FindObjectOfType<ProgressionManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        inputManager = FindObjectOfType<InputManager>();
    }

    private void Start()
    {
        inputManager.StopShakeController();
        freeFallUI.UpdateHealthSlider(Health, 0, 5);
        AudioManager.Instance.MainMixer.SetFloat("MainVolume", 1);

        CanBeHurt = true;
        CanMove = true;

        FirstLeftRightSpeed = LeftRightSpeed;
        FirstMoveDownSpeed = MoveDownSpeed;
        FirstFloatSpeed = FloatSpeed;
    }

    private void Update()
    {
        MoveInput = inputManager.inputMode == InputMode.Windows ? inputManager.MainInput.Main.Walk.ReadValue <Vector2> ().x : inputManager.joystick.Horizontal;

        anim.SetBool("Turning", MoveInput != 0 && (inputManager.inputMode == InputMode.Windows ? (inputManager.MainInput.Main.Walk.ReadValue<Vector2>().x != 0) : (inputManager.joystick.Vertical > -0.2f)));

        if (!Damaged)
        {
            spriteRenderer.flipX = MoveInput < 0;
        }

        if(UsingShield && Time.time >= NextShieldTime)
        {
            if (!Damaged)
            {
                CloseShield();
            }
        }

        if(UsingSpeed && Time.time >= NextSpeedTime)
        {
            SetSpeedDefault();
        }

        if(ShutingDownAudio)
        {
            float FirstVol;
            AudioManager.Instance.MainMixer.GetFloat("MainVolume", out FirstVol);
            AudioManager.Instance.MainMixer.SetFloat("MainVolume", Mathf.Lerp(FirstVol, -50, 0.2f * Time.deltaTime));
        }
    }

    public void Finish (){
        rb.gravityScale = 1f;
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        this.enabled = false;
        MoveInput = 0;
    }

    private void FixedUpdate()
    {
        anim.SetBool("Speeding", (inputManager.inputMode == InputMode.Windows ? (inputManager.MainInput.Main.Walk.ReadValue <Vector2> ().y < 0) : (inputManager.joystick.Vertical < -0.3f)) && CanMove);

        if((inputManager.inputMode == InputMode.Windows ? (inputManager.MainInput.Main.Walk.ReadValue <Vector2> ().y < 0) : (inputManager.joystick.Vertical < -0.3f)) && CanMove)
        {
            rb.position = new Vector2(rb.position.x, Mathf.Lerp(rb.position.y, Y_Bottom, Time.fixedDeltaTime * MoveDownSpeed));
            progression.CurrentSpeed = progression.ProgressionSpeedSpeeding;
        }
        else
        {
            rb.position = new Vector2(rb.position.x, Mathf.Lerp(rb.position.y, Y_Top, Time.fixedDeltaTime * FloatSpeed));
            progression.CurrentSpeed = progression.ProgressionSpeedNormal;
        }

        if (CanMove)
        {
            rb.linearVelocity = new Vector2(MoveInput * LeftRightSpeed * Time.fixedDeltaTime, rb.linearVelocity.y);
        }
    }

    public void IncreaseHealth(int Amount)
    {
        if (Health < 5)
        {
            Health += Amount;
            freeFallUI.UpdateHealthSlider(Health, 0, 5);
        }
    }

    public void ReduceHealth(int Reducer)
    {
        if (CanBeHurt)
        {
            ES3.Save<bool>("FailedOutOfTouch", true, filename);
            Health -= Reducer;
            Damaged = true;
            freeFallUI.UpdateHealthSlider(Health, 0, 5);
            anim.Play("SkyDive IdleToHit");

            if (Health <= 0)
            {
                GameObject.Find("Transition").GetComponent<Animator>().SetTrigger("FadeIn");
                Camera.main.GetComponent<Animator>().SetTrigger("FadeOut");
                AudioManager.Instance.PlaySound("Death", 1, 1, false);
                FindObjectOfType<LevelLoader>().CanLoadLevel = false;
                ShutingDownAudio = true;
                Invoke("GameOver", 3);
                inputManager.StartShakeController();
            }

            CanBeHurt = false;

            rb.linearVelocity = Vector2.zero;
            rb.AddForce(Vector2.up * 3, ForceMode2D.Impulse);

            ActivateShield(true, 3, true);
            SlowDownMovement(150, 0.5f, 0.5f, true, 3);

            Material mat = GetComponent<Renderer>().material;
            mat.SetFloat("_HitEffectBlend", 1);
            Invoke("SetMatBack", 0.1f);

            inputManager.ShakeController();
        }
    }

    void SetMatBack()
    {
        Material mat = GetComponent<Renderer>().material;
        mat.SetFloat("_HitEffectBlend", 0);
    }

    public void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SlowDownMovement(float LeftRightSpeedDecreaser, float MoveDownSpeedDecreaser, float FloatSpeedDecreaser, bool UseTime, float Time)
    {
        LeftRightSpeed -= LeftRightSpeedDecreaser;
        MoveDownSpeed -= MoveDownSpeedDecreaser;
        FloatSpeed -= FloatSpeedDecreaser;

        if (UseTime)
        {
            Invoke("SetSpeedDefault", Time);
        }
    }

    public void SetSpeed(float New_LeftRightSpeed, float New_MoveDownSpeed, float New_FloatSpeed, float SpeedTime)
    {
        LeftRightSpeed = New_LeftRightSpeed;
        MoveDownSpeed = New_MoveDownSpeed;
        FloatSpeed = New_FloatSpeed;
        freeFallUI.ToogleSpeedBoost(true, SpeedTime);
        UsingSpeed = true;

        NextSpeedTime = Time.time + SpeedTime;
    }

    void SetSpeedDefault()
    {
        UsingSpeed = false;
        if (UsingShield)
        {
            if (Time.time > NextShieldTime)
            {
                GetComponent<Renderer>().material.SetFloat("_HologramBlend", 0);
            }
        }else
        {
            GetComponent<Renderer>().material.SetFloat("_HologramBlend", 0);
        }

        LeftRightSpeed = FirstLeftRightSpeed;
        MoveDownSpeed = FirstMoveDownSpeed;
        FloatSpeed = FirstFloatSpeed;
        freeFallUI.ToogleSpeedBoost(false, 0);
    }

    string filename
    {
        get { return "SaveFile" + ".es3"; }
    }

    public void ActivateShield(bool UseTime, float ShieldTime, bool FromDamage)
    {
        CanBeHurt = false;

        if (UseTime)
        {
            if (!FromDamage)
            {
                NextShieldTime = Time.time + ShieldTime;
                UsingShield = true;

                if(Damaged)
                {
                    Damaged = false;
                    anim.Play("SkyDive HitToIdle");
                }
            }
            else if(FromDamage)
            {
                Invoke("CloseShieldFromDamage", ShieldTime);
            }
        }

        freeFallUI.ToogleShield(true, ShieldTime);
    }

    void CloseShield()
    {
        UsingShield = false;
        CanBeHurt = true;
        freeFallUI.ToogleShield(false, 0);
        Damaged = false;

        if (UsingSpeed)
        {
            if (Time.time > NextSpeedTime)
            {
                GetComponent<Renderer>().material.SetFloat("_HologramBlend", 0);
            }
        }
        else
        {
            GetComponent<Renderer>().material.SetFloat("_HologramBlend", 0);
        }
    }

    void CloseShieldFromDamage()
    {
        if (!UsingShield && Damaged)
        {
            anim.Play("SkyDive HitToIdle");
            CanBeHurt = true;
            freeFallUI.ToogleShield(false, 0);
            Damaged = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(new Vector2(transform.position.x - 1000, Y_Top), new Vector2(transform.position.x + 1000, Y_Top));
        Gizmos.DrawLine(new Vector2(transform.position.x - 1000, Y_Bottom), new Vector2(transform.position.x + 1000, Y_Bottom));
    }
}
