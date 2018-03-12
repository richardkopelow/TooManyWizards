using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class AnimationVarAssignClip : PlayableAsset, ITimelineClipAsset
{
    public AnimationVarAssignBehaviour template = new AnimationVarAssignBehaviour ();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.None; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<AnimationVarAssignBehaviour>.Create (graph, template);
        AnimationVarAssignBehaviour clone = playable.GetBehaviour ();
        return playable;
    }
}
