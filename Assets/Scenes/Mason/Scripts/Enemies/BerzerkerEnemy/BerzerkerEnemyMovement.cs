using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BerzerkerEnemyMovement : EnemyMovement
{
    public override Vector3 GetAttackPosition() {
        return GameManager.instance.player.position;
    }

    public override Vector3 GetCoverPosition()
    {
        throw new System.NotImplementedException();
    }

    public override void MoveToAttackPosition(NavMeshAgent agent, Vector3 movePos) {//, Transform player, LayerMask playerLayer) {
        //bool canSeePlayer = Physics.Raycast(movePos, player.position - movePos, Mathf.Infinity, playerLayer);
        //if(canSeePlayer)
        agent.SetDestination(movePos);
    }

    public override void MovetoCoverPosition(NavMeshAgent agent, Vector3 movePos)
    {
        throw new System.NotImplementedException();
    }
}
