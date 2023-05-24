using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Serialized Fields
    [Header("Components")]
    [SerializeField] private CapsuleCollider capsule;
    [SerializeField] private Transform leanPoint;

    [Header("Movement")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float minSpeedThreshold;
    [SerializeField] private float drag;

    [Header("Jumping")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float airMovementScalar;
    [SerializeField] private float gravityScalar;
    [SerializeField] private LayerMask groundLayer;

    [Header("Dashing")]
    [SerializeField] private float dashForce;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCooldown;

    [Header("Leaning")]
    [SerializeField] private float leanDegree;
    [SerializeField] private float leanSpeed;
    [SerializeField] private float leanStepScalar;

    [Header("Slopes and Steps")]
    [SerializeField] private float maxSlopeAngle;
    [SerializeField] private float maxStepHeight;
    #endregion

    #region Private Variables
    private PlayerInputActions pInput;
    private Rigidbody body;
    private RaycastHit currentGround;
    private Vector3 moveDir;

    private bool onSlope;
    private bool crouched;
    private bool canDash;
    private bool dashing;
    private bool leanMove;
    private float leanVal;
    private float currentMoveScalar;
    private float currentSpeed;
    #endregion

    public bool onGround { get; private set; }

    #region Unity Functions
    private void Awake() {
        pInput = new PlayerInputActions();
        body = GetComponent<Rigidbody>();
        canDash = true;
        dashing = false;

        pInput.Player.Jump.performed += Jump;
        pInput.Player.Crouch.performed += ToggleCrouch;
        pInput.Player.Dash.performed += Dash;
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
        SetLean();
        body.useGravity = !onSlope;
    }

    private void FixedUpdate() {
        Move();
        SpeedControl();
        ApplyGravity();
    }
    #endregion

    #region General Functions
    private void SetMoveDir() {
        // get player input and set initial move direction
        Vector2 input = pInput.Player.Movement.ReadValue<Vector2>();
        moveDir = (transform.forward * input.y) + (transform.right * input.x);
        moveDir.y = 0;  // ensure no upward movement in normal direction

        // get angle of ground and change moveDir if on slope
        float angle = onGround ? Mathf.Round(Vector3.Angle(Vector3.up, currentGround.normal)) : Mathf.Infinity;
        onSlope = angle > 0 && angle <= maxSlopeAngle;
        if(onSlope)
            moveDir = Vector3.ProjectOnPlane(moveDir, currentGround.normal);
    }

    private void Move() {
        body.AddForce(moveDir.normalized * currentSpeed * 10 * currentMoveScalar, ForceMode.Force);
    }

    private void SetSpeed() {
        // use crouch speed if croucehd
        if(crouched)
            currentSpeed = crouchSpeed;
        else {
            // use sprint speed if sprinting, otherwise use walk speed
            bool movingForward = transform.InverseTransformDirection(moveDir).z > 0;
            bool sprintPossible = pInput.Player.Sprint.ReadValue<float>() > 0.1f && onGround && movingForward;
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

    private void SetLean() {
        // get lean direction
        float prevLeanVal = leanVal;
        leanVal = pInput.Player.Lean.ReadValue<float>();
        float targetZ = -leanVal * leanDegree;

        if(leanVal == 0) {
            // if previously leaning, move step back from lean
            if(leanMove) {
                Vector3 leanMoveDir = transform.right * -prevLeanVal * leanStepScalar * walkSpeed;
                body.AddForce(leanMoveDir, ForceMode.Force);
            }
            leanMove = false;
        }
        else if(!leanMove) {
            // take step out in direction of lean
            Vector3 leanMoveDir = transform.right * leanVal * leanStepScalar * walkSpeed;
            body.AddForce(leanMoveDir, ForceMode.Force);
            leanMove = true;
        }

        // lerp player to lean in that direction smoothely
        float lerpVal = Mathf.LerpAngle(leanPoint.localEulerAngles.z, targetZ, leanSpeed * Time.deltaTime);
        leanPoint.localEulerAngles = new Vector3(leanPoint.localEulerAngles.x, leanPoint.localEulerAngles.y, lerpVal);
    }

    private void ApplyGravity() {
        // applies extra gravity to player
        if(body.useGravity)
            body.AddForce(Physics.gravity * body.mass * gravityScalar, ForceMode.Force);
    }

    private void SpeedControl() {
        if(onSlope && !dashing) {
            float speed = body.velocity.magnitude;
            if(speed > currentSpeed)
                body.velocity = body.velocity.normalized * currentSpeed;
            else if(speed < minSpeedThreshold)
                body.velocity = Vector3.zero;
        }
    }

    private bool Grounded() {
        // boxcast from player downwards to see if on ground
        Vector3 center = capsule.bounds.center;
        Vector3 halfExtents = capsule.bounds.extents * 0.5f;
        halfExtents.y = 0.1f;
        float maxDistance = capsule.bounds.extents.y;

        bool hit = Physics.BoxCast(center, halfExtents, Vector3.down, out currentGround, transform.rotation, maxDistance, groundLayer);
        return hit;
    }
    #endregion

    #region CallbackEvents
    private void Jump(InputAction.CallbackContext context) {
        if(onGround && !crouched)
            body.AddForce(jumpForce * transform.up, ForceMode.Impulse);
    }

    private void ToggleCrouch(InputAction.CallbackContext context) {
        if(!onGround) return;

        crouched = !crouched;
        if(crouched) {
            // half scale and push player to ground
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y / 2, transform.localScale.z);
            body.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        else
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * 2, transform.localScale.z);
    }

    private void Dash(InputAction.CallbackContext context) {
        if(!onGround || crouched || !canDash) return;

        dashing = true;
        body.AddForce(moveDir.normalized * dashForce, ForceMode.Impulse);
        StartCoroutine("DashCooldown");
    }
    #endregion

    #region Coroutines
    private IEnumerator DashCooldown() {
        canDash = false;
        yield return new WaitForSeconds(dashTime);
        dashing = false;
        yield return new WaitForSeconds(dashCooldown - dashTime);
        canDash = true;
    }
    #endregion
}
