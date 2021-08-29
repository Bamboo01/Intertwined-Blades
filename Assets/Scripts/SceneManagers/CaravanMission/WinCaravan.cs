using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCaravan : BaseFightSceneManager
{
    void Start()
    {
        base.Start();
        Player.Instance.pointer.enabled = false;
        Player.Instance.controller.enabled = true;
        Player.Instance.interacter.enabled = false;
        Player.Instance.PlaySubtitles("Village Chief", "Thank you adventurer! You look like hell...what happened?", 10.0f, Part1);
    }

    void Part1()
    {
        Player.Instance.PlaySubtitles("Me", "Had a rough night, here's your caravan goods all in one piece", 6.0f, Part2);
    }

    void Part2()
    {
        Player.Instance.PlaySubtitles("Village Chief", "I can't thank you enough! This village owes you its thanks!", 5.0f, Part3);
    }

    void Part3()
    {
        Player.Instance.PlaySubtitles("Me", "No problem chief.", 3.0f, Part4);
    }

    void Part4()
    {
        Player.Instance.PlaySubtitles("SYSTEM", "Caravan mission complete! Sending you back to main menu...", 5.0f, ()=> { LoadHandler.Instance.ChangeScene("MainMenuScene"); });
    }
}
