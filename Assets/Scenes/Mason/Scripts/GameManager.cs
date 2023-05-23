using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    private Checkpoint currentCheckpoint;
    public Transform player;
    public LayerMask playerLayer;
    public UnityAction onPlayerDeath;

    private void Awake() {
        if(instance == null)
            instance = this;
        else if(instance != null)
            Destroy(this);
        DontDestroyOnLoad(this);
        onPlayerDeath += RespawnPlayer;
    }

    public void SetCurrentCheckpoint(Checkpoint cp) {
        if(currentCheckpoint && currentCheckpoint != cp)
            Destroy(currentCheckpoint.gameObject);
        currentCheckpoint = cp;
        print("current checkpoint set to " + currentCheckpoint.cpCoordinates);
    }

    public void RespawnPlayer() {
        player.GetComponent<PlayerController>().Respawn(currentCheckpoint);
    }
}
