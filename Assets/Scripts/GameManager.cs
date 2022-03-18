using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isPc;
    public bool isGamepad;

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
       Debug.Log("vers la prochaine scene");
    }
}
