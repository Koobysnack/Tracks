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
    private bool playMetronome = false;
    [SerializeField]
    private FMODUnity.StudioEventEmitter metronomeSound = null;

    [StructLayout(LayoutKind.Sequential)]
    class TimelineInfo
    {
        public bool newMarker = false;
        public FMOD.StringWrapper markerName = new FMOD.StringWrapper();
        public bool newBeat = false;
        public FMOD.Studio.TIMELINE_BEAT_PROPERTIES beat;
    }

    TimelineInfo timelineInfo;
    GCHandle timelineHandle;

    FMOD.Studio.EVENT_CALLBACK timelineCallback;

    [SerializeField]
    private bool currentlySyncedToBeat = false;

    public class QueueItem
    {
        public FMODUnity.StudioEventEmitter emitter = null;
        public FMOD.Studio.EventInstance instance;
    }

    private List<QueueItem> sfxQueue;

    // Start is called before the first frame update
    void Start()
    {
        if (emitter == null)
            emitter = GetComponent<FMODUnity.StudioEventEmitter>();

        emitter.Play();

        timelineCallback = new FMOD.Studio.EVENT_CALLBACK(TimelineCallback);
        timelineInfo = new TimelineInfo();
        // Pin the class that will store the data modified during the callback
        timelineHandle = GCHandle.Alloc(timelineInfo, GCHandleType.Pinned);
        // Pass the object through the userdata of the instance
        emitter.EventInstance.setUserData(GCHandle.ToIntPtr(timelineHandle));
        emitter.EventInstance.setCallback(TimelineCallback, FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER | FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT);

        sfxQueue = new List<QueueItem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timelineInfo.newBeat)
        {
            timelineInfo.newBeat = false;

            if (playMetronome && metronomeSound != null)
                metronomeSound.Play();

            if (sfxQueue.Count > 0)
            {
                foreach (var item in sfxQueue)
                {
                    if (item.emitter != null)
                        item.emitter.Play();
                    else
                        item.instance.start();
                }

                sfxQueue.Clear();
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

    public void TriggerSFX(FMODUnity.StudioEventEmitter sfx)
    {
        if (currentlySyncedToBeat)
        {
            QueueItem queueItem = new QueueItem();
            queueItem.emitter = sfx;
            sfxQueue.Add(queueItem);
        }
        else
            sfx.Play();
    }
    public void TriggerSFX(FMOD.Studio.EventInstance sfx)
    {
        if (currentlySyncedToBeat)
        {
            QueueItem queueItem = new QueueItem();
            queueItem.instance = sfx;
            sfxQueue.Add(queueItem);
        }
        else
            sfx.start();
    }
}
