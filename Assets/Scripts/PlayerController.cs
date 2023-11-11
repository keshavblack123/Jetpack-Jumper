using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public GameConstants gameConstants;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private BoxCollider2D boxCollider2D;

    private Vector3 mouseDirection;

    public FloatVariable fuel;
    public FloatVariable jumpForce;
    float delayTime;
    float maxFuel;
    LayerMask jumpableGround;

    private float delay;

    [Header("Game Objects")]
    // public TextMeshProUGUI jumpText;
    // public TextMeshProUGUI fuelText;
    public Texture2D customCursor;

    [Header("Cheat Config (only change during PlayMode)")]
    [Tooltip("Default Value is 30")]
    public float maxJumpForce = 30f;

    [Tooltip("Default Value is 0.1")]
    public float fuelIncrement = 0.1f;

    [Tooltip("Default Value is 30")]
    public float dragValue = 30f;
    Vector3 startingPosition;

    void Start()
    {
        jumpForce.SetValue(gameConstants.startingJumpForce);
        delayTime = gameConstants.delayTime;
        maxFuel = gameConstants.maxFuel;
        jumpableGround = gameConstants.jumpableGround;

        startingPosition = transform.position;

        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();

        fuel.SetValue(maxFuel);
        maxJumpForce = 30f;
        fuelIncrement = 0.05f;
        dragValue = 30f;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        // fuelText.text = "Fuel: " + fuel.ToString();
        // jumpText.text = "Jump: " + jumpForce.ToString();

        // Jumping Mechanics [no fuel logic inside this]
        mouseDirection = getCursorLocation();

        //Temp Fix to prevent sliding
        if (IsGrounded())
        {
            if (!Input.GetMouseButton(0))
            {
                rb.drag = dragValue;
            }
            else
            {
                rb.drag = 0f;
            }
        }
        else
        {
            rb.drag = 0f;
        }
        if (mouseDirection.y < -0.05)
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

            if (fuel.Value >= 0)
            {
                // JumpAction using Mouse Left Click
                if (Input.GetMouseButton(0))
                {
                    if(IsGrounded())
                    {
                    //Click to charge
                    jumpForce.Value += 0.1f;
                    // Debug.Log("Charge" + " " + jumpForce);
                    if (jumpForce.Value > maxJumpForce)
                    {
                        jumpForce.SetValue(maxJumpForce);
                    }
                    }

                    else 
                    {
                        // Freeze player when mouse click down when in air 
                        // as long as fuel >0
                        rb.velocity = Vector3.zero;
                        rb.constraints = RigidbodyConstraints2D.FreezePosition;
                        fuelDrain(0.1f);
                        jumpForce.Value += 0.1f;
    
                    }
                }
            }
            //Release to jump
            if (Input.GetMouseButtonUp(0))
            {
                if(floatState())
                {
                    Jump();
                }
                else 
                {
                    if (jumpForce.Value < fuel.Value)
                    {
                        Jump();
                        fuelDrain(jumpForce.Value);        
                    }                    
                    else
                    {
                        jumpForce.SetValue(maxJumpForce);
                        fuelDrain(fuel.Value);
                        Jump();
                    }
                }

                    // Reset JumpForce after release
                jumpForce.SetValue(5f);
                delay = delayTime;
            }
            
                
    
        }
        else
        {
            Cursor.SetCursor(customCursor, Vector2.zero, CursorMode.Auto);
        }
        // Refuel on the ground after a delay
        if (IsGrounded() && fuel.Value >= 0)
        {
            // Refuel after a delay on the ground
            if (fuel.Value < maxFuel)
            {
                if (delay > 0)
                {
                    delay -= 0.1f;
                }
                if (delay < 0)
                {
                    refuel();
                }
            }
        }

        // // Drain fuel mid air
        // if (!IsGrounded() && fuel.Value > 0 && rb.velocity.y != 0)
        // {
        //     fuelDrain();
        // }
    }

    public void fuelDrain(float drainValue)
    {
        
        if(drainValue <= 50)
        {
            fuel.ApplyChange(-drainValue);
        }
        
    }

    public void refuel()
    {
        fuel.ApplyChange(fuelIncrement);
    }

    public void Jump()
    {
        
        // If mouse is below the player
        if (mouseDirection.y < -0.05)
        {
            Debug.Log("Jump");
            //Unfreeze player Set back rotation constraint
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.AddForce(-1 * mouseDirection * jumpForce.Value, ForceMode2D.Impulse);

            if (mouseDirection.x > 0)
            {
                sprite.flipX = true;
            }
            else
            {
                sprite.flipX = false;
            }
        }
    }

    private Vector3 getCursorLocation()
    {
        // Get Mouse Position on Screen
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 mouseDirection = Input.mousePosition - screenPosition;
        mouseDirection.Normalize();

        return mouseDirection;
    }

    public void ResetPlayer()
    {
        transform.position = startingPosition;
        fuel.SetValue(gameConstants.startingFuel);
        jumpForce.SetValue(gameConstants.startingJumpForce);
    }

    // public void fall()
    // {
    //     rb.constraints = RigidbodyConstraints2D.None;
    //     rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    // }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            boxCollider2D.bounds.center,
            boxCollider2D.bounds.size,
            0,
            Vector2.down,
            0.1f,
            jumpableGround
        );
        return raycastHit.collider != null;
    }


    private bool floatState()
    {
        if(!IsGrounded() && rb.velocity.magnitude == 0 && Input.GetMouseButton(0))
        {
            return true;
        }

        else
        {
            return false;
        }
    }

}
