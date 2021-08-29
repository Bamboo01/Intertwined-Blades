using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    [SerializeField] Animator animator;

    public enum AnimationList
    {
        Idle = 0,
        Stun,
        Stab,
        Uppercut,
        Death,
        Hit,
        StrafeLeft,
        StrafeRight,
        Overhead,
        JumpTowards,
        JumpAway
    }

    [SerializeField] AnimationList CurrentAnim
    {
        get
        {
            return CurrentAnim;
        }
        set
        {
            CurrentAnim = value;
            SwitchAnimation();
        }
    }

    private void SwitchAnimation()
    {
        animator.SetTrigger(CurrentAnim.ToString());
    }
}
