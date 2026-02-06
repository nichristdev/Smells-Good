using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ParallaxEffect : MonoBehaviour
{
    #region Variables

    [Title("Parallax Effect Settings")]
    public float ParallaxAmount;
    float length, startpos;
    float Height;
    GameObject cam;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        #region Setting Variables

        cam = Camera.main.gameObject;
        startpos = transform.position.x;
        Height = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x;

        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        #region Parallax Effect System

        float temp = (cam.transform.position.x * (1 - ParallaxAmount));
        float distance = (cam.transform.position.x * ParallaxAmount);

        transform.position = new Vector3(startpos + distance, Height, transform.position.z);

        if(temp > startpos + length)
        {
            startpos += length;
        }
        else if (temp < startpos - length)
        {
            startpos -= length;
        }

        #endregion
    }
}
