using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableAcceptQuest : InteractableBase
{
    [SerializeField] string questName;
    void Start()
    {
        base.Start();
    }

    public override void OnLoadCompleteEvent()
    {
        InteractableDoor.scene = questName;
    }
}
