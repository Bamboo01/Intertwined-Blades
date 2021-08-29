using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayEscort_Part01 : BaseFightSceneManager
{
    void Start()
    {
        base.Start();
        Player.Instance.pointer.enabled = false;
        Player.Instance.controller.enabled = true;
        Player.Instance.interacter.enabled = false;
        Player.Instance.PlaySubtitles("Me", "Hmm, quiet day. I don't think anything could go wrong...", 5.0f, Part1);
    }

    void Part1()
    {
        Player.Instance.PlaySubtitles("Me", "Wait, what is that!", 3.0f, Part2);
    }

    void Part2()
    {
        Player.Instance.PlaySubtitles("Unknown voice", "I have come to rob you!", 3.0f, Part3);
    }

    void Part3()
    {
        SoundManager.Instance.ForceResetFades();
        SoundManager.Instance.FadeInSoundByName("NormalBattleBGM", 2.0f);
        RunWave("StartWave");
        Player.Instance.PlaySubtitles("Me", "No I will stop you! I must make sure I block his attack first, then when he is stunned by my awesome blocking skills, I will attack him!", 15.0f);
    }

    public void OnKillBandit()
    {
        Player.Instance.PlaySubtitles("Bandit", "Argh I am dead...my brothers will avenge me!", 5.0f, Part4);
    }

    void Part4()
    {
        Player.Instance.PlaySubtitles("Me", "What did he mean by that!", 5.0f, Part5);
    }

    void Part5()
    {
        Player.Instance.PlaySubtitles("Unknown voice 01", "Nooooo! My brother! I will avenge you!", 5.0f, Part6);
    }

    void Part6()
    {
        Player.Instance.PlaySubtitles("Unknown voice 02", "Our brother is dead?!?! I will avenge him!", 5.0f, Part7);
    }

    void Part7()
    {
        RunWave("RevengeWave");
        Player.Instance.PlaySubtitles("Me", "Oh no! This will be hard...", 5.0f);
    }

    public void OnKillBrothers()
    {
        Player.Instance.PlaySubtitles("Bandits 01 and 02", "Oh no you are too strong!", 5.0f, Part8);
    }

    void Part8()
    {
        Player.Instance.PlaySubtitles("Me", "That was easy! Time to carry on...", 3.0f, ()=> { SoundManager.Instance.FadeOutSoundByName("NormalFightBGM", 1.5f); LoadHandler.Instance.ChangeScene("ForestFightScene_DayPart02"); });
    }
}
