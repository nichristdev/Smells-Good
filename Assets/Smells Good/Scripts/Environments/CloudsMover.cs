using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsMover : MonoBehaviour
{
    public float Y_Pos = 16;
    public float Speed;
    public GameObject[] Clouds;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(transform.position.y >= Y_Pos)
        {
            Instantiate(Clouds[Random.Range(0, Clouds.Length)], new Vector2(0, -Y_Pos), Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(transform.position.x, Speed * Time.fixedDeltaTime);
    }
}
