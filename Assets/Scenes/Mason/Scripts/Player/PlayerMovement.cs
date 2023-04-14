using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Serialized Fields
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

    [Header("Dashing")]
    [SerializeField] private float dashForce;
    [SerializeField] private float dashCooldown;

    [Header("Leaning")]
    [SerializeField] private float leanDegree;
    [SerializeField] private float interpVal;
    #endregion

    #region Private Variables
    private PlayerInputActions pInput;
    private Rigidbody body;
    private CapsuleCollider capsule;
    private RaycastHit currentGround;
    private Vector3 moveDir;

    private bool onGround;
    private bool onSlope;
    private bool crouched;
    private bool canDash;
    private float currentMoveScalar;
    private float currentSpeed;
    #endregion

    #region Unity Functions
    private void Awake() {
        pInput = new PlayerInputActions();
        body = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
        canDash = true;

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
        if(body.useGravity)
            body.AddForce(Physics.gravity * body.mass * gravityScalar, ForceMode.Force);
    }
    #endregion

    #region General Functions
    private void SetMoveDir() {
        // get player input and set initial move direction
        Vector2 input = pInput.Player.Movement.ReadValue<Vector2>();
        moveDir = (transform.forward * input.y) + (transform.right * input.x);

        // get angle of ground and change moveDir if on slope
        float angle = onGround ? Mathf.Round(Vector3.Angle(Vector3.up, currentGround.normal)) : Mathf.Infinity;
        onSlope = angle > 0 && angle <= maxSlopeAngle;
        if(onSlope)
            moveDir = Vector3.ProjectOnPlane(moveDir, currentGround.normal);
    }

    /*
    *  BUG: PLAYER STILL SLIDES ON SLOPES
    */
    private void Move() {
        body.AddForce(moveDir.normalized * currentSpeed * 10 * currentMoveScalar, ForceMode.Force);
    }

    private void SetSpeed() {
        // use crouch speed if croucehd
        if(crouched)
            currentSpeed = crouchSpeed;
        else {
            // use sprint speed if sprinting, otherwise use walk speed
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

    private void SetLean() {
        float leanVal = pInput.Player.Lean.ReadValue<float>();
        float leanZ = -leanVal * leanDegree;
        float lerpVal = Mathf.LerpAngle(transform.localEulerAngles.z, leanZ, interpVal);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, lerpVal);
    }

    private bool Grounded() {
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
        if(onGround && !crouched) {
            body.AddForce(jumpForce * transform.up, ForceMode.Impulse);
        }
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

    /*
    *  BUG: DASHING MAKES PLAYER BOUNCE WHEN LEANING
    */
    private void Dash(InputAction.CallbackContext context) {
        if(!onGround || crouched || !canDash) return;

        body.AddForce(moveDir.normalized * dashForce, ForceMode.Impulse);
        StartCoroutine("DashCooldown");
    }
    #endregion

    #region Coroutines
    private IEnumerator DashCooldown() {
        canDash = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    #endregion
}
