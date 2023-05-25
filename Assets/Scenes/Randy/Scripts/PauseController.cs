using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    [SerializeField] GameObject hud;
    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject optionsScreen;
    [SerializeField] FMODUnity.StudioEventEmitter pauseMusic;
    CanvasGroup pScreenCG;
    CanvasGroup oScreenCG;

    void Awake()
    {
        pScreenCG = pauseScreen.GetComponent<CanvasGroup>();
        oScreenCG = optionsScreen.GetComponent<CanvasGroup>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if(PauseManager.instance)
            PauseManager.instance.pauseController = this;
    }

    public void TogglePause(bool paused)
    {
        hud.SetActive(!paused);
        CloseOptions(); // Will need a better fix time permitting
        pauseScreen.SetActive(paused);
        Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;

        if (paused)
        {
            pauseMusic.Play();
        }
        else
        {
            pauseMusic.Stop();
        }
        if(MusicManager.instance)
            MusicManager.instance.SetPaused(paused);
    }

    public void OpenOptions()
    {
        pScreenCG.alpha = 0f;
        oScreenCG.alpha = 1f;
        optionsScreen.SetActive(true);
    }

    public void CloseOptions()
    {
        pScreenCG.alpha = 1f;
        oScreenCG.alpha = 0f;
        optionsScreen.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
