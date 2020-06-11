using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static bool disableAudio;

    void Awake()
    {
        disableAudio = Convert.ToBoolean(PlayerPrefs.GetInt("DisableAudio"));
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
        }
    }

    public void Play(string name)
    {
        if (disableAudio)
            return;

        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound \"" + name + "\" not found");
            return;
        }
        s.source.pitch = UnityEngine.Random.Range(s.pitchRange.x, s.pitchRange.y);
        s.source.Play();
    }

    public void DisableAudio(bool disabled)
    {
        disableAudio = disabled;
        PlayerPrefs.SetInt("DisableAudio", Convert.ToInt32(disabled));
    }
}
