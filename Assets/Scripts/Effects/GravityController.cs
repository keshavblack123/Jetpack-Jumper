using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    [SerializeField]
    private float gravity=10;

    private Rigidbody2D playerRb;


    // Start is called before the first frame update
    void Start() { 
        playerRb = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    void Update() { }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag=="Player"){
            playerRb.gravityScale=gravity;
        }
    }
    

  
}
