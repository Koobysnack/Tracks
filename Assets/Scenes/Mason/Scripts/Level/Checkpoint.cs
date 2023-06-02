using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Vector3 cpCoordinates;
    public float cpPlayerRotation;
    
    [HideInInspector] public float playerCurHealth;
    [HideInInspector] public int playerCurAmmo;

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {
            GameManager.instance.SetCurrentCheckpoint(this);
            PlayerController player = GameManager.instance.player.GetComponent<PlayerController>();
            // SaveData.instance.playerData.currentHealth = player.GetCurrentHealth();
            // SaveData.instance.playerData.currentAmmoCount = player.currentAmmoCount;
            playerCurHealth = player.GetCurrentHealth();
            playerCurAmmo = player.currentAmmoCount;
        }
    }
}
