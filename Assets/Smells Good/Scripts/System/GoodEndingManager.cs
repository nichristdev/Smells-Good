using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GoodEndingManager : MonoBehaviour
{
    [SerializeField] GameObject Border;
    [SerializeField] float MinX = -51.19f;
    PlayerSideMovement sideMovement;
    AudioSource HeavenlySFX;
    Animator anim;
    AudioSource audioSource;
    float FirstSpeed;
    bool SlowingDown;
    bool HaveGoodEnding;

    private void Awake()
    {
        Border.SetActive(false);
        sideMovement = FindObjectOfType<PlayerSideMovement>();
        anim = sideMovement.gameObject.GetComponent<Animator>();
        audioSource = sideMovement.gameObject.GetComponent<AudioSource>();
        HeavenlySFX = GetComponent<AudioSource>();
        FirstSpeed = sideMovement.Speed;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void CancelGoodEnding()
    {
        Border.SetActive(true);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(sideMovement.transform.position.x <= MinX)
        {
            if (!HaveGoodEnding)
            {
                HaveGoodEnding = true;
                GetComponent<PlayableDirector>().Play();
            }
        }

        if (SlowingDown)
        {
            sideMovement.rb.linearVelocity = new Vector2(-sideMovement.Speed * Time.fixedDeltaTime, sideMovement.rb.linearVelocity.y);

            sideMovement.Speed = Mathf.Lerp(sideMovement.Speed, 15, 0.225f * Time.deltaTime);
            anim.speed = sideMovement.Speed / FirstSpeed;
            audioSource.volume = sideMovement.Speed / FirstSpeed;
            HeavenlySFX.volume = Mathf.Lerp(HeavenlySFX.volume, 1, 0.2f * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            SlowingDown = true;
            sideMovement.BeingControlled = true;
            HeavenlySFX.Play();
            Camera.main.GetComponent<Animator>().SetTrigger("FadeOut");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SlowingDown = false;
            sideMovement.Speed = FirstSpeed;
            anim.speed = 1;
            audioSource.volume = 1;
        }
    }
}
