using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Obsticle : MonoBehaviour
{
    [Title("Settings")]
    public int HealthReduced;
    [SerializeField] string SfxHitName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !ProgressionManager.Completed)
        {
            PlayerFreeFall playerFreeFall = collision.GetComponent<PlayerFreeFall>();

            if (playerFreeFall.Health > 0)
            {
                playerFreeFall.ReduceHealth(HealthReduced);
                AudioManager.Instance.PlaySound(SfxHitName, 1, 1, false);
                GetComponent<Collider2D>().enabled = false;
                Camera.main.GetComponent<Animator>().SetTrigger("Shake");
            }

            Material mat = GetComponent<Renderer>().material;
            mat.SetFloat("_HitEffectBlend", 1);
            Invoke("DestroyObsticle", 0.15f);
        }
    }

    void DestroyObsticle()
    {
        Destroy(gameObject);
    }
}
