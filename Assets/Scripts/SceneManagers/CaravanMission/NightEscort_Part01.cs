using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightEscort_Part01 : BaseFightSceneManager
{
    void Start()
    {
        base.Start();
        Player.Instance.pointer.enabled = false;
        Player.Instance.controller.enabled = false;
        Player.Instance.interacter.enabled = false;
        ChooseOneInteractableBase.chooseOneAction = OnEat;
        Player.Instance.PlaySubtitles("Me", "Nighttime...", 2.0f, Part1);
    }

    void Part1()
    {
        Player.Instance.PlaySubtitles("Me", "Might have an ambush, guess I can only eat one of these two...", 6.0f,
        ()=>{
            Player.Instance.pointer.enabled = true;
            Player.Instance.controller.enabled = false;
            Player.Instance.interacter.enabled = true;
        });
    }

    void OnEat()
    {
        Player.Instance.PlaySubtitles("Me", "Alright, I'm stuffed. Let's continue!", 6.0f, () => { LoadHandler.Instance.ChangeScene("ForestFightScene_NightPart02"); });
    }
}
