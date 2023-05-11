using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MidEnemyMovement : EnemyMovement
{
    [Header("Mid Enemy Movement")]
    [SerializeField] private int numSamples;
    private Transform player;

    private void Update() {
        if(player == null){
            player = GameManager.instance.player;
        }
    }

    private float GetScore(Vector3 pos) {
        float score = 0;
        Transform player = GameManager.instance.player;

        // check if can see player
        RaycastHit hit;
        Vector3 dir = Vector3.Normalize(player.position - pos);
        bool canSeePlayer = Physics.Raycast(pos, dir, out hit, Mathf.Infinity);
        canSeePlayer = hit.transform != null ? hit.transform.tag == "Player" : false;
        score += canSeePlayer ? 10 : 0;
        Debug.DrawRay(pos, dir, Color.red, 0.5f);

        // check if near ideal range
        float distFromPlayer = Vector3.Distance(transform.position, player.position);
        float distFromIdeal = Mathf.Abs(idealRange - distFromPlayer);
        distFromIdeal = distFromIdeal != 0 ? 0.1f : distFromIdeal;
        score += 5 * (1 / distFromIdeal);

        return score;
    }

    private int MaxIndex(float[] arr) {
        float max = arr[0];
        int maxI = 0;

        // loop and find index of largest value
        for(int i = 1; i < arr.Length; ++i) {
            if(arr[i] > max) {
                max = arr[i];
                maxI = i;
            }
        }
        return maxI;
    }

    private Vector3 GetEdgeVector(Vector3 origin, float angle) {
        Vector3 direction = (player.position - origin) * walkRadius;
        float x = (direction.x * Mathf.Cos(angle)) - (direction.z * Mathf.Sin(angle));
        float z = (direction.x * Mathf.Sin(angle)) + (direction.z * Mathf.Cos(angle));
        return new Vector3(x, origin.y, z);
    }

    private Vector3 FindRandomPos(Vector3 origin, float angle) {
        Vector3 arcLeft = GetEdgeVector(origin, -angle);
        Vector3 arcRight = GetEdgeVector(origin, angle);
        // pick random x between sqrt(3)/2 * walkRad and -sqrt(3)/2 * walkRad
        // i.e. random point between arcLeft.x and arcRight.x
        float x = Random.Range(arcLeft.x, arcRight.x);

        // pick random z between maxZ and minZ
        // minZ = (sin(theta)/cos(theta))*x
        // maxZ = sqrt(r^2-x^2)
        float minZ = (Mathf.Sin(angle * Mathf.Deg2Rad) / Mathf.Cos(angle * Mathf.Deg2Rad)) * x;
        float maxZ = Mathf.Sqrt((walkRadius * walkRadius) - (x * x));
        float z = Random.Range(minZ, maxZ);
        return new Vector3(x, origin.y, z);
    }

    public override Vector3 GetAttackPosition(Vector3 origin) {
        float[] scores = new float[numSamples];
        Vector3[] positions = new Vector3[numSamples];

        // sample positions, score them, and store in arrays
        for(int i = 0; i < numSamples; ++i) {
            NavMeshHit hit;
            Vector3 pos = (Random.insideUnitSphere * walkRadius) + origin; // maybe split unit sphere to be half that faces player
            NavMesh.SamplePosition(pos, out hit, walkRadius, 1);
            scores[i] = GetScore(hit.position);
            positions[i] = hit.position;
        }
        
        // return position with best score
        int bestIndex = MaxIndex(scores);
        return positions[bestIndex];
    }

    public override Vector3 GetCoverPosition() {
        throw new System.NotImplementedException();
    }

    public override void MoveToAttackPosition(NavMeshAgent agent, Vector3 movePos) {
        agent.SetDestination(movePos);
    }

    public override void MovetoCoverPosition(NavMeshAgent agent, Vector3 movePos) {
        throw new System.NotImplementedException();
    }

    public override bool GoodPosition(Vector3 pos, Transform player) {
        // good position if in comfort range and can see player
        RaycastHit hit;
        bool canSeePlayer = Physics.Raycast(pos, player.position - pos, out hit, Mathf.Infinity);
        canSeePlayer = hit.transform != null ? hit.transform.tag == "Player" : false;
        return InComfortRange(pos) && canSeePlayer;
    }

    public override bool InComfortRange(Vector3 pos) {
        // return if player is within ideal range +/- range variance
        float playerDist = Vector3.Distance(player.position, pos);
        return (idealRange - rangeVariance) <= playerDist && playerDist <= (idealRange + rangeVariance);
    }
}
