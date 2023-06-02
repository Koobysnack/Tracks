using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionController : MonoBehaviour
{
    protected enum EnemyType { BERZERKER, RANGED }

    [System.Serializable]
    protected class ArenaEnemy {
        public EnemyType enemyType;
        public Vector3 enemyPosition;
        public Vector3 enemyRotation;
        [HideInInspector] public GameObject enemyObj;

        public ArenaEnemy(ArenaEnemy prevEnemy) {
            enemyType = prevEnemy.enemyType;
            enemyPosition = prevEnemy.enemyPosition;
            enemyRotation = prevEnemy.enemyRotation;
            enemyObj = prevEnemy.enemyObj;
        }
    }

    [System.Serializable]
    protected struct EnemyWave {
        public List<ArenaEnemy> enemies;
    }

    [SerializeField] protected GameObject[] enemyPrefabs;
    [SerializeField] protected List<EnemyWave> waves;
    protected List<EnemyWave> updateWaves;
    [HideInInspector] public int currentWave;

    private void Awake() {
        currentWave = 0;
        updateWaves = CopyWaves();

        for(int i = 0; i < waves.Count; ++i) {
            for(int j = 0; j < waves[i].enemies.Count; ++j) {
                SpawnEnemy(waves[i].enemies[j], this);
            }
        }
    }

    protected List<EnemyWave> CopyWaves() {
        List<EnemyWave> newWaves = new List<EnemyWave>();
        foreach(EnemyWave wave in waves) {
            List<ArenaEnemy> enemies = new List<ArenaEnemy>();
            foreach(ArenaEnemy enemy in wave.enemies) {
                ArenaEnemy copyEnemy = new ArenaEnemy(enemy);
                enemies.Add(copyEnemy);
            }
            EnemyWave copyWave;
            copyWave.enemies = enemies;
            newWaves.Add(copyWave);
        }
        return newWaves;
    }

    protected GameObject SpawnEnemy(ArenaEnemy enemy, SectionController _section) {
        GameObject spawnedEnemy = Instantiate(enemyPrefabs[(int)enemy.enemyType], enemy.enemyPosition, 
                                              Quaternion.Euler(enemy.enemyRotation));
        enemy.enemyObj = spawnedEnemy;
        spawnedEnemy.GetComponent<EnemyController>().section = _section;
        return spawnedEnemy;
    }

    public List<GameObject> GetEnemiesInWave() {
        List<GameObject> enemies = new List<GameObject>();
        foreach(ArenaEnemy enemy in updateWaves[currentWave].enemies)
            enemies.Add(enemy.enemyObj);
        return enemies;
    }
}
