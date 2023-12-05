using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Finish : MonoBehaviour
{
    public TextMeshProUGUI timeText;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            int currentTime = ConvertToSeconds(timeText.text);
            int bestTime = ConvertToSeconds(PlayerPrefs.GetString("FastestTime"));
            if (currentTime < bestTime)
            {
                PlayerPrefs.SetString("FastestTime", timeText.text);
            }
            PlayerPrefs.SetString("Time", timeText.text);
            SceneManager.LoadSceneAsync(2);
        }
    }

    private int ConvertToSeconds(string timeString)
    {
        // Parse hours, minutes, and seconds from the time string
        string[] timeComponents = timeString.Split(':');

        int hours = int.Parse(timeComponents[0]);
        int minutes = int.Parse(timeComponents[1]);
        int seconds = int.Parse(timeComponents[2]);

        // Calculate total seconds
        int totalSeconds = hours * 3600 + minutes * 60 + seconds;

        return totalSeconds;
    }
}
