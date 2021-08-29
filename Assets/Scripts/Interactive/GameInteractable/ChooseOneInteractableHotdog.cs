using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseOneInteractableHotdog : ChooseOneInteractableBase
{
    public override void OnLoadCompleteEvent()
    {
        base.OnLoadCompleteEvent();
        Player.Instance.controller.HealHealth(1000);
    }
}
