using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : EntityController
{
    [Header("Player Stats")]
    [SerializeField] private int maxAmmoCount;
    public int currentAmmoCount { get; private set; }

    [Header("References")]
    [SerializeField] private Transform weaponHolder;
    public Revolver revolver;
    private PlayerMovement movement;
    private PlayerCamera pCam;
    private WeaponBobSway wBob;

    private PlayerInputActions pInput;

    private void Awake() {
        movement = GetComponent<PlayerMovement>();
        pCam = GetComponentInChildren<PlayerCamera>();
        wBob = GetComponent<WeaponBobSway>();
        currentHealth = maxHealth;
        currentAmmoCount = maxAmmoCount / 2;

        pInput = new PlayerInputActions();
        pInput.Attack.Fire.performed += RevolverShoot;
        pInput.Attack.AltFire.performed += ReadySin;
        pInput.Attack.AltFire.canceled += CancelSin;
        pInput.Attack.RotateCylinder.performed += RevolverSelectBullet;
        pInput.Attack.Reload.performed += RevolverReload;
    }

    private void Start() {
        if(GameManager.instance) {
            GameManager.instance.player = transform;
            GameManager.instance.playerLayer = LayerMask.GetMask("Player");
        }
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.L))
            Damage(1000, null);
    }

    private void OnEnable() {
        pInput.Enable();
    }

    private void OnDisable() {
        pInput.Disable();
    }

    private void RevolverShoot(InputAction.CallbackContext context) {
        if(PauseManager.instance && PauseManager.instance.paused)
            return;
        revolver.Shoot();
    }

    private void ReadySin(InputAction.CallbackContext context) {
        if(PauseManager.instance && PauseManager.instance.paused)
            return;
        revolver.ReadySin();
    }
    private void CancelSin(InputAction.CallbackContext context) {   // No pause check since it would be easier to cancel regardless of pause status
        revolver.CancelSin();
    }

    private void RevolverSelectBullet(InputAction.CallbackContext context) {
        if(PauseManager.instance && PauseManager.instance.paused)
            return;
        revolver.SelectBullet(context.ReadValue<float>());
    }

    private void RevolverReload(InputAction.CallbackContext context) {
        if(PauseManager.instance && PauseManager.instance.paused)
            return;
        revolver.StartReload();
    }

    private IEnumerator DeathAnim() {
        // pause player for a moment before starting animation
        weaponHolder.localEulerAngles = new Vector3(0, weaponHolder.localEulerAngles.y, weaponHolder.localEulerAngles.z);
        yield return new WaitForSecondsRealtime(0.25f);
        StartCoroutine("LowerWeapon");

        // if player not crouched, bring them to their knees
        if(!movement.GetCrouched()) {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y / 2, transform.localScale.z);
            GetComponent<Rigidbody>().AddForce(Vector3.down * (0.25f / (Time.timeScale != 0 ? Time.timeScale : 0.1f)), ForceMode.Impulse);
            yield return new WaitForSecondsRealtime(1.25f);
        }

        // lerp z angle until around 90 degrees
        Vector3 targetAngle = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 90);
        while(Mathf.Abs(transform.localEulerAngles.z - 90) > 5) {
            float lerpVal = 0.75f / (Time.timeScale != 0 ? Time.timeScale : 0.1f);
            transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, targetAngle, Time.deltaTime * lerpVal);
            yield return new WaitForEndOfFrame();
        }

        // ready for respawn screen
        StopCoroutine("LowerWeapon");
        yield return new WaitForSecondsRealtime(1);
        GameManager.instance.onPlayerDeath.Invoke();
    }

    private IEnumerator LowerWeapon() {
        // lerp x angle of weapon until around 45 degrees
        Vector3 targetAngle = new Vector3(45, weaponHolder.localEulerAngles.y, weaponHolder.localEulerAngles.z);
        while(Mathf.Abs(weaponHolder.localEulerAngles.z - 45) > 3) {
            float lerpVal = 1.25f / (Time.timeScale != 0 ? Time.timeScale : 0.1f);
            weaponHolder.localEulerAngles = Vector3.Lerp(weaponHolder.localEulerAngles, targetAngle, Time.deltaTime * lerpVal);
            yield return new WaitForEndOfFrame();
        }
    }

    protected override void Die() {
        pInput.Disable();
        GameManager.instance.playerDead = true;
        movement.enabled = false;
        pCam.enabled = false;
        wBob.enabled = false;
        StartCoroutine("DeathAnim");
    }

    public override void Damage(float dmgAmt, Transform opponent) {
        currentHealth -= dmgAmt;
        if(currentHealth <= 0 && !GameManager.instance.playerDead)
            Die();
        if(UIDamageIndicatorManager.instance != null)
            UIDamageIndicatorManager.instance.PlayerHit(opponent);
    }

    public void Heal(float healAmt) {
        currentHealth = Mathf.Clamp(currentHealth + healAmt, 0, maxHealth);
    }

    public float GetCurrentHealth() {
        return currentHealth;
    }

    public float GetHealthPercent() {
        return currentHealth / maxHealth;
    }

    public void ChangeAmmo(int amt) {
        currentAmmoCount = Mathf.Clamp(currentAmmoCount + amt, 0, maxAmmoCount);
    }

    public void Respawn(Checkpoint cp) {
        transform.position = cp.cpCoordinates;
        transform.rotation = Quaternion.Euler(new Vector3(0, cp.cpPlayerRotation, 0));
        transform.GetComponentInChildren<PlayerCamera>().ResetView();
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * 2, transform.localScale.z);
        weaponHolder.transform.localEulerAngles = Vector3.zero;

        currentHealth = cp.playerCurHealth;
        currentAmmoCount = cp.playerCurAmmo;
        pInput.Enable();
        movement.enabled = true;
        pCam.enabled = true;
        wBob.enabled = true;
    }
}
