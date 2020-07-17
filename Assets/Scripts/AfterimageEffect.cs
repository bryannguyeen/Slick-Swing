using UnityEngine;

public class AfterimageEffect : MonoBehaviour
{
    ParticleSystem afterimageParticleSystem;
    SpriteRenderer playerSprite;
    ParticleSystemRenderer particleRenderer;

    void Start()
    {
        afterimageParticleSystem = GetComponent<ParticleSystem>();
        afterimageParticleSystem.Stop();

        playerSprite = GetComponent<SpriteRenderer>();

        particleRenderer = GetComponent<ParticleSystemRenderer>();
        particleRenderer.material = new Material(Shader.Find("Sprites/Default"))
        {
            mainTexture = playerSprite.sprite.texture
        };
    }

    void Update()
    {
        // only update texture if particle system is currently playing
        if (afterimageParticleSystem.isPlaying)
            particleRenderer.material.mainTexture = playerSprite.sprite.texture;
    }

    public void Play()
    {
        afterimageParticleSystem.Play();
    }

    public void Stop()
    {
        afterimageParticleSystem.Stop();
    }
}
