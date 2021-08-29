using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableNoClick : InteractableBase
{
    void Start()
    {
        base.Start();
        hasClickInteraction = false;
    }

    public override void OnLoadCompleteEvent()
    {
        LoadHandler.Instance.ChangeScene("ForestFightScene");
    }
}
