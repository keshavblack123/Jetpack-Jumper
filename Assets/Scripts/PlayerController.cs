using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.Timeline;
using Unity.Mathematics;
using System.Data.Common;
using System;

public class PlayerController : MonoBehaviour
{
    public GameConstants gameConstants;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;

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
        playerAudio = GetComponent<AudioSource>();

        fuel.SetValue(maxFuel);
        maxJumpForce = 30f;
        dragValue = 30f;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(
            Mathf.Clamp(rb.velocity.x, -maxFuel / 2, maxFuel / 2),
            rb.velocity.y
        );
        mouseDirection = getCursorLocation();

        Cursor.SetCursor(
            mouseDirection.y < -0.05 ? null : customCursor,
            Vector2.zero,
            CursorMode.Auto
        );

        //Temp Fix to prevent sliding
        if (IsGrounded)
        {
            canDoubleJump = true;
            rb.drag = Input.GetMouseButton(0) ? 0f : dragValue;
        }
        else
        {
            rb.drag = 0f;
        }
        if (mouseDirection.y < -0.05)
        {
            if (fuel.Value >= 0)
            {
                // JumpAction using Mouse Left Click
                if (Input.GetMouseButton(0))
                {
                    if (IsGrounded)
                    {
                        //Click to charge
                        jumpForce.Value += 0.1f * Time.deltaTime * 600;
                        if (jumpForce.Value > maxJumpForce)
                        {
                            jumpForce.SetValue(maxJumpForce);
                        }
                    }
                    else
                    {
                        // if canDoubleJump, jump where mouse is (use all maxJumpForce fuel)
                        if (canDoubleJump)
                        {
                            //StartCoroutine(PerformDoubleJump());
                            if (fuel.Value < 20f)
                            {
                                jumpForce.Value = fuel.Value;
                            }
                            else
                            {
                                jumpForce.Value = 20f;
                            }
                            fuelDrain(jumpForce.Value);
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
                    // Fix Moving Platform Bug
                    transform.SetParent(null);
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
            // Refuel on the ground after a delay
            if (IsGrounded && fuel.Value >= 0)
            {
                if (fuel.Value < maxFuel)
                {
                    if (delay > 0)
                    {
                        delay -= 0.1f * Time.deltaTime * 600;
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
        fuel.ApplyChange(fuelIncrement * Time.deltaTime * 500);
    }

    public void Jump()
    {
        // If mouse is below the player
        if (mouseDirection.y < -0.05)
        {
            //Play jump audio here
            playerAudio.PlayOneShot(jumpSound);
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

    private void spawnGroundParticles()
    {
        GameObject instantiatedPrefab = Instantiate(groundParticlePrefab);
        Vector3 gameObjectSize = GetComponent<Renderer>().bounds.size;
        instantiatedPrefab.transform.position =
            transform.position - new Vector3(0, gameObjectSize.y / 2, 0);
        ;
    }

    // Particles and Sound Effect
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
                playerAudio.PlayOneShot(landOnPlatformSound);
            }
        }
        if (collision.gameObject.CompareTag("Limits"))
        {
            playerAudio.PlayOneShot(hitSideWall);
        }
    }

    // Ground Check
    private void OnCollisionStay2D(Collision2D collision)
    {
        bool isContactFromBelow = false;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal == Vector2.up)
            {
                isContactFromBelow = true;
                break;
            }
        }
        bool isGroundCollision = collision.gameObject.layer == LayerMask.NameToLayer("Ground");
        IsGrounded = isContactFromBelow && isGroundCollision;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        IsGrounded = false;
    }

    IEnumerator PerformDoubleJump()
    {
        float desiredJumpForce = Mathf.Min(fuel.Value, 20f);
        jumpForce.Value = desiredJumpForce;
        fuelDrain(jumpForce.Value);
        Jump();
        yield return null; // Wait for the next frame
        jumpForce.SetValue(5f);
        delay = delayTime;
        canDoubleJump = false;
    }
}
