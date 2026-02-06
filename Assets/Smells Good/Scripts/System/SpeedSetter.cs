using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedSetter : MonoBehaviour
{
    [SerializeField] float SetSpeedTo;
    float PreviousSpeed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            PlayerSideMovement SideMovement = collision.GetComponent<PlayerSideMovement>();
            PreviousSpeed = SideMovement.Speed;
            SideMovement.Speed = SetSpeedTo;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerSideMovement SideMovement = collision.GetComponent<PlayerSideMovement>();
            SideMovement.Speed = PreviousSpeed;
        }
    }
}
