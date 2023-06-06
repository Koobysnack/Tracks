using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickUp : PickUp
{
    // Start is called before the first frame update

    public int ammoAmount;
    public int ammoRandMax;
    public int ammoRandMin;
    public bool ammoRand;
    void Start()
    {
        //PlayerRef = GameObject.Find("PlayerContainer/Player").GetComponent<PlayerController>();
        PlayerRef = GameManager.instance.player.GetComponent<PlayerController>();
        print(PlayerRef);
        if (ammoRand)
        {
            ammoAmount = RandAmmoAmount(ammoRandMax,ammoRandMin);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   public override void Interaction()
    {
        print("getAmmo");
        PlayerRef.ChangeAmmo(ammoAmount);

    }



    private int RandAmmoAmount(int ammoRandMax, int ammoRandMin)
    {
        return Random.Range(ammoRandMin,ammoRandMax);
    }
}
