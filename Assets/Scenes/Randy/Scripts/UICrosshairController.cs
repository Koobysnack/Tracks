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

    public void RotateTo(int chamber, float direction)
    {
        float targetAngle = 360f / 7f * chamber;
        if (Mathf.Sign(targetAngle) < 0)
            targetAngle -= 360f;
        print(targetAngle);
        StartCoroutine(coordinator.RotateChamber(targetAngle));
    }
}
