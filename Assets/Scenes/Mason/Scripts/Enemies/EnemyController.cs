using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyController : EntityController
{
    [Header("Enemy Spawn Particles")]
    [SerializeField] protected GameObject spawnParticlesPrefab;

    [Header("Children")]
    [SerializeField] protected Transform model;
    [SerializeField] protected Transform firePoint;

    [Header("Attacking")]
    [SerializeField] protected GameObject attackObj;
    [SerializeField] protected float minTelegraphTime;
    [SerializeField] protected float maxTelegraphTime;
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

    private void OnEnable() {
        Instantiate(spawnParticlesPrefab, transform.position, transform.rotation);
    }
}
