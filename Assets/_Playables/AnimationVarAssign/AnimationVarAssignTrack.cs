using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0.986207f, 0f, 1f)]
[TrackClipType(typeof(AnimationVarAssignClip))]
[TrackBindingType(typeof(Animator))]
public class AnimationVarAssignTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<AnimationVarAssignMixerBehaviour>.Create (graph, inputCount);
    }
}
