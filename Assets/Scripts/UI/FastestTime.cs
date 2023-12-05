using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FastestTime : MonoBehaviour
{
    private string fastestTime;

    void Start()
    {
        if (PlayerPrefs.HasKey("FastestTime"))
        {
            fastestTime = PlayerPrefs.GetString("FastestTime");
        }
        else
        {
            string defaultFastestTime = "00:00:00";
            PlayerPrefs.SetString("FastestTime", defaultFastestTime);
            PlayerPrefs.Save();
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<TextMeshProUGUI>().text = fastestTime;
    }
}
