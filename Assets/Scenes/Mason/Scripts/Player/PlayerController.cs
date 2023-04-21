using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    private float currentHealth;

    private void Awake() {
        currentHealth = maxHealth;
    }

    private void Start() {
        GameManager.instance.player = transform;
        GameManager.instance.playerLayer = LayerMask.GetMask("Player");
    }

    private void Die() {
        print("Player is dead");
    }

    public void Damage(float dmgAmt, Transform enemy) {
        currentHealth -= dmgAmt;
        if(currentHealth <= 0)
            Die();
        print("Player Damaged. New Health: " + currentHealth);
    }

    public void Heal(float healAmt) {
        currentHealth = Mathf.Clamp(currentHealth + healAmt, 0, maxHealth);
    }
}
