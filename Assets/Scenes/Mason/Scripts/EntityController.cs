using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityController : MonoBehaviour
{
    [Header("Entity Stats")]
    [SerializeField] protected float maxHealth;
    protected float currentHealth;

    protected abstract void Die();
    public abstract void Damage(float dmgAmt, Transform opponent=null);
}
