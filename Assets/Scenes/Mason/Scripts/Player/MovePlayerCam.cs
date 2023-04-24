using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerCam : MonoBehaviour
{
    [SerializeField] private Transform playerCamContainer;

    private void Update() {
        transform.position = playerCamContainer.position;
        transform.rotation = playerCamContainer.rotation;
    }
}
