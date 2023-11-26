using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
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

        Debug.Log("Animation: update -> Idle");
        state = MovementState.idle;

        if(state == MovementState.idle && Input.GetMouseButton(0))
        {
            state = MovementState.charge;
            Debug.Log("Animation: update -> Charge");
        }
        else if (rb.velocity.y > 0.1f) 
        {
            state = MovementState.jump;
        }
        else if (rb.velocity.y < -0.1f)
        {
            state = MovementState.fall;
        }
        

        playerAnimation.SetInteger("state", (int)state);


     
    }
}
