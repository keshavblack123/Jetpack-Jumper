using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;

public class Finish : MonoBehaviour
{
    public TextMeshProUGUI timeText;

    public float targetVignetteIntensity = 1f;
    public float delayBeforeLoading = 0.5f;
    public float vignetteChangeDuration = 0.5f;

    public Volume postProcessVolume;
    private Vignette vignette;

    public GameObject TimerObject;
    public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        postProcessVolume.profile.TryGet(out vignette);
    }

    // Update is called once per frame
    void Update() { }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            TimerObject.GetComponent<TimerController>().timerStops = true;

            GetComponent<PlayerController>().enabled = false;
            GetComponentInChildren<IndicatorController>().enabled=false;

            if (PlayerPrefs.GetString("FastestTime") != "")
            {
                float currentTime = ConvertToSeconds(timeText.text);
                float bestTime = ConvertToSeconds(PlayerPrefs.GetString("FastestTime"));
                if (currentTime < bestTime)
                {
                    PlayerPrefs.SetString("FastestTime", timeText.text);
                }
            }
            else{
                PlayerPrefs.SetString("FastestTime", timeText.text);
            }
            PlayerPrefs.SetString("Time", timeText.text);
            StartCoroutine(LoadSceneWithGradualVignette(2));
            //SceneManager.LoadSceneAsync(2);
        }
    }

    private float ConvertToSeconds(string timeString)
    {
        string[] timeComponents = timeString.Split(':');

        int minutes = int.Parse(timeComponents[0]);
        int seconds = int.Parse(timeComponents[1]);
        int milliseconds = int.Parse(timeComponents[2]);

        float totalSeconds = minutes * 60 + seconds + milliseconds / 100;

        return totalSeconds;
    }

    IEnumerator LoadSceneWithGradualVignette(int i)
    {
        yield return StartCoroutine(
            ChangeVignetteIntensityOverTime(targetVignetteIntensity, vignetteChangeDuration)
        );

        yield return new WaitForSeconds(delayBeforeLoading);

        SceneManager.LoadSceneAsync(i);
    }

    IEnumerator ChangeVignetteIntensityOverTime(float targetIntensity, float duration)
    {
        float startTime = Time.time;
        float initialIntensity = vignette.intensity.value;

        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            vignette.intensity.value = Mathf.Lerp(initialIntensity, targetIntensity, t);
            yield return null;
        }

        vignette.intensity.value = targetIntensity;
    }
}
