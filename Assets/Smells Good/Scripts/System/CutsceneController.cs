using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
public class CutsceneController : MonoBehaviour
{
    [SerializeField] TimelineAsset BedLandCutscene;
    [SerializeField] TimelineAsset DissolveCutscene;
    PlayableDirector playableDirector;

    private void Awake()
    {
        playableDirector = GetComponent<PlayableDirector>();

        if (ES3.KeyExists("HaveLand"))
        {
            if (ES3.Load<bool>("HaveLand"))
            {
                SetCutsceneToDissolve();
            }
        }
    }

    public void SetCutsceneToDissolve()
    {
        playableDirector.playableAsset = DissolveCutscene;
        SaveManager.Instance.Save("HaveLand", true);
    }
}
