using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthVFXController : MonoBehaviour
{
    // TODO: Reference to effect to change values
    [Range(0f, 1f)] float activeIntensity;
    float windDownTimer;
    [SerializeField, Range(0f, 1f)] float targetIntensity;
    [SerializeField, Range(0f, 1f), Tooltip("The percentage of the vfx intensity the effect will fall back to when not in use. 0 = Completely disappear, 1 = Don't wind down")]
        float standbyIntensity;
    [SerializeField, Range(0f, 1f), Tooltip("The maximum rate at which the effect will ramp up in intensity, after being hit")]
        float rampUpRate;
    [SerializeField, Range(0f, 1f), Tooltip("The maximum rate at which the effect will wind down in intensity, after not being hit")]
        float windDownRate;
    [SerializeField, Tooltip("The amount of time (seconds) after getting hit to wind down intensity")]
        float windDownDelay;
    [SerializeField] Material mat;
    [SerializeField] string floatVar;

    // Start is called before the first frame update
    void Start()
    {
        activeIntensity = 0;
        mat.SetFloat(floatVar, 1);
        foreach(string s in mat.GetTexturePropertyNames())
        {
            print(s);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetIntensity(float v)
    {
        targetIntensity = v;
        StartCoroutine(RampUp());
    }

    IEnumerator RampUp()
    {
        // Cue wind down
        StartCoroutine(WindDown());
        // Handle ramp up
        float step = rampUpRate * Time.deltaTime;
        while (activeIntensity < targetIntensity)
        {
            activeIntensity += (activeIntensity + step) > targetIntensity ? targetIntensity - activeIntensity : step;
            // TODO: Set vfx intensity value based on activeIntensity
            mat.SetFloat(floatVar, 1 - activeIntensity);
            yield return null;
        }
    }

    IEnumerator WindDown()
    {
        // Ensure only one timer is running. Reset existing timer if waiting to wind down
        if(windDownTimer > 0f)
        {
            windDownTimer -= windDownDelay;
            yield break;
        }
        windDownTimer = windDownDelay;
        // Delay winding down by waiting on timer
        while(windDownTimer > 0f)
        {
            windDownTimer -= Time.deltaTime;
            yield return null;
        }
        float step = -windDownRate * Time.deltaTime;
        // Perform wind down. Interrupt if damaged again
        while (activeIntensity > 0f && windDownTimer <= 0f)
        {
            activeIntensity += (activeIntensity + step) < 0 ? -activeIntensity : step;
            // TODO: Set vfx intensity value based on activeIntensity
            mat.SetFloat(floatVar, 1 - activeIntensity);
            yield return null;
        }

    }
}
