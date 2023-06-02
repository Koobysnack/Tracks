using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinSwitchInteractable : PickUp
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (GameManager.instance && GameManager.instance.player && PlayerRef == null)
            PlayerRef = GameManager.instance.player.gameObject.GetComponent<PlayerController>();
    }

    public override void Interaction()
    {
        PauseManager.instance.TogglePauseSinMenu();
        // Open Menu
        UIChamberMenuManager.instance.OpenMenu();
    }
}
