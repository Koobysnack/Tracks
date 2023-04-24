using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BerzerkerEnemyController : EnemyController
{
    [Header("Berzerker Stats")]
    [SerializeField] private float explosionRange;
    [SerializeField] private float damageAmount;

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
        
        // attack if alert
        if(alert.status == EnemyAlert.AlertStatus.ALERT) {
            if(Vector3.Distance(transform.position, player.position) < explosionRange) {
                agent.destination = transform.position;
                InitiateAttack();
            }
            else if(!attacking) {
                Vector3 movePos = movement.GetAttackPosition();
                movement.MoveToAttackPosition(agent, movePos);
            }
        }
    }
    #endregion

    #region Protected Functions
    protected override void InitiateAttack() {
        StartCoroutine("Telegraph");
    }

    protected override void Die() {
        Destroy(gameObject);
    }

    protected override IEnumerator Telegraph() {
        attacking = true;
        yield return new WaitForSeconds(telegraphTime);
        StartCoroutine("Attack");
    }

    protected override IEnumerator Attack() {
        if(Vector3.Distance(transform.position, player.position) < explosionRange)
            player.GetComponent<PlayerController>().Damage(damageAmount, transform);
        Destroy(gameObject);
        yield break;
    }
    #endregion

    #region Public Functions
    public override void Damage(float damage, Transform opponent=null) {
        currentHealth -= damage;
        if(currentHealth <= 0)
            Die();
    }
    #endregion
}
