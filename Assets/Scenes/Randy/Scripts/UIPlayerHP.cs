using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHP : MonoBehaviour
{
    PlayerController player;
    Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance && GameManager.instance.player && player == null)
            player = GameManager.instance.player.GetComponent<PlayerController>();
        if(player == null)
            return;
        if(slider.value != player.GetHealthPercent())
            slider.value = player.GetHealthPercent();
    }
}
