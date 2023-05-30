using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.InteropServices;

public class MusicManager : MonoBehaviour
{
    static public MusicManager instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
    }

    [SerializeField]
    private FMODUnity.StudioEventEmitter emitter = null;
    [SerializeField]
    private bool playOnStart = true;

    [SerializeField]
    private bool playMetronome = false;
    [SerializeField]
    private FMODUnity.StudioEventEmitter metronomeSound = null;

    [StructLayout(LayoutKind.Sequential)]
    class TimelineInfo
    {
        public bool newMarker = false;
        public FMOD.StringWrapper markerName = new FMOD.StringWrapper();
        public bool bpmSyncChange = false;
        public bool bpmSync = false;
        public bool newBeat = false;
        public FMOD.Studio.TIMELINE_BEAT_PROPERTIES beat;
    }

    TimelineInfo timelineInfo = null;
    GCHandle timelineHandle;

    FMOD.Studio.EVENT_CALLBACK timelineCallback;

    [SerializeField]
    private bool currentlySyncedToBeat = false;

    const string BPMSyncStartMarkerName = "BPMSync";
    const string BPMSyncStopMarkerName = "NOBPMSync";

    public class QueueItem
    {
        public FMODUnity.StudioEventEmitter emitter = null;
        public FMOD.Studio.EventInstance instance;
        public int beatType = -1;
    }

    private List<QueueItem> sfxQueue;

    // Start is called before the first frame update
    void Start()
    {
        if (emitter == null)
            emitter = GetComponent<FMODUnity.StudioEventEmitter>();

        if (playOnStart)
        {
            emitter.Play();
            SetMusicTrack(emitter);
        }

        sfxQueue = new List<QueueItem>();
    }

    public void SetMusicTrack(FMODUnity.StudioEventEmitter em)
    {
        emitter = em;

        timelineCallback = new FMOD.Studio.EVENT_CALLBACK(TimelineCallback);
        timelineInfo = new TimelineInfo();
        // Pin the class that will store the data modified during the callback
        timelineHandle = GCHandle.Alloc(timelineInfo, GCHandleType.Pinned);
        // Pass the object through the userdata of the instance
        emitter.EventInstance.setUserData(GCHandle.ToIntPtr(timelineHandle));
        emitter.EventInstance.setCallback(TimelineCallback, FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER | FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT);
    }

    public FMODUnity.StudioEventEmitter GetCurrentEmitter() { return emitter; }

    public void SetPaused(bool paused)
    {
        emitter.EventInstance.setPaused(paused);
    }

    private bool IsWholeNumber(float x)
    {
        return (x % 1) == 0;
    }
    private bool IsValidBeatType(int beatType, FMOD.Studio.TIMELINE_BEAT_PROPERTIES beat)
    {
        float ratio = (float)(beatType * (beat.beat - 1)) / (float)beat.timesignaturelower;
        return IsWholeNumber(ratio);
    }

    // Update is called once per frame
    void Update()
    {
        if (timelineInfo == null)
            return;

        if (timelineInfo.newMarker)
        {
            timelineInfo.newMarker = false;

            if (timelineInfo.bpmSyncChange)
            {
                timelineInfo.bpmSyncChange = false;

                if (timelineInfo.bpmSync != currentlySyncedToBeat)
                {
                    currentlySyncedToBeat = timelineInfo.bpmSync;

                    if (!currentlySyncedToBeat)
                    {
                        // immediately clean sfx queue
                        foreach (var item in sfxQueue)
                        {
                            if (item.emitter != null)
                                item.emitter.Play();
                            else
                            {
                                item.instance.start();
                                item.instance.release();
                            }
                        }

                        sfxQueue.Clear();
                    }
                }
            }
        }

        if (timelineInfo.newBeat)
        {
            timelineInfo.newBeat = false;

            if (playMetronome && metronomeSound != null)
                metronomeSound.Play();

            // might be better to use linked list or smarter removing method (creating new list at end) if this becomes performance issue
            if (sfxQueue.Count > 0)
            {
                for (int i = sfxQueue.Count - 1; i >= 0; --i)
                {
                    var item = sfxQueue[i];

                    if (item.beatType != -1 && !IsValidBeatType(item.beatType, timelineInfo.beat))
                        continue;

                    if (item.emitter != null)
                        item.emitter.Play();
                    else
                    {
                        item.instance.start();
                        item.instance.release();
                    }

                    sfxQueue.RemoveAt(i);
                }
            }
        }
    }

    [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
    static FMOD.RESULT TimelineCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type, System.IntPtr _event, System.IntPtr parameterPtr)
    {
        // retrieve pointer to event instance
        FMOD.Studio.EventInstance instance = new FMOD.Studio.EventInstance(_event);
        // Retrieve the user data
        System.IntPtr timelineInfoPtr;
        FMOD.RESULT result = instance.getUserData(out timelineInfoPtr);

        // checks
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogError("Timeline Callback error: " + result);
        }
        if (timelineInfoPtr == System.IntPtr.Zero)
            return FMOD.RESULT.OK;

        // Get the object to store beat and marker details
        GCHandle timelineHandle = GCHandle.FromIntPtr(timelineInfoPtr);
        TimelineInfo timelineInfo = (TimelineInfo)timelineHandle.Target;

        switch (type)
        {
            case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER:
            {
                var parameter = (FMOD.Studio.TIMELINE_MARKER_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_MARKER_PROPERTIES));
                timelineInfo.markerName = parameter.name;
                timelineInfo.newMarker = true;

                // bpm sync markers
                if (timelineInfo.markerName == BPMSyncStartMarkerName)
                {
                    timelineInfo.bpmSyncChange = true;
                    timelineInfo.bpmSync = true;
                }
                else if (timelineInfo.markerName == BPMSyncStopMarkerName)
                {
                    timelineInfo.bpmSyncChange = true;
                    timelineInfo.bpmSync = false;
                }

                break;
            }
            case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT:
            {
                var beat = (FMOD.Studio.TIMELINE_BEAT_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_BEAT_PROPERTIES));
                timelineInfo.beat = beat;
                timelineInfo.newBeat = true;
                break;
            }
        }
        return FMOD.RESULT.OK;
    }

    public void TriggerSFX(FMODUnity.StudioEventEmitter sfx, int beatType = -1)
    {
        if (currentlySyncedToBeat)
        {
            QueueItem queueItem = new QueueItem();
            queueItem.emitter = sfx;
            queueItem.beatType = beatType;
            sfxQueue.Add(queueItem);
        }
        else
            sfx.Play();
    }
    public void TriggerSFX(FMOD.Studio.EventInstance sfx, int beatType = -1)
    {
        if (currentlySyncedToBeat)
        {
            QueueItem queueItem = new QueueItem();
            queueItem.instance = sfx;
            queueItem.beatType = beatType;
            sfxQueue.Add(queueItem);
        }
        else
            sfx.start();
    }

    public void StartLERPParam(string paramName, float target, float duration)
    {
        StartLERPParam(emitter, paramName, target, duration);
    }
    public void StartLERPParam(FMODUnity.StudioEventEmitter emitter, string paramName, float target, float duration)
    {
        StartCoroutine(LERPEmitterParam(emitter, paramName, target, duration));
    }
    private IEnumerator LERPEmitterParam(FMODUnity.StudioEventEmitter emitter, string paramName, float target, float duration)
    {
        float start;
        emitter.EventInstance.getParameterByName(paramName, out start);

        float remaining = duration;
        while (remaining > 0)
        {
            remaining -= Time.unscaledDeltaTime;
            float value = start + ((target - start) * (1.0f - (remaining / duration)));
            emitter.EventInstance.setParameterByName(paramName, value);
            Debug.Log("remaining: " + remaining);
            yield return null;
        }

        emitter.EventInstance.setParameterByName(paramName, target);
    }
}
