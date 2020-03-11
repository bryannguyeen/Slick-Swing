using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePlayer : MonoBehaviour
{
    public ParticleSystem particles;

    public void PlayParticles(Vector3 position)
    {
        transform.position = position;
        particles.Play();
    }
}
