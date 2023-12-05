using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerController : MonoBehaviour
{
    private TextMeshProUGUI timeText;

    private float time;
    private bool timerStarts;

    // Start is called before the first frame update
    void Start()
    {
        timeText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            timerStarts = true;
        }

        if (timerStarts)
        {
            time += Time.deltaTime;

            int minutes = Mathf.FloorToInt(time / 60F);
            int seconds = Mathf.FloorToInt(time - minutes * 60);
            int milliseconds = Mathf.FloorToInt((time - (minutes * 60 + seconds)) * 100); // Convert remaining seconds to milliseconds

            timeText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
        }
    }

    public void ResetTimer()
    {
        time = 0;
        timerStarts = false;
    }
}
