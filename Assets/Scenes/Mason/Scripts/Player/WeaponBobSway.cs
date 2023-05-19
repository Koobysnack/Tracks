using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBobSway : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float posSmooth;
    [SerializeField] private float rotSmooth;

    [Header("Sway Position")]
    [SerializeField] private float posStep;
    [SerializeField] private float maxPosStepDist;
    private Vector3 swayPos;

    [Header("Sway Rotation")]
    [SerializeField] private float rotStep;
    [SerializeField] private float maxRotStepDist;
    private Vector3 swayRot;

    private PlayerMovement movement;
    private Rigidbody body;

    private PlayerInputActions pInput;
    private Vector2 moveInput;
    private Vector2 mouseInput;

    private void Awake() {
        movement = player.GetComponent<PlayerMovement>();
        body = player.GetComponent<Rigidbody>();
        pInput = new PlayerInputActions();
    }

    private void OnEnable() {
        pInput.Enable();
    }

    private void OnDisable() {
        pInput.Disable();
    }

    private void Update() {
        GetInput();

        SetSwayPosition();
        SetSwayRotation();
        Sway();
    }

    private void GetInput() {
        moveInput = pInput.Player.Movement.ReadValue<Vector2>();
        moveInput = moveInput.normalized;

        mouseInput = pInput.Player.Looking.ReadValue<Vector2>();
    }
    
    private void SetSwayPosition() {
        Vector3 invertMouse = mouseInput * -posStep;
        invertMouse.x = Mathf.Clamp(invertMouse.x, -maxPosStepDist, maxPosStepDist);
        invertMouse.y = Mathf.Clamp(invertMouse.y, -maxPosStepDist, maxPosStepDist);
        swayPos = invertMouse;
    }

    private void SetSwayRotation() {
        Vector3 invertMouse = mouseInput * -rotStep;
        invertMouse.x = Mathf.Clamp(invertMouse.x, -maxRotStepDist, maxRotStepDist);
        invertMouse.y = Mathf.Clamp(invertMouse.y, -maxRotStepDist, maxRotStepDist);
        swayRot = new Vector3(invertMouse.y, invertMouse.x, invertMouse.x);
    }

    private void Sway() {
        //transform.localPosition = Vector3.Lerp(transform.localPosition, swayPos, Time.deltaTime * posSmooth);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(swayRot), Time.deltaTime * rotSmooth);
    }

}
