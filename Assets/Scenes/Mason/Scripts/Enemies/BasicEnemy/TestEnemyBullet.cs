using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyBullet : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody body;

    private void Awake() {
        body = GetComponent<Rigidbody>();
    }

    private void Start() {
        body.velocity = transform.forward * speed;
        StartCoroutine("DestroyObj");
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag != "Enemy")
            Destroy(gameObject);
    }

    private IEnumerator DestroyObj() {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }
}
