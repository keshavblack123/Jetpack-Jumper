using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFinishedParticles : MonoBehaviour
{
    private float duration;

    // Start is called before the first frame update
    void Start()
    {
        // Get the particle system component
        duration = GetComponent<ParticleSystem>().main.duration;
        Destroy(gameObject, duration + 1f);
    }
}
