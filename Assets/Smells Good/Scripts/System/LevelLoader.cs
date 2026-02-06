using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public string SceneName;
    public bool SetHaveLand;
    public bool CanLoadLevel = true;

    public void LoadNextLevel()
    {
        if (CanLoadLevel)
        {
            SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
            ProgressionManager.Completed = false;

            if (SetHaveLand)
            {
                SaveManager.Instance.Save("HaveLand", false);
            }
        }
    }

    public void LoadNextLevelTimeline()
    {
        if (CanLoadLevel)
        {
            SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
            ProgressionManager.Completed = false;

            if (SetHaveLand)
            {
                SaveManager.Instance.Save("HaveLand", false);
            }
        }
    }
}
