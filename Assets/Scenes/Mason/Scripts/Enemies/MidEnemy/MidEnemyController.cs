using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MidEnemyController : EnemyController
{
    [Header("Mid Enemy")]
    [SerializeField] private GameObject ammoPrefab;
    [SerializeField] private float aimVariance;
    [SerializeField] private float rotationSpeed;
    private PlayerMovement pMovement;
    private Animator anim;
    private bool shot;

    private bool telegraphIsDone;
    private bool shotIsDone;

    #region Unity Functions
    private void Awake() {
        movement = GetComponent<EnemyMovement>();
        alert = GetComponent<EnemyAlert>();
        agent = GetComponent<NavMeshAgent>();
        currentHealth = maxHealth;
        shot = false;

        telegraphIsDone = false;
        shotIsDone = false;
    }

    private void Update() {
        if(player == null) {
            player = GameManager.instance.player;
            pMovement = player.GetComponent<PlayerMovement>();
            anim = GetComponentInChildren<Animator>();
            playerLayer = GameManager.instance.playerLayer;
            return;
        }

        if(GameManager.instance.playerDead) {
            StopAllCoroutines();
            return;
        }

        // move and attack if alert
        if(alert.status == EnemyAlert.AlertStatus.ALERT) {
            // if in bad position or shot while attacking
            if(!movement.GoodPosition(agent.destination + Vector3.up, player) || (shot && attacking)) {
                // find new position
                bool inRange = movement.InComfortRange(agent.destination);
                Vector3 movePos = movement.GetAttackPosition(inRange ? transform.position : agent.destination);
                movement.MoveToAttackPosition(agent, movePos);
                shot = false;   
            }
            else {
                // attack if in good position and not attacking
                if(agent.remainingDistance < 0.1f && !attacking)
                    InitiateAttack();
            }

            // rotate towards player when attacking
            if(attacking)
                RotateTowardsPlayer();
            else
                agent.updateRotation = true;
        }

        if(agent.remainingDistance > 0.1f) {
            anim.SetBool("isMoving", true);
        }
        else {
            anim.SetBool("isMoving", false);
        }
        
    }
    #endregion

    #region PrivateFunctions
    private void RotateTowardsPlayer() {
        // rotate body to player
        agent.updateRotation = false;
        Quaternion target = Quaternion.LookRotation(player.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * rotationSpeed);
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);

        // random aiming
        float aimX = Random.Range(-aimVariance, aimVariance);
        Vector3 aimVarianceVec = pMovement.GetMoveDir().normalized * aimX;
        firePoint.LookAt(player.position + aimVarianceVec, transform.up);
    }
    #endregion

    #region Protected Functions
    protected override void InitiateAttack() {
        // attack if can see player
        attacking = true;
        bool canSeePlayer = Physics.Raycast(transform.position, (player.position - transform.position).normalized , Mathf.Infinity, playerLayer);
        if(canSeePlayer)
            StartCoroutine("Telegraph");
        else
            attacking = false;
    }

    protected override void Die() {
        if(ammoPrefab)
            Instantiate(ammoPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    protected override IEnumerator Telegraph() {
        // do some telegraph stuff here
        float playerDist = Vector3.Distance(transform.position, player.position);
        Vector2 range = movement.GetRange();
        anim.SetBool("isShooting", true);
        yield return new WaitUntil(() => telegraphIsDone);
        telegraphIsDone = false;
        StartCoroutine("Attack");
    }

    protected override IEnumerator Attack() {
        // TODO: Fire sound
        firePoint.GetComponent<HitScanRaycast>().PierceRayCaster();
        yield return new WaitUntil(() => shotIsDone);
        attacking = false;
        attackedLast = true;
        shotIsDone = false;
        anim.SetBool("isShooting", false); 
    }
    #endregion

    #region Public Functions
    public override void Damage(float damage, Transform opponent=null) {
        shot = true;

        // alert enemies in arena or alert self if not in arena
        if(section && section.GetType() == typeof(ArenaController))
            //section.GetType().InvokeMember("AlertAll", System.Reflection.BindingFlags.InvokeMethod, null, section, null);
            ((ArenaController)section).AlertAll();
        else
            alert.status = EnemyAlert.AlertStatus.ALERT;
        
        // deal damage and die if no health
        currentHealth -= damage;
        if(currentHealth <= 0)
            Die();
    }

    public void TelegraphDone() {
        telegraphIsDone = true;
    }

    public void ShotDone() {
        shotIsDone = true;
    }
    #endregion
}
