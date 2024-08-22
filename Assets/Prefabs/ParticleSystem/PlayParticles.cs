using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Quaternion = System.Numerics.Quaternion;

public class PlayParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particle;
    

    public void PlayParticlesSystem()
    {
        Instantiate(_particle, transform.position, quaternion.identity);
    }
}
