using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 1200f;
    // Start is called before the first frame update

    public GameObject player;
    public GameObject arrow;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        

    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
        

        // JumpAction using Mouse Left Click
        if (Input.GetMouseButton(0))
        {
            //Click to charge
            jumpForce += 100;
            if (jumpForce>1800f)
                jumpForce = 1800f;
        }

        //Release to jump
        if (Input.GetMouseButtonUp(0))
        {
            Jump();
            //Reset JumpForce after release
            jumpForce = 1200f;
        }
    }
    public void ResetPlayer()
    {

    }

    public void Jump()
    {
        // Get Mouse Position on Screen
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 mouseDirection = Input.mousePosition - screenPosition;
        mouseDirection.Normalize();
        rb.AddForce(mouseDirection * jumpForce);
        Debug.Log(mouseDirection);
        Debug.Log("Jump");

        if(mouseDirection.x < 0){
            sprite.flipX = true;
        }
        else{
            sprite.flipX = false;
        }
    }

    public void flip()
    {
         
    }
}
