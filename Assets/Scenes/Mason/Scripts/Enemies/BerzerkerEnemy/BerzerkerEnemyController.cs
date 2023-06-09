using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BerzerkerEnemyController : EnemyController
{
    [Header("Berzerker")]
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float explosionRange;
    private Animator anim;

    #region Unity Functions
    private void Awake() {
        movement = GetComponent<EnemyMovement>();
        alert = GetComponent<EnemyAlert>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        currentHealth = maxHealth;
    }

    private void Update() {
        if(player == null) {
            player = GameManager.instance.player;
            playerLayer = GameManager.instance.playerLayer;
            return;
        }

        if(GameManager.instance.playerDead) {
            agent.destination = transform.position;
            StopAllCoroutines();
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

        anim.SetBool("isRunning", agent.remainingDistance > 0.1f);
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
        yield return new WaitForSeconds(minTelegraphTime);
        StartCoroutine("Attack");
    }

    protected override IEnumerator Attack() {
        if(Vector3.Distance(transform.position, player.position) < explosionRange)
            player.GetComponent<PlayerController>().Damage(damage, transform);
        Instantiate(explosionPrefab, transform.position, explosionPrefab.transform.rotation);
        Destroy(gameObject);
        yield break;
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
