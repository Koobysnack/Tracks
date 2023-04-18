using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Player Info")]
    [SerializeField] private Transform player; // eventually this should be gotten in player manager
    [SerializeField] private LayerMask playerLayer;

    [Header("Attackcing")]
    [SerializeField] private GameObject attackObj;
    [SerializeField] private float attackChance;
    [SerializeField] private float telegraphTime;
    private bool attacking;
    private bool attackedLast;

    private NavMeshAgent agent;
    private BasicEnemyMovement movement;

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
        movement = GetComponent<BasicEnemyMovement>();
    }

    private void Update() {
        if(agent.remainingDistance < 0.1f && !attacking) {
            if(Random.Range(0, 101) <= attackChance && !attackedLast) {
                InitiateAttack();
            }
            else {
                Vector3 movePos = movement.GetAttackPosition();
                movement.MoveToAttackPosition(agent, movePos, player, playerLayer);
                attackedLast = false;
            }
        }

        if(attacking) {
            RotateTowardsPlayer();
        }
    }

    private void RotateTowardsPlayer() {
        transform.LookAt(player.position, transform.up);
    }

    private void InitiateAttack() {
        attacking = true;
        bool canSeePlayer = Physics.Raycast(transform.position, player.position - transform.position, Mathf.Infinity, playerLayer);
        if(canSeePlayer)
            StartCoroutine("Telegraph");
        else
            attacking = false;
    }

    private void Attack() {
        Vector3 instantiatePos = transform.GetChild(0).position;
        Instantiate(attackObj, instantiatePos, transform.rotation);
        attacking = false;
        attackedLast = true;
    }

    private IEnumerator Telegraph() {
        // do some telegraph stuff here
        yield return new WaitForSeconds(telegraphTime);
        Attack();
    }
}
