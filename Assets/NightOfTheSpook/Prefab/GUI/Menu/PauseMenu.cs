using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject QuitButton;
    public GameObject Content;

    public void Resume()
    {
        TogglePause();
    }

    public void ReturnToMenu()
    {
        var gameManager = FindObjectOfType<GameManager>();
        if (gameManager)
        {
            gameManager.ShowMainWindow();
        }
        else
        {
            TogglePause();
        }
    }

    public void Quit()
    {
#if !UNITY_WEBGL
    Application.Quit();
#endif
    }

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_WEBGL
        Destroy(QuitButton);
#endif
    }

    float _cachedTimescale;
    void TogglePause()
    {
        if (Content.activeInHierarchy)
        {
            Time.timeScale = _cachedTimescale;
            Content.SetActive(false);
        }
        else
        {
            _cachedTimescale = Time.timeScale;
            Time.timeScale = 0.0f;
            Content.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
}
