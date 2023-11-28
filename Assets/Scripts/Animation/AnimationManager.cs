using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public GameObject[] FireParticlesPrefab;

    private Animator playerAnimation;
    private Rigidbody2D rb;

    private enum MovementState
    {
        idle,
        jump,
        fall,
        charge
    };

    // Start is called before the first frame update
    void Start()
    {
        playerAnimation = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        MovementState state;

        state = MovementState.idle;

        if (rb.velocity.y <= 0.1f && Input.GetMouseButton(0))
        {
            state = MovementState.charge;
        }
        else if (rb.velocity.y > 0.1f && !Input.GetMouseButton(0))
        {
            state = MovementState.jump;
        }
        else if (rb.velocity.y < -0.1f)
        {
            state = MovementState.fall;
        }

        playerAnimation.SetInteger("state", (int)state);
    }

    private void SpawnFireParticles()
    {
        foreach (GameObject prefab in FireParticlesPrefab)
        {
            GameObject spawnedObject = Instantiate(prefab);
            
            // Player Leave Trails
            // spawnedObject.transform.position = GameObject.Find("FireSpawnPoint").transform.position;

            spawnedObject.transform.SetParent(GameObject.Find("FireSpawnPoint").transform);
            spawnedObject.transform.localPosition = Vector3.zero;
        }
    }
}
