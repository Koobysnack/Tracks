using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PlayerController : EntityController
{
    public FMODUnity.EventReference playerDamagedSoundEvent;
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
        FMODUnity.RuntimeManager.PlayOneShot(playerDamagedSoundEvent, transform.position);
        if(currentHealth <= 0)
            Die();
        print("Player Damaged. New Health: " + currentHealth);
        if(UIDamageIndicatorManager.instance != null)
            UIDamageIndicatorManager.instance.PlayerHit(opponent);
    }

    public void Heal(float healAmt) {
        currentHealth = Mathf.Clamp(currentHealth + healAmt, 0, maxHealth);
    }
}
