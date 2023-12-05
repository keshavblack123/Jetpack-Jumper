using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorController : MonoBehaviour
{
    private Camera mainCamera;
    private Vector3 mousePosition;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mousePosition - transform.position;
        float rotationZ = Mathf.Atan2(-rotation.y, -rotation.x) * Mathf.Rad2Deg;

        // If Mouse is below the player (arrow is above the player)
        if (rotationZ > 0.05)
        {
            transform.rotation = Quaternion.Euler(0, 0, rotationZ);
            if (rotationZ >= 90)
            {
                transform.localScale = new Vector3(
                    transform.localScale.x,
                    -1,
                    transform.localScale.z
                );
            }
            else
            {
                transform.localScale = new Vector3(
                    transform.localScale.x,
                    1,
                    transform.localScale.z
                );
            }
        }
    }
}