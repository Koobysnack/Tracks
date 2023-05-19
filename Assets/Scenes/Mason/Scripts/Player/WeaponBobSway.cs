using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBobSway : MonoBehaviour
{
    [SerializeField] private Transform weaponHolder;
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

    [Header("Bob Position")]
    private float speedCurve;
    private float curveSin { get => Mathf.Sin(speedCurve); }
    private float curveCos { get => Mathf.Cos(speedCurve); }
    private Vector3 travelLimit;
    private Vector3 bobLimit;
    private Vector3 bobPos;

    [Header("Bob Rotation")]
    [SerializeField] private Vector3 multiplier;
    private Vector3 bobRot;

    private PlayerMovement movement;
    private Rigidbody body;

    private PlayerInputActions pInput;
    private Vector2 moveInput;
    private Vector2 mouseInput;

    private void Awake() {
        movement = GetComponent<PlayerMovement>();
        body = GetComponent<Rigidbody>();
        pInput = new PlayerInputActions();

        travelLimit = Vector3.one * 0.025f;
        bobLimit = Vector3.one * 0.01f;
    }

    private void OnEnable() {
        pInput.Enable();
    }

    private void OnDisable() {
        pInput.Disable();
    }

    private void Update() {
        GetInput();

        //SetSwayPosition();
        SetSwayRotation();

        SetBobPosition();
        SetBobRotation();

        ApplySwayBob();
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

    private void SetBobPosition() {
        speedCurve += Time.deltaTime * (movement.onGround ? body.velocity.magnitude : 1);

        bobPos.x = (curveCos * bobLimit.x + (movement.onGround ? 1 : 0)) - (moveInput.x * travelLimit.x);
        bobPos.y = (curveSin * bobLimit.y) - (body.velocity.y * travelLimit.y);
        bobPos.z = -(moveInput.y * travelLimit.z);
    }

    private void SetBobRotation() {
        bobRot.x = moveInput != Vector2.zero ? multiplier.x * Mathf.Sin(2 * speedCurve) : Mathf.Sin(2 * speedCurve) / 2;
        bobRot.y = moveInput != Vector2.zero ? multiplier.y * curveCos : 0;
        bobRot.z = moveInput != Vector2.zero ? multiplier.z * curveCos * moveInput.x : 0;
    }

    private void ApplySwayBob() {
        //Vector3 newPos = swayPos + bobPos;
        Quaternion newRot = Quaternion.Euler(swayRot) * Quaternion.Euler(bobRot);

        //transform.localPosition = Vector3.Lerp(transform.localPosition, newPos, Time.deltaTime * posSmooth);
        weaponHolder.transform.localRotation = Quaternion.Slerp(weaponHolder.transform.localRotation, newRot, Time.deltaTime * rotSmooth);
    }
}
