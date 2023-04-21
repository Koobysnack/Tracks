using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyController : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] protected float maxHealth;
    protected float currentHealth;

    [Header("Player Info")]
    [SerializeField] protected Transform player; // eventually this should be gotten from game manager
    [SerializeField] protected LayerMask playerLayer;

    [Header("Attacking")]
    [SerializeField] protected GameObject attackObj;
    [SerializeField] protected float attackChance;
    [SerializeField] protected float telegraphTime;
    protected bool attacking;
    protected bool attackedLast;

    protected NavMeshAgent agent;
    protected EnemyMovement movement;
    protected Transform model;
    
    // protected functions
    protected abstract void InitiateAttack();
    protected abstract void Die();
    protected abstract IEnumerator Telegraph();
    protected abstract IEnumerator Attack();

    // public functions
    public abstract void TakeDamage(float damage);
}
