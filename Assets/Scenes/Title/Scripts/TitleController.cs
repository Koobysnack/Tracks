using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    bool started;
    List<AsyncOperation> loadingScenes = new List<AsyncOperation>();

    public void StartGame()
    {
        // AudioManager.instance.PlaySound(uiclickSound, gameObject);
        if (started) return;
        started = true;
        //loadScreen.SetActive(true);
        StartCoroutine(LoadSequence());
    }
    IEnumerator LoadSequence()
    {
        //FadeCoordinator animStatus = loadScreen.GetComponent<FadeCoordinator>();
        //yield return new WaitUntil(() => animStatus.animComplete);
        //loadIcon.SetActive(true);
        //loadText.SetActive(true);
        LoadGame();
        yield return new WaitUntil(() => finishedLoading()); // Wait until all scenes are loaded
        SceneManager.UnloadSceneAsync(1);
    }

    void LoadGame()
    {
        loadingScenes.Add(SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive));
        loadingScenes.Add(SceneManager.LoadSceneAsync(3, LoadSceneMode.Additive));
        loadingScenes.Add(SceneManager.LoadSceneAsync(4, LoadSceneMode.Additive));
        loadingScenes.Add(SceneManager.LoadSceneAsync(5, LoadSceneMode.Additive));
        loadingScenes.Add(SceneManager.LoadSceneAsync(6, LoadSceneMode.Additive));
    }

    bool finishedLoading()
    {
        for (int i = 0; i < loadingScenes.Count; i++)
        {
            if (!loadingScenes[i].isDone) return false;
        }
        return true;
    }

}
