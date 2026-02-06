using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class PupilIntro : MonoBehaviour
{
    [SerializeField] string GoToScene;
    VideoPlayer videoPlayer;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        if(GoToScene == "")
        {
            Debug.LogError("Scene haven't been set!");
        }
    }

    private void Start()
    {
        Invoke("StartGame", (float)videoPlayer.length + 1);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(GoToScene);
    }
}
