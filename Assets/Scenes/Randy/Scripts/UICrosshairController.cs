using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICrosshairController : MonoBehaviour
{
    [SerializeField] UICrosshairCoordinator coordinator;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowAmmoPanel()
    {
        // More code may be needed later on depending on creative direction
        coordinator.ShowDisplay();
    }

    public void RotateTo(int chamber, float direction)
    {
        float targetAngle = 360f / 7f * chamber;
        //print(targetAngle + "," + direction);
        StartCoroutine(coordinator.RotateChamber(targetAngle, direction));
    }
}
