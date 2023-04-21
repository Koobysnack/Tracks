using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyController : EnemyController
{
    #region Unity Functions
    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
        movement = GetComponent<EnemyMovement>();
        model = transform.GetChild(0);
        currentHealth = maxHealth;
    }

    private void Update() {
        if(player == null) {
            player = GameManager.instance.player;
            playerLayer = GameManager.instance.playerLayer;
            return;
        }

        if(agent.remainingDistance < 0.1f && !attacking) {
            if(Random.Range(0, 101) <= attackChance && !attackedLast) {
                InitiateAttack();
            }
            else {
                Vector3 movePos = movement.GetAttackPosition();
                movement.MoveToAttackPosition(agent, movePos);
                attackedLast = false;
            }
        }

        if(attacking) {
            RotateTowardsPlayer();
        }
    }
    #endregion

    #region PrivateFunctions
    private void RotateTowardsPlayer() {
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
        throw new System.NotImplementedException();
    }

    protected override IEnumerator Telegraph() {
        // do some telegraph stuff here
        yield return new WaitForSeconds(telegraphTime);
        StartCoroutine("Attack");
    }

    protected override IEnumerator Attack() {
        Vector3 instantiatePos = model.position;
        GameObject shot = Instantiate(attackObj, instantiatePos, transform.rotation);
        shot.GetComponent<TestEnemyBullet>().SetShooter(transform);
        yield return new WaitForSeconds(0.5f);
        attacking = false;
        attackedLast = true;
    }
    #endregion

    #region Public Functions
    public override void TakeDamage(float damage) {
        currentHealth -= damage;
        if(currentHealth <= 0)
            Die();
    }
    #endregion
}
