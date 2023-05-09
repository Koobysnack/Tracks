using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MidEnemyMovement : EnemyMovement
{
    [SerializeField] private int numSamples;
    /*
    public override Vector3 GetAttackPosition() {
        Transform player = GameManager.instance.player;
        LayerMask playerLayer = GameManager.instance.playerLayer;

        // find position near player
        bool goodPos = false;
        Vector3 pos = transform.position;
        while(!goodPos) {
            // NEED TO SAMPLE NAVMESH
            // TRY TO ONLY WALK WITHIN WALK RADIUS
            // OR
            // SAMPLE SEVERAL PLACES WITHIN RANGE AND GET THE ONE WITH THE BEST SCORE
            // CAN SEE PLAYER: +SCORE
            // CLOSER TO IDEAL RANGE: +SCORE


            pos = (Random.insideUnitSphere * maxComfortRange) + player.position;
            if(GoodPosition(pos, player, playerLayer))
                goodPos = true;
        }
        return pos;
    }
    */

    private float GetScore(Vector3 pos) {
        float score = 0;
        Transform player = GameManager.instance.player;

        // check if can see player
        RaycastHit hit;
        Vector3 dir = Vector3.Normalize(player.position - pos);
        Debug.DrawRay(pos, dir, Color.red, 0.5f);
        bool canSeePlayer = Physics.Raycast(pos, dir, out hit, Mathf.Infinity);
        canSeePlayer = hit.transform != null ? hit.transform.tag == "Player" : false;
        score += canSeePlayer ? 10 : 0;

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

    // TODO: add PositionBase parameter that can be used to find a new position from
    // i.e. can be transform.position or agent.destination
    public override Vector3 GetAttackPosition() {
        float[] scores = new float[numSamples];
        Vector3[] positions = new Vector3[numSamples];

        // sample positions, score them, and store in arrays
        for(int i = 0; i < numSamples; ++i) {
            NavMeshHit hit;
            Vector3 pos = (Random.insideUnitSphere * walkRadius) + transform.position;  // position base will be used here
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
        RaycastHit hit;
        bool canSeePlayer = Physics.Raycast(pos, player.position - pos, out hit, Mathf.Infinity);
        canSeePlayer = hit.transform != null ? hit.transform.tag == "Player" : false;
        float playerDist = Vector3.Distance(pos, player.position);
        return playerDist <= maxComfortRange && canSeePlayer;
    }

    
}
