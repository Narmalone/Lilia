using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isPc;
    public bool isGamepad;
    public bool isPaused;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void PcSelected()
    {
        isPc = true;
        isGamepad = false;
        NextScene();
    }

    public void GamepadSelected()
    {
        isGamepad = true;
        isPc = false;
        NextScene();
    }

    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void GamePaused()
    {
        Time.timeScale = 0;
    }

    public void GameResume()
    {
        Time.timeScale = 1;
    }

}