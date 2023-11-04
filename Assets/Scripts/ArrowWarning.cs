using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowWarning : MonoBehaviour
{
    public float XOffset;
    private float initialY;

    // Start is called before the first frame update
    void Start()
    {
        initialY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        Camera mainCamera = Camera.main; // Get a reference to the main camera
        if (mainCamera != null)
        {
            // Define the relative X and Y positions
            float relativeX = mainCamera.transform.position.x + XOffset; // Adjust the X position as needed
            float relativeY = initialY; // Set the Y position in the scene coordinates

            // Create a new Vector3 with the relative positions
            Vector3 newPosition = new Vector3(relativeX, relativeY, 0); // Assuming a Z position of 0

            // Set the game object's position to the new position
            transform.position = newPosition;
        }
    }
}
