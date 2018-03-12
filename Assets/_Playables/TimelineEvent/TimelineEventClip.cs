using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.Events;

[Serializable]
public class TimelineEventClip : PlayableAsset, ITimelineClipAsset
{
    public TimelineEventBehaviour template = new TimelineEventBehaviour ();
    public ExposedReference<TimelineEventHolder> Event;

    public ClipCaps clipCaps
    {
        get { return ClipCaps.None; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<TimelineEventBehaviour>.Create (graph, template);
        TimelineEventBehaviour clone = playable.GetBehaviour ();
        clone.Event = Event.Resolve (graph.GetResolver ());
        return playable;
    }
}
