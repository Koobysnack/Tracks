using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidEnemyAnimController : MonoBehaviour
{
    private MidEnemyController enemy;

    private void Awake() {
        enemy = GetComponentInParent<MidEnemyController>();
    }

    public void SetTelegraphDone()  {
        enemy.TelegraphDone();
    }

    public void SetShotDone() {
        enemy.ShotDone();
    }
}
