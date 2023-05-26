using System.Collections;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    public float minIntensity = 0.25f;
    public float maxIntensity = 0.5f;

    public float minFlickerSpeed = 0.07f;
    public float maxFlickerSpeed = 0.13f;

    public float transitionTime = 1.0f;

    private Light flickerLight;
    private float targetIntensity;

    private void Start()
    {
        flickerLight = GetComponent<Light>();
        targetIntensity = flickerLight.intensity;
        StartCoroutine(FlickerRoutine());
    }

    private IEnumerator FlickerRoutine()
    {
        while (true)
        {
            // Choose a random target intensity
            targetIntensity = Random.Range(minIntensity, maxIntensity);

            // Interpolate between current intensity and target intensity over transitionTime seconds
            float elapsed = 0f;
            float startIntensity = flickerLight.intensity;
            while (elapsed < transitionTime)
            {
                elapsed += Time.deltaTime;
                flickerLight.intensity = Mathf.Lerp(startIntensity, targetIntensity, elapsed / transitionTime);
                yield return null;
            }

            // Wait for a random interval before transitioning again
            yield return new WaitForSeconds(Random.Range(minFlickerSpeed, maxFlickerSpeed));
        }
    }
}
