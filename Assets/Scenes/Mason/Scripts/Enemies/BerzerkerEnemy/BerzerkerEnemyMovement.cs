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

    public override void MoveToAttackPosition(NavMeshAgent agent, Vector3 movePos) {
        agent.SetDestination(movePos);
    }

    public override void MovetoCoverPosition(NavMeshAgent agent, Vector3 movePos)
    {
        throw new System.NotImplementedException();
    }
}
