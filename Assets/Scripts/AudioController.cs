using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource stage1; // y < 43
    public AudioSource stage2; // y < 73
    public AudioSource stage3; // y < 101
    public AudioSource stage4; // y >= 101
    private AudioSource currentAudio;
    private AudioSource previousAudio;

    private int currentStage = 1;
    private int previousStage = 1;

    private Transform player;
    private float playerY;

    public float fadeTime;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentAudio = stage1;
        previousAudio = stage1;
    }

    // Update is called once per frame
    void Update()
    {
        playerY = player.position.y;

        if (playerY < 44)
        {
            currentStage = 1;
            currentAudio = stage1;
        }
        else if (playerY < 73)
        {
            currentStage = 2;
            currentAudio = stage2;
        }
        else if (playerY < 101)
        {
            currentStage = 3;
            currentAudio = stage3;
        }
        else
        {
            currentStage = 4;
            currentAudio = stage4;
        }

        if (currentStage != previousStage)
        {
            SwitchAudio();
            previousStage = currentStage;
        }
    }

    void SwitchAudio()
    {
        // // Stop all audio sources
        // stage1.Stop();
        // stage2.Stop();
        // stage3.Stop();
        // stage4.Stop();

        // // Play the audio for the current stage
        // switch (currentStage)
        // {
        //     case 1:
        //         stage1.Play();
        //         break;
        //     case 2:
        //         stage2.Play();
        //         break;
        //     case 3:
        //         stage3.Play();
        //         break;
        //     case 4:
        //         stage4.Play();
        //         break;
        // }

        //fade out
        StartCoroutine(StartFade(previousAudio, fadeTime, 0));

        //fade in
        StartCoroutine(StartFade(currentAudio, fadeTime, 1));

        previousAudio = currentAudio;

    }

    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        Debug.Log("Audio Fade", audioSource);
        // audioSource.Play();
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }
}
