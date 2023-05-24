using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionController : MonoBehaviour
{
    [System.Serializable]
    protected struct EnemyWave {
        public List<Transform> enemies;
    }

    // make this a list of waves (new struct)
    //[SerializeField] protected List<Transform> enemies;
    [SerializeField] protected List<EnemyWave> waves;
    [HideInInspector] public int currentWave;

    private void Awake() {
        SetEnemySection(this);
        currentWave = 0;
    }

    protected void SetEnemySection(SectionController _section) {
        //foreach(Transform enemy in enemies)
        //    enemy.GetComponent<EnemyController>().section = _section;
        foreach(EnemyWave wave in waves)
            foreach(Transform enemy in wave.enemies)
                enemy.GetComponent<EnemyController>().section = _section;
    }

    public List<Transform> GetEnemiesInWave() {
        return waves[currentWave].enemies;
    }
}
