using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyController : EntityController
{
    [Header("Children")]
    [SerializeField] protected Transform model;
    [SerializeField] protected Transform firePoint;

    [Header("Attacking")]
    [SerializeField] protected GameObject attackObj;
    [SerializeField] protected float attackChance;
    [SerializeField] protected float telegraphTime;
    protected bool attacking;
    protected bool attackedLast;
    
    protected EnemyMovement movement;
    protected EnemyAlert alert;

    protected NavMeshAgent agent;
    protected Transform player;
    protected LayerMask playerLayer;
    public SectionController section;
    
    // protected functions
    protected abstract void InitiateAttack();
    protected abstract IEnumerator Telegraph();
    protected abstract IEnumerator Attack();
}
