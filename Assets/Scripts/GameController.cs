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

    [Header("Fields for progress bar")]
    [SerializeField] private Transform startTransform;
    [SerializeField] private Transform finishTransform;
    [SerializeField] private PlayerController player;
    public float StartDistance { get; private set; }
    public float DistanceLeft { get; private set; }
    
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

    private void Start()
    {
        // working with progress bar
        if (player!= null)
            StartDistance = finishTransform.position.z - startTransform.position.z;
    }

    private void Update()
    {
        // working with progress bar
        if (player != null)
        {
            DistanceLeft = Vector3.Distance(player.transform.position, finishTransform.position);
            if (DistanceLeft > StartDistance)
            {
                DistanceLeft = StartDistance;
            }
            if (player.transform.position.z > finishTransform.position.z)
            {
                DistanceLeft = 0;
            }
        }
    }

    public void StartGame()
    {
        GameStarted = true;
        Debug.Log("Game started");
    }

    public void EndGame()
    {
        GameEnded = true;
    }

    /// <summary>
    /// overloading EndGame method
    /// </summary>
    /// <param name="win">true - u win; false - u lose.</param>
    public void EndGame(bool win)
    {
        GameEnded = true;
        Time.timeScale = slowMoEffect;
        Time.fixedDeltaTime *= slowMoEffect;
        if (!win)
        {
            Invoke("Restart", 2 * slowMoEffect);
        }
        else
        {
            Invoke("LoadNextLvl", 2 * slowMoEffect);
        }
    }


    // There are Method for working with Scenes...
    public void Restart()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }

    public void LoadNextLvl()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (scene.buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(scene.buildIndex + 1);
        }
        else
        {
            LoadMainMenu();
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
