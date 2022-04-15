using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Gamemanager : Singleton<Gamemanager>
{
    [SerializeField] private bool _isDevelopmentMode = true;

    public static event Action OnGameStarted;
    public static event Action OnGameFinished;

    public override void Awake()
    {
        base.Awake();
    }

    void StartGame()
    {
        OnGameStarted?.Invoke();
    }

    private void Update()
    {
        if (_isDevelopmentMode)
        {
            if (Input.GetMouseButtonDown(1))
            {
                StartGame();
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
