using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemySpawningState : EnemyStateBase<EnemyStates>
{
    public EnemyStates key => EnemyStates.SPAWNING;

    public EnemyController controller { get; set; }

    public void Init()
    {

    }

    public EnemyStates OnEnter()
    {
        controller.transform.position = new Vector3(Random.Range(-10, 10), 100, Random.Range(-10, 10));
        controller.isInBackline = true;
        controller.isTransitioning = true;

        RaycastHit hit;
        Vector3 pos = controller.transform.position;
        Vector3 landPosition = Vector3.zero;
        Vector3 startPosition = Vector3.zero;
        if (Physics.Raycast(pos, Vector3.down, out hit, Mathf.Infinity, controller.terrainMask))
        {
            landPosition = hit.point;
        }

        if (controller.enemyProperties.spawnFromSky)
        {
            controller.transform.position = landPosition;
            float r = !controller.isInBackline ? controller.enemyProperties.EngageRadius : controller.enemyProperties.RetreatRadius;
            controller.desiredPosition = (landPosition - controller.center.position).normalized * r + controller.center.position;
            startPosition = controller.desiredPosition;
            startPosition.y = 20.0f;
        controller.transform.position = startPosition;

            controller.animator.SetTrigger("Spawning");

            controller.transform.DOMoveY(landPosition.y, 2.0f).SetEase(Ease.InCubic).OnComplete(
                () =>
                {
                    controller.stateMachine.ChangeState(EnemyStates.IDLE);
                });
        }
        else
        {
            controller.transform.position = landPosition;
            float r = !controller.isInBackline ? controller.enemyProperties.EngageRadius : controller.enemyProperties.RetreatRadius;
            controller.desiredPosition = (landPosition - controller.center.position).normalized * r + controller.center.position;
            startPosition = controller.desiredPosition;
            controller.stateMachine.ChangeState(EnemyStates.IDLE);
        }

        return key;
    }

    public void OnExit()
    {
        controller.isTransitioning = false;
        SoundManager.Instance.PlaySoundAtPointByName("JumpEnd", controller.transform.position);
    }

    public EnemyStates OnUpdate()
    {
        // Look at player while falling
        controller.transform.LookAt(controller.center, Vector3.up);
        controller.transform.rotation = Quaternion.Euler(0, controller.transform.rotation.eulerAngles.y, 0);

        return key;
    }
}
