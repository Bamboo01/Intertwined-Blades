using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRetreatState : EnemyStateBase<EnemyStates>
{
    public EnemyStates key => EnemyStates.RETREAT;

    public EnemyController controller { get; set; }

    public void Init()
    {

    }

    public EnemyStates OnEnter()
    {
        controller.SwapOnSwordCollisionEvent();
        controller.SwapOnEnemyCollisionEvent();
        controller.animator.SetTrigger("JumpAway");
        controller.Retreat();
        SoundManager.Instance.PlaySoundAtPointByName("JumpStart", controller.transform.position);
        return key;
    }

    public void OnExit()
    {
        SoundManager.Instance.PlaySoundAtPointByName("JumpEnd", controller.transform.position);
    }

    public EnemyStates OnUpdate()
    {
        if (controller.isInBackline)
        {
            return EnemyStates.IDLE;
        }
        else
        {
            return key;
        }
    }
}
