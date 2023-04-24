using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAlert : MonoBehaviour
{
    public enum AlertStatus { UNALERT, ALERT };

    [SerializeField] private float viewAngle;
    [SerializeField] private float viewDistance;
    [HideInInspector] public AlertStatus status { get; private set; }

    private Transform player;
    private LayerMask playerLayer;

    private void Awake() {
        status = AlertStatus.UNALERT;
    }

    private void Update() {
        // once alert, always alert
        if(status == AlertStatus.ALERT)
            return;

        if(player == null) {
            player = GameManager.instance.player;
            playerLayer = GameManager.instance.playerLayer;
            return;
        }

        // get player direction and check if within FOV
        Vector3 playerDir = player.position - transform.position;
        if(Vector3.Angle(transform.forward, playerDir) <= viewAngle) {
            // change alert status based on if player is visible
            status = CheckPlayerVisible(playerDir) ? AlertStatus.ALERT : AlertStatus.UNALERT;
        }
    }

    private bool CheckPlayerVisible(Vector3 direction) {
        if(Physics.Raycast(transform.position, direction, viewDistance, playerLayer))
            return true;
        return false;
    }

}
