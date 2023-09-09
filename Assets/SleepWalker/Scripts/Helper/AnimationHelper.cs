using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationHelper
{
    private const string ANIMATOR_SPEED_PARAMETER = "Speed";
    private const string ANIMATOR_DEAD_PARAMETER = "Dead";
    private const string ANIMATOR_HURT_PARAMETER = "Hurt";
    private const string ANIMATOR_FADE_OUT_PARAMETER = "FadeOut";
    public static readonly int SpeedParameter = Animator.StringToHash(ANIMATOR_SPEED_PARAMETER);
    public static readonly int DeadParameter = Animator.StringToHash(ANIMATOR_DEAD_PARAMETER);
    public static readonly int HurtParameter = Animator.StringToHash(ANIMATOR_HURT_PARAMETER);
    public static readonly int FadeOutParameter = Animator.StringToHash(ANIMATOR_FADE_OUT_PARAMETER);

}
