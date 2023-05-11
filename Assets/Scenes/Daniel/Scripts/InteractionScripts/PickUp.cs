using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour, IInteractable
{
    // Start is called before the first frame update
    [SerializeField]
    protected PlayerController PlayerRef;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Interaction()
    {
        print("funny");
    }
}
