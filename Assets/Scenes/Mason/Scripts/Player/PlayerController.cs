using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : EntityController
{
    [Header("Player Stats")]
    [SerializeField] private int maxAmmoCount;
    private int currentAmmoCount;

    [Header("Debug")]
    [SerializeField] private float rad;
    [SerializeField] private float angle;

    private void Awake() {
        currentHealth = maxHealth;
    }

    private void Start() {
        GameManager.instance.player = transform;
        GameManager.instance.playerLayer = LayerMask.GetMask("Player");
    }

    private void Update() {
        Vector3 dir = transform.forward * rad;
        float x1 = (dir.x * Mathf.Cos(angle * Mathf.Deg2Rad)) - (dir.z * Mathf.Sin(angle * Mathf.Deg2Rad));
        float z1 = (dir.x * Mathf.Sin(angle * Mathf.Deg2Rad)) + (dir.z * Mathf.Cos(angle * Mathf.Deg2Rad));
        float x2 = (dir.x * Mathf.Cos(-angle * Mathf.Deg2Rad)) - (dir.z * Mathf.Sin(-angle * Mathf.Deg2Rad));
        float z2 = (dir.x * Mathf.Sin(-angle * Mathf.Deg2Rad)) + (dir.z * Mathf.Cos(-angle * Mathf.Deg2Rad));

        Vector3 vec1 = new Vector3(x1, transform.position.y, z1);
        Vector3 vec2 = new Vector3(x2, transform.position.y, z2);

        Debug.DrawLine(transform.position, vec1 + transform.position, Color.red, 0.1f);
        Debug.DrawLine(transform.position, vec2 + transform.position, Color.red, 0.1f);

        if(Input.GetKeyDown(KeyCode.L)) {
            //print(transform.forward);
            // rotate coordinates to match transform.forward

            float randX = Random.Range(x1, x2);

            print(new Vector3(x1, randX, x2));

            float xMid = (x1 + x2) / 2;
            float minZ;
            if(randX >= xMid)
                minZ = (Mathf.Sin((angle / 2) * Mathf.Deg2Rad) / Mathf.Cos((angle / 2) * Mathf.Deg2Rad)) * randX;
            else
                minZ = (Mathf.Sin((-angle / 2) * Mathf.Deg2Rad) / Mathf.Cos((-angle / 2) * Mathf.Deg2Rad)) * randX;
            float maxZ = Mathf.Sqrt((rad * rad) - (randX * randX));
            float randZ = Random.Range(minZ, maxZ);

            Vector3 randPos = new Vector3(randX, 0, randZ) + transform.position;
            Vector3 hightPoint = new Vector3(randX, 100, randZ) + transform.position;
            Debug.DrawLine(randPos, hightPoint, Color.red, 1f);
            //print(randPos);
        }
    }

    protected override void Die() {
        print("Player is dead");
    }

    public override void Damage(float dmgAmt, Transform opponent) {
        currentHealth -= dmgAmt;
        if(currentHealth <= 0)
            Die();
        print("Player Damaged. New Health: " + currentHealth);
        if(UIDamageIndicatorManager.instance != null)
            UIDamageIndicatorManager.instance.PlayerHit(opponent);
    }

    public void Heal(float healAmt) {
        currentHealth = Mathf.Clamp(currentHealth + healAmt, 0, maxHealth);
    }

    public float GetHealthPercent() {
        return currentHealth / maxHealth;
    }

    public void AddAmmo(int amt) {
        currentAmmoCount = Mathf.Clamp(currentAmmoCount + amt, 0, maxAmmoCount);
    }
}
