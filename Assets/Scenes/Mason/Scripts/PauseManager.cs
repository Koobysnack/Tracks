using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;
    
    private PlayerInputActions pInput;
    public PauseController pauseController;
    public bool paused { get; private set; }

    private void Awake() {
        if(instance == null)
            instance = this;
        else
            Destroy(this);

        paused = false;

        pInput = new PlayerInputActions();
        pInput.Menu.TogglePause.performed += TogglePause;
    }

    private void OnEnable() {
        pInput.Enable();
    }

    private void OnDisable() {
        pInput.Disable();
    }

    private void TogglePause(InputAction.CallbackContext context) {
        paused = !paused;
        if(pauseController)
            pauseController.TogglePause(paused);
        Time.timeScale = paused ? 0 : 1;
    }

    public void TogglePauseSinMenu()
    {
        paused = !paused;
        // Bandaid fix to prevent using togglePause during a UI interface open. Ideally pressing Esc with it open should close the interface
        if (paused)
        {
            pInput.Menu.TogglePause.Disable();
            pInput.Player.Interact.Disable();
        }
        else
        {
            pInput.Menu.TogglePause.Enable();
            pInput.Player.Interact.Enable();
            UIChamberMenuManager.instance.CloseMenu();

        }
        
        Time.timeScale = paused ? 0 : 1;
    }
}
