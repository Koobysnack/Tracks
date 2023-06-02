using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaController : SectionController
{
    [System.Serializable]
    private class ArenaDoor {
        public Vector3 doorPosition;
        public Vector3 doorRotation;
        [HideInInspector] public GameObject doorObj;
    }

    [SerializeField] private GameObject doorPrefab;
    [SerializeField] private List<ArenaDoor> doors;
    private bool arenaEntered;
    private bool spawnedFirstWave;
    
    private void Awake() {
        OpenDoors(false);
        updateWaves = CopyWaves();
        spawnedFirstWave = false;
    }

    private void Update() {
        if(!spawnedFirstWave)
            return;

        // check if all enemies in current wave are dead
        if(updateWaves[currentWave].enemies.Count == 0) {
            currentWave++;

            // spawn next wave or end arena battle if no more waves
            if(currentWave < updateWaves.Count)
                SpawnWave(true);
            else
                OpenDoors(true);
        }

        if(currentWave < updateWaves.Count)
            updateWaves[currentWave].enemies.RemoveAll(enemy => enemy.enemyObj == null);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player" && !arenaEntered) {
            currentWave = 0;
            CloseDoors();
            SpawnWave(false);
            arenaEntered = true;
        }
    }

    private void SpawnWave(bool alert) {
        for(int i = 0; i < updateWaves[currentWave].enemies.Count; ++i) {
            GameObject spawnedEnemy = SpawnEnemy(updateWaves[currentWave].enemies[i], this);
            spawnedEnemy.GetComponent<EnemyAlert>().status = alert ? EnemyAlert.AlertStatus.ALERT : EnemyAlert.AlertStatus.UNALERT;
        }
        spawnedFirstWave = true;
    }

    private void CloseDoors() {
        foreach(ArenaDoor door in doors)
            door.doorObj = Instantiate(doorPrefab, door.doorPosition, Quaternion.Euler(door.doorRotation));
    }

    private void OpenDoors(bool destroy) {
        foreach(ArenaDoor door in doors) {
            if(door.doorObj)
                Destroy(door.doorObj);
        }

        if(destroy)
            Destroy(gameObject);
    }

    public void AlertAll() {
        List<GameObject> enemiesInWave = GetEnemiesInWave();
        foreach(GameObject enemy in enemiesInWave)
            enemy.GetComponent<EnemyAlert>().status = EnemyAlert.AlertStatus.ALERT;
    }
}
