using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseOneInteractableBanana : ChooseOneInteractableBase
{
    public override void OnLoadCompleteEvent()
    {
        Player.Instance.controller.IncreaseStamina(5.0f);
        base.OnLoadCompleteEvent();
    }
}
