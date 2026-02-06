using UnityEngine.UI;
using UnityEngine;

public class GameSettings : MonoBehaviour {
    public Slider MainVolume, MusicVolume, SFXVolume;
    public const string MAIN = "Main Volume",
                        MUSIC = "Music Volume",
                        SFX = "SFX Volume";
    public static GameSettings MainSetting;

    private void Awake (){
        if (MainSetting == null)MainSetting = this;
        else Destroy (this);
    }

    private void Start() {
        LoadLastSettings ();
    }

    void LoadLastSettings (){
        float mainVol = SaveManager.Instance.Load<float>(MAIN, 5f);
        float musicVol = SaveManager.Instance.Load<float>(MUSIC, 0f);
        float sfxVol = SaveManager.Instance.Load<float>(SFX, 0f);

        MainVolume.value = mainVol
            ;
        MusicVolume.value = musicVol;
        SFXVolume.value = sfxVol;

        AudioManager.Instance.MainMixer.SetFloat ("MainVolume", mainVol);
        AudioManager.Instance.MainMixer.SetFloat ("MusicVolume", musicVol);
        AudioManager.Instance.MainMixer.SetFloat ("SfxVolume", sfxVol);
    }

    public void OnMainVolumeChanged (float value){
        AudioManager.Instance.MainMixer.SetFloat ("MainVolume", value);
        SaveManager.Instance.Save(MAIN, value);
    }

    public void OnMusicVolumeChanged (float value){
        AudioManager.Instance.MainMixer.SetFloat ("MusicVolume", value);
        SaveManager.Instance.Save(MUSIC, value);
    }

    public void OnSFXVolumeChanged (float value){
        AudioManager.Instance.MainMixer.SetFloat ("SfxVolume", value);
        SaveManager.Instance.Save(SFX, value);
    }
}