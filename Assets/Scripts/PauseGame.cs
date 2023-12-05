using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    private bool isPaused = false;
    public GameObject PausedOverlay;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                Time.timeScale = 0f;
                PausedOverlay.SetActive(true);
            }
            else
            {
                Time.timeScale = 1f;
                PausedOverlay.SetActive(false);
            }

            isPaused = !isPaused;
        }

        if (isPaused)
        {
            GetComponent<PlayerController>().enabled = false;
            GetComponentInChildren<IndicatorController>().enabled=false;
        }
        else
        {
            GetComponent<PlayerController>().enabled = true;
            GetComponentInChildren<IndicatorController>().enabled=true;
        }
    }
}
