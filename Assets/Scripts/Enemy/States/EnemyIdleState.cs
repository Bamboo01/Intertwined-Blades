using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyIdleState : EnemyStateBase<EnemyStates>
{
    public EnemyStates key => EnemyStates.IDLE;

    public EnemyController controller { get; set; }

    float idleDuration;
    float idleMaxDuration = 2.0f;
    float idleMinDuration = 1.0f;

    float resetRootDuration = 0.5f;

    int backlineTotalMod = 0;
    int engageTotalMod = 0;

    public void Init()
    {
        var prop = controller.enemyProperties;
        for (int i = 0; i < prop.backlineMods.Count; i++)
        {
            backlineTotalMod += prop.backlineMods[i].mod;
        }

        for (int i = 0; i < prop.engageMods.Count; i++)
        {
            engageTotalMod += prop.engageMods[i].mod;
        }
    }

    public EnemyStates OnEnter()
    {
        controller.SwapOnSwordCollisionEvent();
        controller.SwapOnEnemyCollisionEvent();
        idleDuration = Random.Range(idleMinDuration, idleMaxDuration);
        controller.animator.SetTrigger("Idle");
        controller.SwapOnEnemyCollisionEvent();

        controller.animator.transform.DOLocalMove(Vector3.zero, resetRootDuration).Play();
        controller.animator.transform.DOLocalRotate(Vector3.zero, resetRootDuration).Play();

        return key;
    }

    public void OnExit()
    {
        controller.animator.transform.DOKill();
        controller.animator.transform.localPosition = Vector3.zero;
        controller.animator.transform.localRotation = Quaternion.identity;
    }

    public EnemyStates OnUpdate()
    {
        idleDuration -= Time.deltaTime;

        if (idleDuration <= 0)
        {
            var prop = controller.enemyProperties;
            EnemyStates state = key;
            if (!controller.isInBackline)
            {
                int rolled = Random.Range(0, engageTotalMod);
                int accumulation = 0;
                for (int i = 0; i < prop.engageMods.Count; i++)
                {
                    accumulation += prop.engageMods[i].mod;
                    if (rolled < accumulation)
                    {
                        return prop.engageMods[i].state;
                    }
                }
            }
            else
            {
                int rolled = Random.Range(0, backlineTotalMod);
                int accumulation = 0;
                for (int i = 0; i < prop.backlineMods.Count; i++)
                {
                    accumulation += prop.backlineMods[i].mod;
                    if (rolled < accumulation)
                    {
                        return prop.backlineMods[i].state;
                    }
                }

            }
            return state;
        }
        else
        {
            return key;
        }
    }
}
