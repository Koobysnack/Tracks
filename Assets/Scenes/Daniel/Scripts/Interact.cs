using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
   /* // Start is called before the first frame update
    private GameObject subject;
    private PlayerInputActions pInput;

    void Awake()
    {
        pInput = new PlayerInputActions();
        pInput.Player.Interact.performed += Interact;
        
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        pInput.Enable();
    }

    private void OnDisable()
    {
        pInput.Disable();
    }

    //Used to trigger UI on looking at interactable things
    //Layer 
    public void InteractCheckRay()
    {
        RaycastHit GetObj;
        if(Physics.Raycast(transform.position,transform.TransformDirection(Vector3.forward),out GetObj, 100f))
        {
            
        }
     

    }
    //Works by casting ray on button press, will only activate Objects implementing IInteractable.
     public void Interact(InputAction.CallbackContext context)
    {
        RaycastHit GetObj;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out GetObj, 100f, layerMask))
        {
            
        }
       
    }
   */
}
