using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController singleton;
    public bool GameStarted { get; private set; }
    public bool GameEnded { get; private set; }

    private float slowMoEffect = .1f;

    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        else if (singleton != this)
        {
            Destroy(gameObject);
        }

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }    

    public void StartGame()
    {
        GameStarted = true;
        Debug.Log("Game started");
    }

    public void EndGame()
    {
        GameEnded = true;
        Debug.Log("Game ended");
    }

    public void EndGame(bool win)
    {
        GameEnded = true;
        Time.timeScale = slowMoEffect;
        Time.fixedDeltaTime *= slowMoEffect;
        if (!win)
        {
            Debug.Log("U have lost. Restart...");
            Invoke("Restart", 2 * slowMoEffect);
        }
        else
        {
            // TODO: Next level menu
        }
    }

    public void Restart()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }

    public void LoadNextLvl()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex + 1);
    }
}
