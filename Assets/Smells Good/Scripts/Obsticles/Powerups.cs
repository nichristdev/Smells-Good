using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Powerups : MonoBehaviour
{
    public PowerupsType powerupsType;
    [HideIf("powerupsType", PowerupsType.LiveIncrease)]
    public float PowerupEffectDuration;

    [ShowIf("powerupsType", PowerupsType.LiveIncrease)]
    public float PowerupValue;

    [ShowIf("powerupsType", PowerupsType.SpeedBoost)]
    public float LeftRightSpeedValue;
    [ShowIf("powerupsType", PowerupsType.SpeedBoost)]
    public float MoveDownSpeedValue;
    [ShowIf("powerupsType", PowerupsType.SpeedBoost)]
    public float FloatSpeedValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !ProgressionManager.Completed)
        {
            PlayerFreeFall playerFreeFall = collision.GetComponent<PlayerFreeFall>();

            if (playerFreeFall.Health > 0)
            {
                AudioManager.Instance.PlaySound("Powerup", 1, 1, false);
                Material PlayerMat = collision.gameObject.GetComponent<Renderer>().material;

                switch (powerupsType)
                {
                    case PowerupsType.Shield:
                        playerFreeFall.ActivateShield(true, PowerupEffectDuration, false);
                        PlayerMat.SetFloat("_HologramBlend", 1);
                        break;
                    case PowerupsType.LiveIncrease:
                        playerFreeFall.IncreaseHealth((int)PowerupValue);
                        break;
                    case PowerupsType.SpeedBoost:
                        playerFreeFall.SetSpeed(LeftRightSpeedValue, MoveDownSpeedValue, FloatSpeedValue, PowerupEffectDuration);
                        PlayerMat.SetFloat("_HologramBlend", 1);
                        break;
                }

                GetComponent<Collider2D>().enabled = false;
            }

            Material mat = GetComponent<Renderer>().material;
            mat.SetFloat("_HitEffectBlend", 1);
            Invoke("DestroyObject", 0.15f);
        }
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }
}

public enum PowerupsType
{
    Shield,
    LiveIncrease,
    SpeedBoost
}
