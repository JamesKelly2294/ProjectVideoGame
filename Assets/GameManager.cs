using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SceneIndices
{
    TitleScreen = 1,
    MenuTest = 2,
    ComputeTest = 3,
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject loadingScreen;
    public Slider progressBar;

    public string loadGameSceneName;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        SceneManager.LoadSceneAsync((int)SceneIndices.TitleScreen, LoadSceneMode.Additive);
    }

    List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

    public void LoadGame()
    {
        loadingScreen.SetActive(true);
        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndices.TitleScreen));

        if (loadGameSceneName == null)
        {
            scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndices.ComputeTest, LoadSceneMode.Additive));
        } else
        {
            scenesLoading.Add(SceneManager.LoadSceneAsync(loadGameSceneName, LoadSceneMode.Additive));
        }

        StartCoroutine(GetSceneLoadProgress());
    }

    public void ShowLoseScreen()
    {
        loadingScreen.SetActive(true);
        scenesLoading.Add(SceneManager.UnloadSceneAsync(loadGameSceneName));
        
        scenesLoading.Add(SceneManager.LoadSceneAsync("LoseScene", LoadSceneMode.Additive));
 
        StartCoroutine(GetSceneLoadProgress());
    }

    public void ShowWinScreen()
    {
        loadingScreen.SetActive(true);
        scenesLoading.Add(SceneManager.UnloadSceneAsync(loadGameSceneName));

        scenesLoading.Add(SceneManager.LoadSceneAsync("RealWinScene", LoadSceneMode.Additive));

        StartCoroutine(GetSceneLoadProgress());
    }

    public void ShowMainWindow()
    {
        loadingScreen.SetActive(true);

        // Absolute fucking hack
        if (SceneManager.GetSceneByName(loadGameSceneName).isLoaded)
        {
            scenesLoading.Add(SceneManager.UnloadSceneAsync(loadGameSceneName));
        }
        if (SceneManager.GetSceneByName("LoseScene").isLoaded)
        {
            scenesLoading.Add(SceneManager.UnloadSceneAsync("LoseScene"));
        }

        scenesLoading.Add(SceneManager.LoadSceneAsync("TitleScene", LoadSceneMode.Additive));

        StartCoroutine(GetSceneLoadProgress());
    }

    public IEnumerator GetSceneLoadProgress()
    {
        foreach(var sceneLoad in scenesLoading)
        {
            while (!sceneLoad.isDone)
            {
                float totalProgress = 0;
                foreach(var operation in scenesLoading)
                {
                    totalProgress += operation.progress;
                }
                progressBar.value = (totalProgress / scenesLoading.Count);
                yield return null;
            }
        }

        loadingScreen.gameObject.SetActive(false);
        scenesLoading.Clear();
    }
}
