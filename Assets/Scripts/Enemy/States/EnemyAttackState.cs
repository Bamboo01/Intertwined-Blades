using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyStateBase<EnemyStates>
{
    public EnemyStates key => EnemyStates.ATTACK;

    public EnemyController controller { get; set; }

    float attackDuration;

    public void Init()
    {
        
    }

    public EnemyStates OnEnter()
    {
        controller.SwapOnSwordCollisionEvent((other) =>
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("PlayerShield"))
            {
                controller.stateMachine.ChangeState(EnemyStates.STUNNED);
                SoundManager.Instance.PlaySoundAtPointByName("Block" + Random.Range(1, 4).ToString(), other.transform.position);
                controller.weaponCollider.enabled = false;
            }
            else if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Debug.Log("Bonk");
                controller.stateMachine.ChangeState(EnemyStates.IDLE);
                controller.weaponCollider.enabled = false;
                SoundManager.Instance.PlaySoundAtPointByName("PlayerDamaged", other.transform.position);
                Player.Instance.controller.TakeDamage();
            }
        });
        controller.SwapOnEnemyCollisionEvent();
        controller.weaponCollider.enabled = true;
        controller.animator.SetTrigger(controller.enemyProperties.Attacks[Random.Range(0, controller.enemyProperties.Attacks.Count)]);
        attackDuration = controller.animator.GetCurrentAnimatorStateInfo(0).length - 0.1f;

        SoundManager.Instance.PlaySoundAtPointByName(controller.enemyProperties.AttackSounds[Random.Range(0, controller.enemyProperties.AttackSounds.Count)], controller.transform.position);

        return key;
    }

    public void OnExit()
    {
        controller.weaponCollider.enabled = false;
    }

    public EnemyStates OnUpdate()
    {
        attackDuration -= Time.deltaTime;
        if (attackDuration <= 0.0f)
        {
            return EnemyStates.IDLE;
        }
        else
        {
            return key;
        }
    }
}
