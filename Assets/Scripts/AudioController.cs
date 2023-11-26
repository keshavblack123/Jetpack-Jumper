using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource stage1; // y < 43
    public AudioSource stage2; // y < 73
    public AudioSource stage3; // y < 101
    public AudioSource stage4; // y >= 101

    private int currentStage = 1;
    private int previousStage = 1;

    private Transform player;
    private float playerY;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        playerY = player.position.y;

        if (playerY < 44)
            currentStage = 1;
        else if (playerY < 73)
            currentStage = 2;
        else if (playerY < 101)
            currentStage = 3;
        else
            currentStage = 4;

        if (currentStage != previousStage)
        {
            SwitchAudio();
            previousStage = currentStage;
        }
    }

    void SwitchAudio()
    {
        // Stop all audio sources
        stage1.Stop();
        stage2.Stop();
        stage3.Stop();
        stage4.Stop();

        // Play the audio for the current stage
        switch (currentStage)
        {
            case 1:
                stage1.Play();
                break;
            case 2:
                stage2.Play();
                break;
            case 3:
                stage3.Play();
                break;
            case 4:
                stage4.Play();
                break;
        }
    }
}
