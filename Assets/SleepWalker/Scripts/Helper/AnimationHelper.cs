using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationHelper
{
    private const string ANIMATOR_SPEED_PARAMETER = "Speed";
    private const string ANIMATOR_DEAD_PARAMETER = "Dead";
    private const string ANIMATOR_HURT_PARAMETER = "Hurt";
    private const string ANIMATOR_FADE_OUT_PARAMETER = "FadeOut";
    private const string ANIMATOR_ACTIVATE_PARAMETER = "Activate";
    private const string ANIMATOR_REACTIVATE_PARAMETER = "Reactivate";
    private const string ANIMATOR_SLEEP_PARAMETER = "Sleeping";
    public static readonly int SpeedParameter = Animator.StringToHash(ANIMATOR_SPEED_PARAMETER);
    public static readonly int DeadParameter = Animator.StringToHash(ANIMATOR_DEAD_PARAMETER);
    public static readonly int HurtParameter = Animator.StringToHash(ANIMATOR_HURT_PARAMETER);
    public static readonly int FadeOutParameter = Animator.StringToHash(ANIMATOR_FADE_OUT_PARAMETER);
    public static readonly int ActivateParameter = Animator.StringToHash(ANIMATOR_ACTIVATE_PARAMETER);
    public static readonly int ReactivateParameter = Animator.StringToHash(ANIMATOR_REACTIVATE_PARAMETER);
    public static readonly int SleepParameter = Animator.StringToHash(ANIMATOR_SLEEP_PARAMETER);

}
