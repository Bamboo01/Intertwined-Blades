using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : EnemyStateBase<EnemyStates>
{
    public EnemyStates key => EnemyStates.DEAD;

    public EnemyController controller { get; set; }

    public void Init()
    {

    }

    public EnemyStates OnEnter()
    {
        SoundManager.Instance.PlaySoundAtPointByName(controller.enemyProperties.DeathSounds[Random.Range(0, controller.enemyProperties.DeathSounds.Count)], controller.transform.position);
        controller.SwapOnSwordCollisionEvent();
        controller.SwapOnEnemyCollisionEvent();
        controller.animator.SetTrigger("Death");
        controller.Invoke("Die", 4.0f);
        return key;
    }

    public void OnExit()
    {

    }

    public EnemyStates OnUpdate()
    {
        return key;
    }
}

