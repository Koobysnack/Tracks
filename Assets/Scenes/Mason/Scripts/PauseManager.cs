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
}
