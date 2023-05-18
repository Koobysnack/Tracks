using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionController : MonoBehaviour
{
    [SerializeField] protected List<Transform> enemies;

    private void Awake() {
        SetEnemySection(this);
    }

    protected void SetEnemySection(SectionController _section) {
        foreach(Transform enemy in enemies)
            enemy.GetComponent<EnemyController>().section = _section;
    }

    public List<Transform> GetEnemies() {
        return enemies;
    }
}
