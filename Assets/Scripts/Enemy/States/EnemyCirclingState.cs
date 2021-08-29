using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCirclingState : EnemyStateBase<EnemyStates>
{
    public EnemyStates key => EnemyStates.CIRCLING;
    List<string> WalkingSounds = new List<string>() { "Footstep1", "Footstep2", "Footstep3" };
    public EnemyController controller { get; set; }

    float circlingDuration;
    float circlingMaxDuration = 8.0f;
    float circlingMMinDuration = 4.0f;
    bool circleLeft;

    Coroutine coroutine;

    float footstepSoundDuration;

    public void Init()
    {
        
    }

    public EnemyStates OnEnter()
    {
        controller.SwapOnSwordCollisionEvent();
        controller.animator.SetTrigger(circleLeft ? "StrafeLeft" : "StrafeRight");
        circlingDuration = Random.Range(circlingMMinDuration, circlingMaxDuration);
        circleLeft = Random.Range(0, 2) == 0;
        controller.SwapOnEnemyCollisionEvent(onCollision);
        footstepSoundDuration = controller.animator.GetCurrentAnimatorStateInfo(0).length;
        coroutine = controller.StartCoroutine(footsteps());
        return key;
    }

    public void OnExit()
    {
        controller.OnEnemyCollision = null;
        controller.StopCoroutine(coroutine);
    }

    public EnemyStates OnUpdate()
    {
        circlingDuration -= Time.deltaTime;
        if (circleLeft)
        {
            controller.StrafeLeft();
        }
        if (!circleLeft)
        {
            controller.StrafeRight();
        }

        if (circlingDuration <= 0)
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
        if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyToEnemy"))
        {
            circleLeft = !circleLeft;
            controller.animator.SetTrigger(circleLeft ? "StrafeLeft" : "StrafeRight");
            controller.StopEnemyCollisionResponse();
        }
    }

    IEnumerator footsteps()
    {
        while(true)
        {
            SoundManager.Instance.PlaySoundAtPointByName(WalkingSounds[Random.Range(0, WalkingSounds.Count)], controller.transform.position);
            yield return new WaitForSeconds(footstepSoundDuration * 0.5f);
        }
    }
}