using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarningManager : MonoBehaviour
{
    [SerializeField] string SceneName;

    private void Awake()
    {
        if(SceneName == "")
        {
            Debug.LogError("Scene haven't been assigned!");
        }
    }

    public void GoToScene()
    {
        SceneManager.LoadScene(SceneName);
    }
}
