using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBanana : InteractableBase
{
    public override void OnLoadCompleteEvent()
    {
        Destroy(gameObject);
        Player.Instance.controller.IncreaseStamina(2.5f);
    }
}
