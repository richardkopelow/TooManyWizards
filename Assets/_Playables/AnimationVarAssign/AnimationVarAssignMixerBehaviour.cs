using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class AnimationVarAssignMixerBehaviour : PlayableBehaviour
{
    // NOTE: This function is called at runtime and edit time.  Keep that in mind when setting the values of properties.
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        Animator trackBinding = playerData as Animator;

        if (!trackBinding)
            return;

        int inputCount = playable.GetInputCount();

        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<AnimationVarAssignBehaviour> inputPlayable = (ScriptPlayable<AnimationVarAssignBehaviour>)playable.GetInput(i);
            AnimationVarAssignBehaviour input = inputPlayable.GetBehaviour();

            if (!input.Done && inputWeight > 0.5f)
            {
                input.Done = true;
                switch (input.VarType)
                {
                    case AnimationVarAssignBehaviour.AnimationVarEnum.Bool:
                        trackBinding.SetBool(input.VarName, input.BoolVal);
                        break;
                    case AnimationVarAssignBehaviour.AnimationVarEnum.Float:
                        trackBinding.SetFloat(input.VarName, input.FloatVal);
                        break;
                    case AnimationVarAssignBehaviour.AnimationVarEnum.Int:
                        trackBinding.SetInteger(input.VarName, input.IntVal);
                        break;
                    case AnimationVarAssignBehaviour.AnimationVarEnum.Trigger:
                        if (input.BoolVal)
                        {
                            trackBinding.SetTrigger(input.VarName);
                        }
                        else
                        {
                            trackBinding.ResetTrigger(input.VarName);
                        }
                        break;
                    default:
                        break;
                }
            }

        }
    }
}
