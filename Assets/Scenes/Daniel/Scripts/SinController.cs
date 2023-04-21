using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinController : MonoBehaviour
{


    ///Stores charge values of sins, handles recharge, and is the second gatekeep before firing
  

    private int ChamberNum;

    [SerializeField]
    private List<AbsSinClass> SevenSinsList; 

    public void SetChamber(int change)
    {
        ChamberNum = change;
    }

    void Awake()
    {
        AddSin(0);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //TestEnvy.SinFire(transform.gameObject);
    } 
    //Non piercing raycast

    public void AddSin(int sinType)
    {
        switch(sinType){
           case 0:
                SevenSinsList.Add(GetComponent<EnvySin>());
                break;
            default:
                break;
        }
    }

    public void FireSinX()
    {
        if (ChamberNum > SevenSinsList.Count)
        {
            return;
        }
        SevenSinsList[ChamberNum].SinFire();
    }

  
}
