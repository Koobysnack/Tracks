using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BerzerkerEnemyMovement : EnemyMovement
{
    public override Vector3 GetAttackPosition(Vector3 posBase=default(Vector3)) {
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

    public override bool GoodPosition(Vector3 pos, Transform player)
    {
        throw new System.NotImplementedException();
    }

    public override bool InComfortRange(Vector3 pos)
    {
        throw new System.NotImplementedException();
    }
}
