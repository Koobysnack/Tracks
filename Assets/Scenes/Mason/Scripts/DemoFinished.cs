using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoFinished : MonoBehaviour
{
    private void Awake() {
        gameObject.SetActive(false);
    }

    public void FadeFinished() {
        GameManager.instance.DemoFinished();
    }
}
