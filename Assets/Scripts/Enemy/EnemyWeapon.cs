using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField] public EnemyController controller;

    void OnTriggerEnter(Collider other)
    {
        if(controller.OnWeaponCollision != null)
        {
            controller.OnWeaponCollision?.Invoke(other);
        }
    }
}
