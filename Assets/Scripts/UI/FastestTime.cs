using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FastestTime : MonoBehaviour
{
    private string fastestTime;

    void Start()
    {
        
        fastestTime = PlayerPrefs.GetString("FastestTime");
        Debug.Log(fastestTime);
        
    }

    // Update is called once per frame
    void Update()
    {   if (fastestTime==""){

    }
    else{
        GetComponent<TextMeshProUGUI>().text = fastestTime;}
    }
}
