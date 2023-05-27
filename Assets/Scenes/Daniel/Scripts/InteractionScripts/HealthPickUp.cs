using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : PickUp {
    // Start is called before the first frame update
    
    public int HealthAmount;

    void Start()
    {
        PlayerRef = GameObject.Find("PlayerContainerVCAM/Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interaction()
    {
      //  print("getHealth");
        PlayerRef.Heal(HealthAmount);
        Destroy(gameObject);

    }

}

