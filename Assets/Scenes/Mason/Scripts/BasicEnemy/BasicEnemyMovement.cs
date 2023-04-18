using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform player; // eventually this should be gotten in player manager
    [SerializeField] private float walkRad;
    private NavMeshAgent agent;

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update() {
        if(agent.remainingDistance < 0.1f) {
            // find new position
            Vector3 pos = GetAttackPosition();
            print(pos);
            bool canSeePlayer = Physics.Raycast(pos, pos + player.position, Mathf.Infinity, player.gameObject.layer);
            Debug.DrawRay(transform.position, player.position, Color.red, 1f);
            if(canSeePlayer) {
                print("ahh");
                agent.SetDestination(pos);
            }
        }
    }

    private Vector3 GetAttackPosition() {
        Vector3 point = (Random.insideUnitSphere * walkRad) + transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(point, out hit, walkRad, 1);
        return hit.position;
    }
}
