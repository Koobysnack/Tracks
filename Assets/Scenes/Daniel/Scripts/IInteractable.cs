using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable 
{
    // Start is called before the first frame update
    public void Interaction();

    public void EnableInteraction();

    public void DisableInteraction();
 }
