using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRayControl : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerInputActions pInput;

    private HitScanRaycast RegShotRay;

    private SinController SinShotControl;

    

    public 

    void Awake()
    {

        RegShotRay = GetComponent<HitScanRaycast>();
        SinShotControl = GetComponent<SinController>();
        pInput = new PlayerInputActions();
        pInput.Attack.Fire.performed += RegRay;
        pInput.Attack.AltFire.performed += SinRay;


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


    public void RegRay(InputAction.CallbackContext context)
    {
        RegShotRay.PierceRayCaster();
    }

    public void SinRay(InputAction.CallbackContext context)
    {
        SinShotControl.FireSinX();
    }
}
