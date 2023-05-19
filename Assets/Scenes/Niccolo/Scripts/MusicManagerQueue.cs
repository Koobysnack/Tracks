using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManagerQueue : MonoBehaviour
{
    public FMODUnity.StudioEventEmitter emitter = null;

    public FMODUnity.EmitterGameEvent trigger;
    public string collisionTag = "Player";
    public bool triggerOnce = true;

    // Start is called before the first frame update
    void Start()
    {
        if (emitter == null)
            emitter = GetComponent<FMODUnity.StudioEventEmitter>();

        if (trigger == FMODUnity.EmitterGameEvent.ObjectStart)
        {
            MusicManager.instance.SetMusicTrack(emitter);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (trigger == FMODUnity.EmitterGameEvent.TriggerEnter
            && other.CompareTag(collisionTag))
        {
            MusicManager.instance.SetMusicTrack(emitter);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (trigger == FMODUnity.EmitterGameEvent.TriggerExit
            && other.CompareTag(collisionTag))
        {
            MusicManager.instance.SetMusicTrack(emitter);
        }
    }
}
