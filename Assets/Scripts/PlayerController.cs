using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.Timeline;
using Unity.Mathematics;
using System.Data.Common;

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
    AudioSource playerAudio;
    public AudioClip jumpSound;
    public AudioClip landOnPlatformSound;
    public AudioClip hitSideWall;

    private float delay;
    public bool IsGrounded = true;

    [Header("Game Objects")]
    // public TextMeshProUGUI jumpText;
    // public TextMeshProUGUI fuelText;
    public Texture2D customCursor;
    public GameObject groundParticlePrefab;

    [Header("Cheat Config (only change during PlayMode)")]
    [Tooltip("Default Value is 30")]
    public float maxJumpForce = 30f;

    [Tooltip("Default Value is 0.1")]
    private float fuelIncrement;

    [Tooltip("Default Value is 30")]
    public float dragValue = 30f;
    Vector3 startingPosition;
    private bool canDoubleJump = true;

    void Start()
    {
        jumpForce.SetValue(gameConstants.startingJumpForce);
        delayTime = gameConstants.delayTime;
        maxFuel = gameConstants.maxFuel;
        jumpableGround = gameConstants.jumpableGround;
        fuelIncrement = gameConstants.fuelIncrement;

        startingPosition = transform.position;

        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        playerAudio = GetComponent<AudioSource>();

        fuel.SetValue(maxFuel);
        maxJumpForce = 30f;
        // fuelIncrement = 0.05f;
        dragValue = 30f;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        mouseDirection = getCursorLocation();

        //Temp Fix to prevent sliding
        if (IsGrounded)
        {
            canDoubleJump = true;
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
                    if (IsGrounded)
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
                        // if canDoubleJump, jump where mouse is (use all maxJumpForce fuel)
                        if (canDoubleJump && !IsGrounded && Input.GetMouseButton(0))
                        {
                            //Play jump audio here
                            playerAudio.PlayOneShot(jumpSound);
                            if (fuel.Value < 20f)
                            {
                                jumpForce.Value = fuel.Value;
                                fuelDrain(jumpForce.Value);
                            }
                            else
                            {
                                jumpForce.Value = 20f;
                                fuelDrain(jumpForce.Value);
                            }
                            Jump();
                            jumpForce.SetValue(5f);
                            delay = delayTime;
                            canDoubleJump = false;
                        }
                }
            }
            //Release to jump
            if (Input.GetMouseButtonUp(0) && IsGrounded)
            {
                //Play jump audio here
                playerAudio.PlayOneShot(jumpSound);
                if (jumpForce.Value < fuel.Value)
                {
                    Jump();
                    fuelDrain(jumpForce.Value);
                }
                else
                {
                    jumpForce.SetValue(fuel.Value);
                    fuelDrain(fuel.Value);
                    Jump();
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
        if (IsGrounded && fuel.Value >= 0)
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
    }
    }

    public void fuelDrain(float drainValue)
    {
        if (drainValue <= 50)
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
        // Fix Moving Platform Bug
        transform.SetParent(null);

        // If mouse is below the player
        if (mouseDirection.y < -0.05)
        {
            //Unfreeze player Set back rotation constraint
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.AddForce(-1 * mouseDirection * jumpForce.Value, ForceMode2D.Impulse);
            IsGrounded = false;

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

    // public bool IsGrounded()
    // {
    //     RaycastHit2D raycastHit = Physics2D.BoxCast(
    //         boxCollider2D.bounds.center,
    //         boxCollider2D.bounds.size,
    //         0,
    //         Vector2.down,
    //         0.1f,
    //         jumpableGround
    //     );
    //     return raycastHit.collider != null;
    // }

    private void spawnGroundParticles()
    {
        GameObject instantiatedPrefab = Instantiate(groundParticlePrefab);
        Vector3 gameObjectSize = GetComponent<Renderer>().bounds.size;
        instantiatedPrefab.transform.position =
            transform.position - new Vector3(0, gameObjectSize.y / 2, 0);
        ;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts.Length > 0)
        {
            ContactPoint2D contact = collision.GetContact(0);

            // Collision is at the bottom
            if (
                contact.normal == Vector2.up
                && collision.gameObject.layer == LayerMask.NameToLayer("Ground")
            )
            {
                spawnGroundParticles();
                // Play land on platform sound
                playerAudio.PlayOneShot(landOnPlatformSound);
                IsGrounded = true;
            }
        }
        if (collision.gameObject.CompareTag("Limits"))
        {
            playerAudio.PlayOneShot(hitSideWall);
        }
    }
}
