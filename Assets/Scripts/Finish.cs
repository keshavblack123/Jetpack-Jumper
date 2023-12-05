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
            PlayerPrefs.SetString("Time",timeText.text);
            SceneManager.LoadSceneAsync(2);
        }
        
    }
    
}
