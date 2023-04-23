using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyController : EntityController
{
    [Header("Attacking")]
    [SerializeField] protected GameObject attackObj;
    [SerializeField] protected float attackChance;
    [SerializeField] protected float telegraphTime;
    protected bool attacking;
    protected bool attackedLast;
    
    protected Transform player;
    protected LayerMask playerLayer;
    protected NavMeshAgent agent;
    protected EnemyMovement movement;
    protected Transform model;
    
    // protected functions
    protected abstract void InitiateAttack();
    protected abstract IEnumerator Telegraph();
    protected abstract IEnumerator Attack();
}
