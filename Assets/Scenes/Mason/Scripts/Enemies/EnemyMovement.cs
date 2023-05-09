using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyMovement : MonoBehaviour
{
    [Header("Base Enemy Movement")]
    [SerializeField] protected float walkRadius;
    [SerializeField] protected float minComfortRange;
    [SerializeField] protected float maxComfortRange;
    [SerializeField] protected float idealRange;

    public abstract Vector3 GetAttackPosition();
    public abstract Vector3 GetCoverPosition();
    public abstract void MoveToAttackPosition(NavMeshAgent agent, Vector3 movePos);
    public abstract void MovetoCoverPosition(NavMeshAgent agent, Vector3 movePos);
    public abstract bool GoodPosition(Vector3 pos, Transform player);
}
