using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : EntityController
{
    [Header("Player Stats")]
    [SerializeField] private int maxAmmoCount;
    private int currentAmmoCount;

    [Header("References")]
    public Revolver revolver;

    private void Awake() {
        currentHealth = maxHealth;
    }

    private void Start() {
        GameManager.instance.player = transform;
        GameManager.instance.playerLayer = LayerMask.GetMask("Player");
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

    public void AddAmmo(int amt) {
        currentAmmoCount = Mathf.Clamp(currentAmmoCount + amt, 0, maxAmmoCount);
    }
}
