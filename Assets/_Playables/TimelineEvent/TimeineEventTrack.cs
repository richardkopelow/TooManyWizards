using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.Analytics;

[TrackColor(0.5073529f, 0.5073529f, 0.5073529f)]
[TrackClipType(typeof(TimelineEventClip))]
public class TimelineEventTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<TimelineEventMixerBehaviour>.Create (graph, inputCount);
    }
}
