using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float cameraSens;
    private PlayerInputActions pInput;
    private Transform player;
    private float horiz;
    private float vert;

    private void Awake() {
        pInput = new PlayerInputActions();
        player = transform.parent;
        horiz = player.rotation.eulerAngles.y;
        vert = 0;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable() {
        pInput.Enable();
    }

    private void OnDisable() {
        pInput.Disable();
    }

    private void Update() {
        // mouse input
        Vector2 mouseIn = pInput.Player.Looking.ReadValue<Vector2>();

        // adjust horiz and vert values
        horiz += mouseIn.x * cameraSens * Time.deltaTime;
        vert -= mouseIn.y * cameraSens * Time.deltaTime;
        vert = Mathf.Clamp(vert, -90, 90);

        // rotate camera and player
        transform.localEulerAngles = new Vector3(vert, 0, 0);
        player.localEulerAngles = new Vector3(0, horiz, player.localEulerAngles.z);
    }
}
