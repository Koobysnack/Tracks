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
        pInput.Gun.AltFire.performed += RevolverAltShoot;
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

    private void RevolverAltShoot(InputAction.CallbackContext context) {
        if(PauseManager.instance && PauseManager.instance.paused)
            return;
        revolver.AltShoot();
    }

    private void RevolverSelectBullet(InputAction.CallbackContext context) {
        if(PauseManager.instance && PauseManager.instance.paused)
            return;
        revolver.SelectBullet(context.ReadValue<float>());
    }

    private void RevolverReload(InputAction.CallbackContext context) {
        if(PauseManager.instance && PauseManager.instance.paused)
            return;
        revolver.Reload();
    }

    protected override void Die() {
        print("Player is dead");
    }

    public override void Damage(float dmgAmt, Transform opponent) {
        currentHealth -= dmgAmt;
        if(currentHealth <= 0)
            Die();
        print("Player Damaged. New Health: " + currentHealth);
        if(UIDamageIndicatorManager.instance != null)
            UIDamageIndicatorManager.instance.PlayerHit(opponent);
    }

    public void Heal(float healAmt) {
        currentHealth = Mathf.Clamp(currentHealth + healAmt, 0, maxHealth);
    }

    public float GetHealthPercent() {
        return currentHealth / maxHealth;
    }

    public void ChangeAmmo(int amt) {
        currentAmmoCount = Mathf.Clamp(currentAmmoCount + amt, 0, maxAmmoCount);
    }
}
