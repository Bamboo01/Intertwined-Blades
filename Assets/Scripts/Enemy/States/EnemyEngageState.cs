using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEngageState : EnemyStateBase<EnemyStates>
{
    public EnemyStates key => EnemyStates.ENGAGE;

    public EnemyController controller { get; set; }

    public void Init()
    {

    }

    public EnemyStates OnEnter()
    {
        controller.SwapOnSwordCollisionEvent();
        controller.SwapOnEnemyCollisionEvent();
        controller.animator.SetTrigger("JumpTowards");
        controller.Engage();
        SoundManager.Instance.PlaySoundAtPointByName("JumpStart", controller.transform.position);
        return key;
    }

    public void OnExit()
    {
        SoundManager.Instance.PlaySoundAtPointByName("JumpEnd", controller.transform.position);
    }

    public EnemyStates OnUpdate()
    {
        if (!controller.isInBackline)
        {
            return EnemyStates.IDLE;
        }
        else
        {
            return key;
        }
    }
}
