using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    [SerializeField] GameObject hud;
    [SerializeField] GameObject pauseScreen;
    [SerializeField] FMODUnity.StudioEventEmitter pauseMusic;

    // Start is called before the first frame update
    void Start()
    {
        if(PauseManager.instance)
            PauseManager.instance.pauseController = this;
    }

    public void TogglePause(bool paused)
    {
        hud.SetActive(!paused);
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
        MusicManager.instance.SetPaused(paused);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
