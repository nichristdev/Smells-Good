using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Audio;
public class AudioManager : MonoBehaviour
{
    #region Variables

    public static AudioManager Instance { get; private set; }

    [TabGroup("Audio Manager")]
    [Title("Settings")]
    public Sound[] sounds;
    [TabGroup("Audio Manager")]
    public AudioMixer MainMixer;
    [TabGroup("Audio Manager")]
    public AudioMixerGroup audioMixer;
    List<Source> CapturedSources = new List<Source>();

    #endregion

    private void Awake()
    {
        #region Setting The Singleton's Instance

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            return;
        }

        #endregion

        #region Adding Audio Components

        foreach (Sound s in sounds)
        {
            s.audioSource = gameObject.AddComponent<AudioSource>();

            s.audioSource.playOnAwake = s.PlayOnAwake;
            s.audioSource.clip = s.clip;
            s.audioSource.volume = s.Volume;
            s.audioSource.pitch = s.Pitch;
            s.audioSource.loop = s.Loop;
            s.audioSource.outputAudioMixerGroup = audioMixer;
        }

        #endregion

        #region Set Mixers To All AudioSource

        AudioSource[] audios = FindObjectsOfType<AudioSource>();

        for (int i = 0; i < audios.Length; i++)
        {
            if (audios[i].gameObject != gameObject && audios[i].outputAudioMixerGroup == null)
            {
                audios[i].outputAudioMixerGroup = audioMixer;
            }
        }

        #endregion
    }

    private void Start()
    {
        foreach (Sound s in sounds)
        {
            if (s.PlayOnAwake)
            {
                s.audioSource.Play();
            }
        }
    }

    public void PauseAllSound()
    {
        CapturedSources.Clear();
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();

        // Capture the state of each audio source
        for (int i = 0; i < audioSources.Length; i++)
        {
            Source source = new Source();
            source.audioSource = audioSources[i];
            source.IsPlaying = audioSources[i].isPlaying;
            CapturedSources.Add(source);

            // Pause the audio source if it's currently playing
            if (source.IsPlaying)
            {
                source.audioSource.Pause();
            }
        }
    }

    public void ResumeAllSound()
    {
        // Resume only the audio sources that were originally playing
        for (int i = 0; i < CapturedSources.Count; i++)
        {
            if (CapturedSources[i].IsPlaying)
            {
                CapturedSources[i].audioSource.Play();
            }
        }
    }


    public void PlaySoundInButton(string Name)
    {
        PlaySound(Name, 1, 1, false);
    }

    public void PlaySound(string SoundName, float Volume, float Pitch, bool Loop)
    {
        //Play audio function
        AudioSource[] audioSources = GetComponents<AudioSource>();
        int AudioSourceFounded = 0;

        for (int i = 0; i < audioSources.Length; i++)
        {
            if (audioSources[i].clip.name == SoundName)
            {
                audioSources[i].volume = Volume;
                audioSources[i].pitch = Pitch;
                audioSources[i].loop = Loop;

                audioSources[i].Play();

                break;
            }else
            {
                AudioSourceFounded++;
            }

            if(i == audioSources.Length - 1)
            {
                if(AudioSourceFounded == audioSources.Length)
                {
                    Debug.LogError("Audio clip isn't found!");
                }
            }
        }
    }

    public AudioSource GetSound(string SoundName)
    {
        //Get audio function
        AudioSource[] audioSources = GetComponents<AudioSource>();
        int AudioSourceFounded = 0;
        AudioSource FoundedClip = null;

        for (int i = 0; i < audioSources.Length; i++)
        {
            if (audioSources[i].clip.name == SoundName)
            {
                FoundedClip = audioSources[i];
            }
            else
            {
                AudioSourceFounded++;
            }

            if (i == audioSources.Length - 1)
            {
                if (AudioSourceFounded == audioSources.Length)
                {
                    Debug.LogError("Audio clip isn't found!");
                }
            }
        }

        return FoundedClip;
    }
}

[System.Serializable]
public class Sound
{
    [Title("Sound Settings")]
    public AudioClip clip;
    [Range(0f, 1f)]
    public float Volume = 1;
    [Range(-3f, 3f)]
    public float Pitch = 1;
    public bool Loop = false;
    public bool PlayOnAwake = false;

    [HideInInspector] public AudioSource audioSource;
}

public class Source
{
    public AudioSource audioSource;
    public bool IsPlaying;
}

