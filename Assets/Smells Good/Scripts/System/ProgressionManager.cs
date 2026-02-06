using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressionManager : MonoBehaviour
{
    public static bool Completed;
    bool HaveFinished;
    public float ProgressionSpeedNormal;
    public float ProgressionSpeedSpeeding;
    public float CurrentSpeed;
    public float Progress = 0;
    public float ProgressToComplete = 20;
    public Slider ProgressSlider;
    [SerializeField] ProgressDifficulty[] progressDifficulties; 
    
    public PlayerFreeFall playerController;
    float CurrentTargetDifficulty;
    int CurrentDifficulty;

    private void Start()
    {
        ProgressSlider.minValue = 0;
        ProgressSlider.maxValue = 20;
        Completed = false;
        CurrentTargetDifficulty = progressDifficulties[CurrentDifficulty].AtProgress;
        ProgressSlider.maxValue = ProgressToComplete;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Progress += CurrentSpeed * Time.deltaTime;
        ProgressSlider.value = Progress;
    }

    private void Update()
    {
        if(Progress >= ProgressToComplete)
        {
            if (!HaveFinished)
            {
                HaveFinished = true;
                GameEnds();
            }
        }

        if (Progress >= CurrentTargetDifficulty && CurrentTargetDifficulty != progressDifficulties[progressDifficulties.Length - 1].AtProgress)
        {
            CurrentDifficulty++;
            CurrentTargetDifficulty = progressDifficulties[CurrentDifficulty].AtProgress;

            Spawner[] spawners = FindObjectsOfType<Spawner>();

            for (int s = 0; s < spawners.Length; s++)
            {
                spawners[s].MinSpawnTime *= progressDifficulties[CurrentDifficulty].SpawnTimeReducer;
                spawners[s].MaxSpawnTime *= progressDifficulties[CurrentDifficulty].SpawnTimeReducer;
            }
        }

    }

    void GameEnds()
    {
        playerController.GetComponent<Animator>().Play("SkyDive_IdleToSpeeding");
        playerController.GetComponent<Animator>().SetTrigger("Speeding");
        Completed = true;
        playerController.Finish ();
        StartCoroutine (Fade ());
    }

    IEnumerator Fade (){
        yield return new WaitForSeconds (3f);
        Camera.main.GetComponent<Animator>().SetTrigger("FadeOut");
        Animator uiAnimator = GameObject.Find ("Transition").GetComponent <Animator>();
        uiAnimator.SetTrigger ("FadeIn");
    }
}

[System.Serializable]
public class ProgressDifficulty
{
    public float AtProgress;
    [Range(0, 0.99f)] public float SpawnTimeReducer;
}
