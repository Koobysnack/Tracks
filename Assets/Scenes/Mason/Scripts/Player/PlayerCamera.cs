using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private Transform player;
    private float horiz;
    private float vert;

    private void Awake() {
        player = transform.parent;
        horiz = player.rotation.eulerAngles.y;
        vert = 0;
    }
}
