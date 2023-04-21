using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyBullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    private Rigidbody body;
    private Transform shooter;

    private void Awake() {
        body = GetComponent<Rigidbody>();
    }

    private void Start() {
        body.velocity = transform.forward * speed;
        StartCoroutine("DestroyObj");
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Player")
            other.transform.GetComponent<PlayerController>().Damage(damage, shooter);
        if(other.gameObject.tag != "Enemy")
            Destroy(gameObject);
    }

    private IEnumerator DestroyObj() {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }

    public void SetShooter(Transform enemy) {
        shooter = enemy;
    }
}
