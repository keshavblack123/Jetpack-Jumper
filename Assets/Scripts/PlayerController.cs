using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private BoxCollider2D boxCollider2D;

    private Vector3 mouseDirection;

    [Header("Player Configs")]
    public float jumpForce = 5f;
    public float delayTime = 20;
    public float maxFuel = 50;
    public LayerMask jumpableGround;

    private float fuel;
    private float delay;

    [Header("Game Objects")]
    public TextMeshProUGUI jumpText;
    public TextMeshProUGUI fuelText;
    public Texture2D customCursor;

    [Header("Cheat Config (only change during PlayMode)")]
    [Tooltip("Default Value is 30")]
    public float maxJumpForce = 30f;

    [Tooltip("Default Value is 0.05")]
    public float fuelIncrement = 0.05f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();

        fuel = maxFuel; // Set initial fuel to
        maxJumpForce = 30f;
        fuelIncrement = 0.05f;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        fuelText.text = "Fuel: " + fuel.ToString();
        jumpText.text = "Jump: " + jumpForce.ToString();

        // Jumping Mechanics [no fuel logic inside this]
        mouseDirection = getCursorLocation();
        if (mouseDirection.y < -0.05)
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

            if (IsGrounded() && fuel >= 0)
            {
                // JumpAction using Mouse Left Click
                if (Input.GetMouseButton(0))
                {
                    //Click to charge
                    jumpForce += 0.05f;
                    Debug.Log("Charge" + " " + jumpForce);
                    if (jumpForce > maxJumpForce)
                    {
                        jumpForce = maxJumpForce;
                    }
                }

                //Release to jump
                if (Input.GetMouseButtonUp(0))
                {
                    if (jumpForce < fuel)
                    {
                        Jump();
                    }
                    else
                    {
                        jumpForce = fuel;
                        Jump();
                    }

                    // Reset JumpForce after release
                    jumpForce = 5f;
                    delay = delayTime;
                }
            }
        }
        else
        {
            Cursor.SetCursor(customCursor, Vector2.zero, CursorMode.Auto);
        }

        // Refuel on the ground after a delay
        if (IsGrounded() && fuel >= 0)
        {
            // Refuel after a delay on the ground
            if (fuel < maxFuel)
            {
                if (delay > 0)
                {
                    delay -= 0.05f;
                }
                if (delay < 0)
                {
                    refuel();
                }
            }
        }

        // Drain fuel mid air
        if (!IsGrounded() && fuel > 0 && rb.velocity.y != 0)
        {
            fuelDrain();
        }
    }

    public void fuelDrain()
    {
        if (!IsGrounded())
        {
            fuel -= 0.05f;
            if (fuel < 0)
            {
                fuel = 0;
            }
        }
        else
        {
            fuel -= jumpForce;
        }
    }

    public void refuel()
    {
        fuel += fuelIncrement;
    }

    public void Jump()
    {
        // If mouse is below the player
        if (mouseDirection.y < -0.05)
        {
            rb.AddForce(-1 * mouseDirection * jumpForce, ForceMode2D.Impulse);

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

    public void flip() { }

    public void ResetPlayer() { }

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
}
