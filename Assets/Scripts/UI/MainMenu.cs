using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioSource mainMenuAudio;

    public void PlayGame()
    {
        StartCoroutine(AudioController.StartFade(mainMenuAudio, 1, 0));
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
