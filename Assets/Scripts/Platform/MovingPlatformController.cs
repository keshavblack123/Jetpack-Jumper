using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    public GameObject[] waypoints;
    private int currentWaypointIndex = 0;
    public float speed = 2f;
    Vector3 parentStartingPosition;
    void Start()
    {
        parentStartingPosition = transform.parent.position;
    }

    // Update is called once per frame
    private void Update()
    {
        if (
            Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position)
            < .1f
        )
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }
        transform.position = Vector2.MoveTowards(
            transform.position,
            waypoints[currentWaypointIndex].transform.position,
            Time.deltaTime * speed
        );
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            collision.gameObject.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            collision.gameObject.transform.SetParent(null);
        }
    }

    public void ResetPlatform()
    {
        transform.localPosition = new Vector3(0, 0, 0);
        transform.parent.position = parentStartingPosition;
    }
}
