using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public Transform player;
    public LayerMask playerLayer;

    private void Awake() {
        if(instance == null)
            instance = this;
        else if(instance != null)
            Destroy(this);
    }
}
