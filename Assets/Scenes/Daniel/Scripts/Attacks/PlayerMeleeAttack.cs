using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMeleeAttack : MonoBehaviour
{
    private PlayerInputActions pInput;

    private MeleeAttack pMelee;

    void Awake()
    {

        pMelee = GetComponent<MeleeAttack>();
        pInput = new PlayerInputActions();
        pInput.Attack.Melee.performed += AttackBoxCast;
      


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

    private void OnEnable()
    {
        pInput.Enable();
    }

    private void OnDisable()
    {
        pInput.Disable();
    }


    public void AttackBoxCast(InputAction.CallbackContext context)
    {
        print("meleed");
        pMelee.ShovelAttack();
    }
   
}
