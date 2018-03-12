using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class AnimationVarAssignBehaviour : PlayableBehaviour
{
    public enum AnimationVarEnum
    {
        Bool,
        Float,
        Int,
        Trigger
    }

    public AnimationVarEnum VarType;
    public string VarName;
    public bool BoolVal;
    public float FloatVal;
    public int IntVal;

    public bool Done;

    public override void OnPlayableCreate (Playable playable)
    {
    }
}
