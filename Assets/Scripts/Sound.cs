using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    public Vector2 pitchRange;

    [HideInInspector]
    public AudioSource source;
}
