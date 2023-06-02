using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MidEnemyMovement : EnemyMovement
{
    [Header("Mid Enemy Movement")]
    [SerializeField] private int numSamples;
    [SerializeField] private float enemyDistThreshold;

    [Header("Feature Weights")]
    [SerializeField] private float playerVisWeight;
    [SerializeField] private float playerDistWeight;
    [SerializeField] private float enemyDistWeight;

    private Transform player;
    private SectionController section;

    private void Start() {
        section = GetComponent<EnemyController>().section;
    }

    private void Update() {
        if(player == null){
            player = GameManager.instance.player;
        }
    }

    private float GetClosestEnemyDist(Vector3 origin) {
        if(section == null)
            return 0;

        // get closest enemy and add reciporical to score
        List<GameObject> enemies = section.GetEnemiesInWave();
        float[] distances = new float[enemies.Count];
        for(int i = 0; i < enemies.Count; ++i)
            if(enemies[i])
                distances[i] = Vector3.Distance(origin, enemies[i].transform.position);
        float closestDist = distances.Length > 0 ? Mathf.Min(distances) : 0.1f;
        return closestDist;
    }

    private float GetScore(Vector3 pos) {
        float score = 0;
        Transform player = GameManager.instance.player;

        // check if can see player
        RaycastHit hit;
        Vector3 dir = Vector3.Normalize(player.position - pos);
        bool canSeePlayer = Physics.Raycast(pos, dir, out hit, Mathf.Infinity);
        canSeePlayer = hit.transform != null ? hit.transform.tag == "Player" : false;
        score += canSeePlayer ? playerVisWeight : 0;

        // check if near ideal range
        float distFromPlayer = Vector3.Distance(transform.position, player.position);
        float distFromIdeal = Mathf.Abs(idealRange - distFromPlayer);
        distFromIdeal = distFromIdeal != 0 ? 0.1f : distFromIdeal;
        score += playerDistWeight * (1 / distFromIdeal);

        // stay away from other enemies
        score += enemyDistWeight * GetClosestEnemyDist(pos);

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

    private Vector3 GetEdgeVector(Vector3 origin, float angle, float dirCoef) {
        // get direction to player and get extends of edge using rotation of axes
        Vector3 direction = player.position - origin;
        float x = ((direction.x * Mathf.Cos(angle * Mathf.Deg2Rad)) - (direction.z * Mathf.Sin(angle * Mathf.Deg2Rad)));
        float z = ((direction.x * Mathf.Sin(angle * Mathf.Deg2Rad)) + (direction.z * Mathf.Cos(angle * Mathf.Deg2Rad)));
        Vector3 vec = new Vector3(x * walkRadius * dirCoef, origin.y, z * walkRadius * dirCoef);

        Debug.DrawLine(origin, vec + origin, Color.red, 0.1f);
        Debug.DrawLine(origin, direction + origin, Color.blue, 0.1f);
        return new Vector3(x, origin.y, z);
    }

    private Vector3 FindRandomPos(Vector3 origin, float angle) {
        // make right and left edges of arc
        float dirCoef = Vector3.Distance(origin, player.position) < idealRange ? -1 : 1;
        Vector3 arcLeft = GetEdgeVector(origin, -angle, dirCoef);
        Vector3 arcRight = GetEdgeVector(origin, angle, dirCoef);

        // pick random x between cos(theta) * walkRad and -cos(theta) * walkRad
        float xRange = Mathf.Cos(angle * Mathf.Deg2Rad) * walkRadius;
        float randX = Random.Range(-xRange, xRange);

        // pick random z between maxZ and minZ
        // minZ = (sin(theta)/cos(theta))*x
        // maxZ = sqrt(r^2-x^2)
        float newAngle = randX >= 0 ? angle : -angle;
        float minZ = (Mathf.Sin((newAngle / 2) * Mathf.Deg2Rad) / Mathf.Cos((newAngle / 2) * Mathf.Deg2Rad)) * randX;
        float maxZ = Mathf.Sqrt((walkRadius * walkRadius) - (randX * randX));
        float randZ = Random.Range(minZ, maxZ);

        // set forward angle for rotation of axes
        float forwardAngle = -transform.eulerAngles.y;
        if(origin != transform.position) {
            // get rotation to player from origin if origin is not transform position
            Vector3 dir = player.position - origin;
            dir.y = 0;
            Quaternion rot = Quaternion.LookRotation(dir);
            forwardAngle = -rot.eulerAngles.y;
        }

        // rotation of axes for coordinates (randX, randZ)
        float newX = (randX * Mathf.Cos(forwardAngle * Mathf.Deg2Rad)) - (randZ * Mathf.Sin(forwardAngle * Mathf.Deg2Rad));
        float newZ = (randX * Mathf.Sin(forwardAngle * Mathf.Deg2Rad)) + (randZ * Mathf.Cos(forwardAngle * Mathf.Deg2Rad));
        Vector3 vec = new Vector3(newX, origin.y, newZ) + transform.position;
        return vec;
    }

    public override Vector3 GetAttackPosition(Vector3 origin) {
        float[] scores = new float[numSamples];
        Vector3[] positions = new Vector3[numSamples];

        // sample positions, score them, and store in arrays
        for(int i = 0; i < numSamples; ++i) {
            NavMeshHit hit;
            Vector3 pos = FindRandomPos(origin, InComfortRange(origin) ? 90 : 60);
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
        // good position if in not too far and can see player
        RaycastHit hit;
        Vector3 dir = Vector3.Normalize(player.position - pos);
        bool canSeePlayer = Physics.Raycast(pos, dir, out hit, Mathf.Infinity);
        canSeePlayer = hit.transform != null ? hit.transform.tag == "Player" : false;
        //float playerDist = Vector3.Distance(player.position, pos);
        //return playerDist <= (idealRange + farRangeVariance) && canSeePlayer;
        return InComfortRange(pos) && canSeePlayer;
    }

    public override bool InComfortRange(Vector3 pos) {
        // return if player is within ideal range +/- range variance
        float playerDist = Vector3.Distance(player.position, pos);
        return (idealRange - closeRangeVariance) <= playerDist && playerDist <= (idealRange + farRangeVariance);
    }
}
