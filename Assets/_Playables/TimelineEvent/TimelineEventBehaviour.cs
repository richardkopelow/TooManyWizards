using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.Analytics;

[Serializable]
public class TimelineEventBehaviour : PlayableBehaviour
{
    public TimelineEventHolder Event;

    public override void OnPlayableCreate (Playable playable)
    {
        
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        base.OnBehaviourPlay(playable, info);
        Event.InvokeEvent();
    }
}
