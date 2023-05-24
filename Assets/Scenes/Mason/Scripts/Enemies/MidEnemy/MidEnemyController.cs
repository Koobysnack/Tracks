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
            // attack if in good position and not attacking
            if(movement.GoodPosition(agent.destination + Vector3.up, player)) {
                if(agent.remainingDistance < 0.1f && !attacking)
                    InitiateAttack();
            }
            else {
                // find new position
                bool inRange = movement.InComfortRange(agent.destination);
                Vector3 movePos = movement.GetAttackPosition(inRange ? transform.position : agent.destination);
                movement.MoveToAttackPosition(agent, movePos);
            }

            // rotate towards player when attacking
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
        // Vector3 playerDir = player.position - transform.position;
        // if(Vector3.Angle(playerDir, transform.forward) > 5)
        //     transform.Rotate(0, 10 * Time.deltaTime, 0);
    }
    #endregion

    #region Protected Functions
    protected override void InitiateAttack() {
        // attack if can see player
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
        if(section && section.GetType() == typeof(ArenaController))
            section.GetType().InvokeMember("AlertAll", System.Reflection.BindingFlags.InvokeMethod, null, section, null);
        else
            alert.status = EnemyAlert.AlertStatus.ALERT;
        currentHealth -= damage;
        if(currentHealth <= 0)
            Die();
    }
    #endregion
}
