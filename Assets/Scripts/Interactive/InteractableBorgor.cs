using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBorgor : InteractableBase
{
    public override void OnLoadCompleteEvent()
    {
        Destroy(gameObject);
        Player.Instance.controller.IncreaseMaxHealth(1);
    }
}

