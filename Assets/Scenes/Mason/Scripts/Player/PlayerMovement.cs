using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float drag;
    [SerializeField] private float maxSlopeAngle;

    [Header("Jumping")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float airMovementScalar;
    [SerializeField] private float gravityScalar;
    [SerializeField] private LayerMask groundLayer;

    private PlayerInputActions pInput;
    private Rigidbody body;
    private CapsuleCollider capsule;
    private RaycastHit currentGround;
    private Vector3 moveDir;
    private Vector3 currentMoveVec;

    private bool onGround;
    private bool crouched;
    private float currentMoveScalar;
    private float currentSpeed;

    private void Awake() {
        pInput = new PlayerInputActions();
        body = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();

        pInput.Player.Jump.performed += Jump;
        pInput.Player.Crouch.performed += ToggleCrouch;
    }

    private void OnEnable() {
        pInput.Enable();
    }

    private void OnDisable() {
        pInput.Disable();
    }

    private void Update() {
        onGround = Grounded();

        SetMoveDir();
        SetSpeed();
        SetDrag();
    }

    private void FixedUpdate() {
        Move();
        if(body.useGravity)
            body.AddForce(Physics.gravity * body.mass * gravityScalar, ForceMode.Force);
    }

    private void SetMoveDir() {
        Vector2 input = pInput.Player.Movement.ReadValue<Vector2>();
        moveDir = (transform.forward * input.y) + (transform.right * input.x);
    }

    private void Move() {
        float angle = Mathf.Infinity;
        if(onGround)
            angle = Vector3.Angle(Vector3.up, currentGround.normal);
        
        if(angle <= maxSlopeAngle) {
            Vector3 groundVec = Vector3.ProjectOnPlane(moveDir, currentGround.normal).normalized;
            body.AddForce(groundVec * currentSpeed * 10 * currentMoveScalar, ForceMode.Force);
        }
        else
            body.AddForce(moveDir * currentSpeed * 10 * currentMoveScalar, ForceMode.Force);

        body.useGravity = !onGround || angle == 0; 
    }

    private void SetSpeed() {
        if(crouched)
            currentSpeed = crouchSpeed;
        else {
            bool sprintPossible = pInput.Player.Sprint.ReadValue<float>() > 0.1f && onGround;
            currentSpeed = sprintPossible ? sprintSpeed : walkSpeed;
        }
    }

    private void SetDrag() {
        if(onGround) {
            body.drag = drag;
            currentMoveScalar = 1;
        }
        else {
            body.drag = 0;
            currentMoveScalar = airMovementScalar;
        }
    }

    private bool Grounded() {
        Vector3 center = capsule.bounds.center;
        Vector3 halfExtents = capsule.bounds.extents * 0.5f;
        halfExtents.y = 0.1f;
        float maxDistance = capsule.bounds.extents.y;

        bool hit = Physics.BoxCast(center, halfExtents, Vector3.down, out currentGround, transform.rotation, maxDistance, groundLayer);
        return hit;
    }

    private void Jump(InputAction.CallbackContext context) {
        if(onGround && !crouched) {
            body.AddForce(jumpForce * transform.up, ForceMode.Impulse);
        }
    }

    private void ToggleCrouch(InputAction.CallbackContext context) {
        if(!onGround) return;

        crouched = !crouched;
        if(crouched) {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y / 2, transform.localScale.z);
            body.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        else
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * 2, transform.localScale.z);
    }
}
