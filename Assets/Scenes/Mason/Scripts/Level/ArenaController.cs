using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaController : SectionController
{
    [SerializeField] private List<ArenaDoor> doors;
    private bool arenaEntered;
    private bool allDead;
    
    private void Awake() {
        OpenDoors(false);
        SetEnemySection(this);

        foreach(EnemyWave wave in waves) 
            foreach(Transform enemy in wave.enemies)
                enemy.gameObject.SetActive(false);
    }

    private void Update() {
        waves[currentWave].enemies.RemoveAll(enemy => enemy == null);

        // check if all enemies in current wave are dead
        if(waves[currentWave].enemies.Count == 0) {
            currentWave++;

            // spawn next wave or end arena battle if no more waves
            if(currentWave < waves.Count)
                SpawnWave(true);
            else {
                allDead = true;
                OpenDoors(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player" && !arenaEntered) {
            CloseDoors();
            SpawnWave(false);
            arenaEntered = true;
            allDead = false;
        }
    }

    private void SpawnWave(bool alert) {
        foreach(Transform enemy in waves[currentWave].enemies) {
            enemy.gameObject.SetActive(true);
            enemy.GetComponent<EnemyAlert>().status = alert ? EnemyAlert.AlertStatus.ALERT : EnemyAlert.AlertStatus.UNALERT;
        }
    }

    private void CloseDoors() {
        foreach(ArenaDoor door in doors)
            door.CloseDoor();
    }

    private void OpenDoors(bool destroy) {
        foreach(ArenaDoor door in doors)
            door.OpenDoor();

        if(destroy)
            Destroy(gameObject);
    }

    public void AlertAll() {
        foreach(Transform enemy in waves[currentWave].enemies)
            enemy.GetComponent<EnemyAlert>().status = EnemyAlert.AlertStatus.ALERT;
    }
}
