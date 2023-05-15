using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interact: MonoBehaviour
{
    // Start is called before the first frame update
    
    private GameObject subject;
    private PlayerInputActions pInput;
    private LayerMask interLayer;

    [SerializeField]
    private PlayerController PlayerRef;

    void Awake()
    {
        pInput = new PlayerInputActions();
        pInput.Player.Interact.performed += InteractRay;
        interLayer = LayerMask.GetMask("Interact");
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
  
    //Works by casting ray on button press, will only activate Objects implementing IInteractable.
     public void InteractRay(InputAction.CallbackContext context)
    {
        RaycastHit GetObj;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out GetObj, 100f, interLayer))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * GetObj.distance, Color.red, 1000);
            if (GetObj.transform.gameObject.TryGetComponent<PickUp>(out PickUp  GotInterface )) {
                print("getgot)");
                GotInterface.Interaction();
            }
            print(GetObj.transform.gameObject);
        }
 
    }
   
}
