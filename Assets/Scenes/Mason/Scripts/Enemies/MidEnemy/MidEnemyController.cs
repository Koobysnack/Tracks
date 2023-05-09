using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MidEnemyController : EnemyController
{
    #region Unity Functions
    private void Awake() {
        movement = GetComponent<EnemyMovement>();
        alert = GetComponent<EnemyAlert>();
        agent = GetComponent<NavMeshAgent>();
        currentHealth = maxHealth;
    }

    private void Update() {
        if(player == null) {
            player = GameManager.instance.player;
            playerLayer = GameManager.instance.playerLayer;
            return;
        }

        // move and attack if alert
        if(alert.status == EnemyAlert.AlertStatus.ALERT) {
            if(movement.GoodPosition(agent.destination, player)) {
                print("can attack");
                if(agent.remainingDistance < 0.1f && !attacking)
                    InitiateAttack();
            }
            else {
                print("finding new spot");
                // TODO: check if player is within maxComfortRange
                // if so, use transform.position as positionBase
                // otherwise, use agent.destination
                Vector3 movePos = movement.GetAttackPosition();
                movement.MoveToAttackPosition(agent, movePos);
            }

            if(attacking)
                RotateTowardsPlayer();
            else
                agent.updateRotation = true;
        }
    }
    #endregion

    #region PrivateFunctions
    private void RotateTowardsPlayer() {
        agent.updateRotation = false;
        transform.LookAt(player.position, transform.up);
    }
    #endregion

    #region Protected Functions
    protected override void InitiateAttack() {
        attacking = true;
        bool canSeePlayer = Physics.Raycast(transform.position, player.position - transform.position, Mathf.Infinity, playerLayer);
        if(canSeePlayer)
            StartCoroutine("Telegraph");
        else
            attacking = false;
    }

    protected override void Die() {
        Destroy(gameObject);
    }

    protected override IEnumerator Telegraph() {
        // do some telegraph stuff here
        yield return new WaitForSeconds(telegraphTime);
        StartCoroutine("Attack");
    }

    protected override IEnumerator Attack() {
        GameObject shot = Instantiate(attackObj, firePoint.position, transform.rotation);
        shot.GetComponent<TestEnemyBullet>().SetShooter(transform);
        yield return new WaitForSeconds(0.5f);
        attacking = false;
        attackedLast = true;
    }
    #endregion

    #region Public Functions
    public override void Damage(float damage, Transform opponent=null) {
        print("Basic Enemy Damaged");
        currentHealth -= damage;
        if(currentHealth <= 0)
            Die();
    }
    #endregion
}
