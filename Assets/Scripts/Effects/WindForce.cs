using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindForce : MonoBehaviour
{
    [Tooltip("1=right ; -1=left")]
    public float direction = -1;
    public float windForce = 10;
    public float windDuration = 5;
    public float warningDuration = 3;
    public float interval = 10;
    public GameObject arrowGameObject;

    private Rigidbody2D playerRb;
    private bool windEnabled;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
        StartCoroutine(ToggleColliderWithDelay());
        this.GetComponent<BoxCollider2D>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (windEnabled)
        {
            playerRb.AddForce(direction * windForce * Vector2.right, ForceMode2D.Impulse);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            windEnabled = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            windEnabled = false;
        }
    }

    private IEnumerator ToggleColliderWithDelay()
    {
        while (true)
        {
            // Arrow Warning
            arrowGameObject.SetActive(true);
            arrowGameObject.GetComponent<Animator>().SetBool("Blink", true);
            yield return new WaitForSeconds(warningDuration);

            // WindForce in Effect
            arrowGameObject.GetComponent<Animator>().SetBool("Blink", false);
            arrowGameObject.GetComponent<Animator>().SetBool("Red", true);
            this.GetComponent<BoxCollider2D>().enabled = true;
            EnableWind(this.transform, "Wind");
            yield return new WaitForSeconds(windDuration);

            // WindForce finish
            windEnabled = false;
            arrowGameObject.GetComponent<Animator>().SetBool("Red", false);
            this.GetComponent<BoxCollider2D>().enabled = false;
            DisableWind(this.transform, "Wind");
            arrowGameObject.SetActive(false);

            yield return new WaitForSeconds(interval);
        }
    }

    void EnableWind(Transform parent, string tag)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag(tag))
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    void DisableWind(Transform parent, string tag)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag(tag))
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}
