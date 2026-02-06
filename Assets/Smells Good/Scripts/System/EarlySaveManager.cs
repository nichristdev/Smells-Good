using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EarlySaveManager : MonoBehaviour
{
    [SerializeField] GameObject SaveUI;
    string filename
    {
        get { return "SaveFile" + ".es3"; }
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        if(!ES3.KeyExists("CurrentScene", filename))
        {
            SceneManager.LoadScene("1_1_Intro");
        }else
        {
            yield return new WaitForSeconds(1);

            SaveUI.SetActive(true);
        }
    }
    
    public void StartOver()
    {
        ES3.DeleteFile(filename);
        SaveUI.SetActive(false);
        StartCoroutine(Restart());
    }

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("1_1_Intro");
    }

    public void LoadGame()
    {
        SaveUI.SetActive(false);
        StartCoroutine(Load());
    }

    IEnumerator Load()
    {
        yield return new WaitForSeconds(3);
        string SceneToload = ES3.Load<string>("CurrentScene", filename);
        print(SceneToload);
        SceneManager.LoadScene(SceneToload);
    }
}
