using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDoor : InteractableBase
{
    public static string scene;

    void Start()
    {
        base.Start();
        scene = "ForestFightScene_DayPart01";
    }
    public override void OnLoadCompleteEvent()
    {
        SoundManager.Instance.FadeOutSoundByName("MainMenuBGM", 2.5f);
        LoadHandler.Instance.ChangeScene(scene);
    }
}