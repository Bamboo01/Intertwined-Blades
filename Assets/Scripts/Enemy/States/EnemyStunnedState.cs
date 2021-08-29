using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyStunnedState : EnemyStateBase<EnemyStates>
{
    public EnemyStates key => EnemyStates.STUNNED;
    List<string> hitSounds = new List<string>() { "Hit1", "Hit2" };
    public EnemyController controller { get; set; }

    float stunDuration;
    float stunMaxDuration = 8.0f;
    float stunMinDuration = 4.0f;
    float resetRootDuration = 0.5f;

    public void Init()
    {

    }

    public EnemyStates OnEnter()
    {
        controller.SwapOnEnemyCollisionEvent(onCollision);
        controller.SwapOnSwordCollisionEvent();
        stunDuration = Random.Range(stunMinDuration, stunMaxDuration);
        controller.animator.SetTrigger("Stun");

        controller.animator.transform.DOLocalMove(Vector3.zero, resetRootDuration).Play();
        controller.animator.transform.DOLocalRotate(Vector3.zero, resetRootDuration).Play();

        controller.stunController.gameObject.SetActive(true);

        return key;
    }

    public void OnExit()
    {
        controller.stunController.gameObject.SetActive(false);
    }

    public EnemyStates OnUpdate()
    {
        stunDuration -= Time.deltaTime;

        if (stunDuration <= 0)
        {
            return EnemyStates.IDLE;
        }
        else
        {
            return key;
        }
    }

    public void onCollision(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerWeapon"))
        {
            controller.animator.transform.DOKill();
            controller.animator.transform.localPosition = Vector3.zero;
            controller.animator.transform.localRotation = Quaternion.identity;
            controller.animator.SetTrigger("Hit");
            stunDuration += 1.0f;

            Player.Instance.controller.SwordCollider.enabled = false;

            // Sword should have damage stats, for now let's hardcode it!
            for (int i = 0; i < 1; i++)
            {
                controller.healthContent.GetChild(controller.enemyProperties.startingHealth - controller.health).gameObject.SetActive(false);
                controller.health--;
            }

            SoundManager.Instance.PlaySoundAtPointByName(hitSounds[Random.Range(0, hitSounds.Count)], controller.transform.position);

            if (controller.health <= 0)
            {
                controller.stateMachine.ChangeState(EnemyStates.DEAD);
                return;
            }
        }
    }
}
