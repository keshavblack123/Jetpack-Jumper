using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioSource mainMenuAudio;

    void Update()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    public void PlayGame()
    {
        StartCoroutine(AudioController.StartFade(mainMenuAudio, 1, 0));
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReturntoMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
