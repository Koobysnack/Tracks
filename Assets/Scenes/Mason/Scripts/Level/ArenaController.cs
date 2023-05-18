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

        base.SetEnemySection(this);
    }

    private void Update() {
        enemies.RemoveAll(enemy => enemy == null);

        if(enemies.Count == 0 && !allDead) {
            allDead = true;
            OpenDoors(true);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player" && !arenaEntered) {
            CloseDoors();
            arenaEntered = true;
            allDead = false;
        }
    }

    private void CloseDoors() {
        foreach(ArenaDoor door in doors) {
            // change this to open a door
            door.CloseDoor();
        }
    }

    private void OpenDoors(bool destroy) {
        foreach(ArenaDoor door in doors) {
            // change this to open a door
            door.OpenDoor();
        }

        if(destroy)
            Destroy(gameObject);
    }
}
