using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        Save<string>("CurrentScene", UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    string filename
    {
        get { return "SaveFile" + ".es3"; }
    }

    public T Load<T>(string Key, T DefaultValue)
    {
        T Output = DefaultValue;

        if (ES3.KeyExists(Key, filename))
        {
            Output =  ES3.Load<T>(Key, filename);
        }

        return Output;
    }

    public void Save<T>(string Key, T value)
    {
        ES3.Save<T>(Key, value, filename);
    }

    public void SaveBool(string Key)
    {
        ES3.Save<bool>(Key, true, filename);
    }
    public void DeleteKey(string Key)
    {
        ES3.DeleteKey(Key, filename);
    }

    public void DeleteFile()
    {
        ES3.DeleteFile(filename);
    }
}
