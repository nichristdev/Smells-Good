using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSaveDestroyer : MonoBehaviour
{
    [SerializeField] string SaveID;

    private void Awake()
    {
        if(SaveID == "")
        {
            Debug.LogError("Save ID haven't been set!");
            return;
        }

        if(ES3.KeyExists(SaveID))
        {
            if(ES3.Load<bool>(SaveID))
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(WaitFirst());
        }
    }

    IEnumerator WaitFirst()
    {
        yield return null;
        DestroyNow();
    }
    public void DestroyNow()
    {
        SaveManager.Instance.Save(SaveID, true);
        Destroy(gameObject);
    }
}
