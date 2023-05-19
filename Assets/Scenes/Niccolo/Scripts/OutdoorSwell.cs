using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutdoorSwell : MonoBehaviour
{
    public enum AXIS { X, Y, Z };
    public AXIS axis;
    public float scale;

    private BoxCollider col;
    private FMODUnity.StudioParameterTrigger parameterTrigger;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<BoxCollider>();
        parameterTrigger = GetComponent<FMODUnity.StudioParameterTrigger>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private float GetDistance(Vector3 other)
    {
        Vector3 diff = (col.center + col.transform.position) - other;
        float dist;
        float maxDist;
        
        switch (axis)
        {
            case AXIS.X:
                dist = diff.x * scale;
                maxDist = col.size.x;
                break;
            case AXIS.Y:
                dist = diff.y * scale;
                maxDist = col.size.y;
                break;
            case AXIS.Z:
                dist = diff.z * scale;
                maxDist = col.size.z;
                break;
            default:
                dist = 1;
                maxDist = 1;
                break;
        }

        return (dist + maxDist) / (2 * maxDist);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            float paramScale = GetDistance(other.transform.position);

            var Emitters = parameterTrigger.Emitters;
            for (int i = 0; i < Emitters.Length; i++)
            {
                var emitterRef = Emitters[i];
                if (emitterRef.Target != null && emitterRef.Target.EventInstance.isValid())
                {
                    for (int j = 0; j < Emitters[i].Params.Length; j++)
                    {
                        emitterRef.Target.EventInstance.setParameterByID(Emitters[i].Params[j].ID, Emitters[i].Params[j].Value * paramScale);
                    }
                }
            }
        }
    }
}
