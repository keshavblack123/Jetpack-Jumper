using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class YourTime : MonoBehaviour
{
    private string time;

    // Start is called before the first frame update
    void Start()
    {
        time = PlayerPrefs.GetString("Time");
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<TextMeshProUGUI>().text = time;
    }
}
