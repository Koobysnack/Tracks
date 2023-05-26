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

    public bool playerDead;

    private void Awake() {
        if(instance == null)
            instance = this;
        else if(instance != null)
            Destroy(this);
        DontDestroyOnLoad(this);
        playerDead = false;
        onPlayerDeath += PlayerDeath;
    }

    public void SetCurrentCheckpoint(Checkpoint cp) {
        if(currentCheckpoint && currentCheckpoint != cp)
            Destroy(currentCheckpoint.gameObject);
        currentCheckpoint = cp;
        print("current checkpoint set to " + currentCheckpoint.cpCoordinates);
    }

    private void PlayerDeath() {
        Time.timeScale = 0.5f;
        playerDead = true;
        StartCoroutine("TestWaitForRespawn");
    }

    private IEnumerator TestWaitForRespawn() {
        yield return new WaitForSeconds(4);
        RespawnPlayer();
    }

    public void RespawnPlayer() {
        Time.timeScale = 1;
        playerDead = false;
        player.GetComponent<PlayerController>().Respawn(currentCheckpoint);
    }
}
