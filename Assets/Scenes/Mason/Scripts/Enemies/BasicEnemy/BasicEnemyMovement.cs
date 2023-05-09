using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyMovement : EnemyMovement
{
    public override Vector3 GetAttackPosition() {
        Vector3 point = (Random.insideUnitSphere * walkRadius) + transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(point, out hit, walkRadius, 1);
        return hit.position;
    }

    public override Vector3 GetCoverPosition() {
        throw new System.NotImplementedException();
    }

    public override bool GoodPosition(Vector3 pos, Transform player)
    {
        throw new System.NotImplementedException();
    }

    public override void MoveToAttackPosition(NavMeshAgent agent, Vector3 movePos) {
        Transform player = GameManager.instance.player;
        LayerMask playerLayer = GameManager.instance.playerLayer;
        bool canSeePlayer = Physics.Raycast(movePos, player.position - movePos, Mathf.Infinity, playerLayer);
        if(canSeePlayer)
            agent.SetDestination(movePos);
    }

    public override void MovetoCoverPosition(NavMeshAgent agent, Vector3 movePos) {
        throw new System.NotImplementedException();
    }
}
