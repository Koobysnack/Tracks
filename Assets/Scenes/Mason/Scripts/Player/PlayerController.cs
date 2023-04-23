using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : EntityController
{
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
    }

    public void Heal(float healAmt) {
        currentHealth = Mathf.Clamp(currentHealth + healAmt, 0, maxHealth);
    }
}
