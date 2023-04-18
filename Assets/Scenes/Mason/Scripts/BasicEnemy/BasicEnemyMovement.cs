using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyMovement : MonoBehaviour
{
    [SerializeField] private float walkRadius;

    public Vector3 GetAttackPosition() {
        Vector3 point = (Random.insideUnitSphere * walkRadius) + transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(point, out hit, walkRadius, 1);
        return hit.position;
    }

    public void MoveToAttackPosition(NavMeshAgent agent, Vector3 movePos, Transform player, LayerMask playerLayer) {
        bool canSeePlayer = Physics.Raycast(movePos, player.position - movePos, Mathf.Infinity, playerLayer);
        if(canSeePlayer)
            agent.SetDestination(movePos);
    }
}
