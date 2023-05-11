using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyMovement : MonoBehaviour
{
    [Header("Base Enemy Movement")]
    [SerializeField] protected float walkRadius;
    [SerializeField] protected float idealRange;
    [SerializeField] protected float rangeVariance;

    public abstract Vector3 GetAttackPosition(Vector3 origin = default(Vector3));
    public abstract Vector3 GetCoverPosition();
    public abstract void MoveToAttackPosition(NavMeshAgent agent, Vector3 movePos);
    public abstract void MovetoCoverPosition(NavMeshAgent agent, Vector3 movePos);
    public abstract bool GoodPosition(Vector3 pos, Transform player);
    public abstract bool InComfortRange(Vector3 pos);
}
