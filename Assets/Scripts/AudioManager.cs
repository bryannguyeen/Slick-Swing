using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Range(0f, 1f)]
    public float masterVolume;

    public Toggle audioToggle;
    public Sound[] sounds;
    public static bool mute;

    void Awake()
    {
        audioToggle.isOn = GetMutePref();

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
        }
    }

    public void Play(string name)
    {
        if (mute)
            return;

        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound \"" + name + "\" not found");
            return;
        }
        s.source.pitch = UnityEngine.Random.Range(s.pitchRange.x, s.pitchRange.y);
        s.source.volume = s.volume * masterVolume;
        s.source.Play();
    }

    public void DisableAudio(bool isOn)
    {
        mute = isOn;
        SetMutePref(mute);
    }

    bool GetMutePref()
    {
        return Convert.ToBoolean(PlayerPrefs.GetInt("DisableAudio"));
    }

    void SetMutePref(bool mutePref)
    {
        PlayerPrefs.SetInt("DisableAudio", Convert.ToInt32(mutePref));
    }
}
