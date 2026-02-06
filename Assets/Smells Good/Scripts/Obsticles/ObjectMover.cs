using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    public float WaitTime;
    [SerializeField] float initialInterval = 5f;
    [SerializeField] float intervalDecreaseRate = 0.1f;
    [SerializeField] float minInterval = 0.5f;
    GameObject Warning;
    bool CanMove;

    public Direction MoveDirection;
    public float MoveSpeed;
    public float RotateSpeed;
    bool RotateRight;
    Vector2 MoveVector;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        switch (MoveDirection)
        {
            case Direction.Up:
                MoveVector = new Vector2(0, 1);
                break;
            case Direction.Down:
                MoveVector = new Vector2(0, -1);
                break;
            case Direction.Left:
                MoveVector = new Vector2(-1, 0);
                break;
            case Direction.Right:
                MoveVector = new Vector2(1, 0);
                break;
        }

        Invoke("SetCanMove", WaitTime);
        RotateRight = Random.value > 0.5;
        Destroy(gameObject, 5 + WaitTime);

        if (WaitTime > 0)
        {
            Warning = transform.Find("Warning").gameObject;
            InvokeRepeating("PlayWarningBeep", 0f, initialInterval);
        }
    }

    private void PlayWarningBeep()
    {
        Warning.GetComponent<Animator>().SetTrigger("Beep");
        AudioManager.Instance.PlaySound("WarnBeep", 0.2f, 1, false);
        DecreaseInterval();
    }

    private void DecreaseInterval()
    {
        initialInterval = Mathf.Max(initialInterval - intervalDecreaseRate, minInterval);
        CancelInvoke("PlayWarningBeep");
        InvokeRepeating("PlayWarningBeep", initialInterval, initialInterval);
    }

    void SetCanMove()
    {
        CanMove = true;

        if(WaitTime > 0)
        {
            Warning.SetActive(false);
            CancelInvoke("PlayWarningBeep");
        }
    }

    private void FixedUpdate()
    {
        if (CanMove)
        {
            rb.linearVelocity = MoveVector * MoveSpeed * Time.fixedDeltaTime;

            if (RotateRight)
            {
                transform.Rotate(0, 0, RotateSpeed * Time.fixedDeltaTime);
            }else
            {
                transform.Rotate(0, 0, -RotateSpeed * Time.fixedDeltaTime);
            }
        }
    }
}
