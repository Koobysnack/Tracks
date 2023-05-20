using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    bool paused = false;
    [SerializeField] GameObject hud;
    [SerializeField] GameObject pauseScreen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
            
    }

    void TogglePause()
    {
        paused = !paused;
        if (paused)
        {
            Time.timeScale = 0f;
            hud.SetActive(false);
            pauseScreen.SetActive(true);
        }
        else
        {
            Time.timeScale = 1.0f;
            hud.SetActive(true);
            pauseScreen.SetActive(false);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
