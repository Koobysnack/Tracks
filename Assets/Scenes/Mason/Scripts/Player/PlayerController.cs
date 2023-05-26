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
    public Revolver revolver;

    private PlayerInputActions pInput;

    private void Awake() {
        currentHealth = maxHealth;
        currentAmmoCount = maxAmmoCount / 2;

        pInput = new PlayerInputActions();
        pInput.Gun.Fire.performed += RevolverShoot;
        pInput.Gun.AltFire.performed += ReadySin;
        pInput.Gun.AltFire.canceled += CancelSin;
        pInput.Gun.RotateCylinder.performed += RevolverSelectBullet;
        pInput.Gun.Reload.performed += RevolverReload;
    }

    private void Start() {
        if(GameManager.instance) {
            GameManager.instance.player = transform;
            GameManager.instance.playerLayer = LayerMask.GetMask("Player");
        }
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

    protected override void Die() {
        GameManager.instance.onPlayerDeath.Invoke();
    }

    public override void Damage(float dmgAmt, Transform opponent) {
        currentHealth -= dmgAmt;
        if(currentHealth <= 0)
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

        currentHealth = cp.playerCurHealth;
        currentAmmoCount = cp.playerCurAmmo;
    }
}
