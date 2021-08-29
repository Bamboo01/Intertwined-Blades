using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayEscort_Part02 : BaseFightSceneManager
{
    void Start()
    {
        base.Start();
        Player.Instance.pointer.enabled = false;
        Player.Instance.controller.enabled = true;
        Player.Instance.interacter.enabled = false;
        Player.Instance.PlaySubtitles("Me", "I'm tired, let's take a break...", 5.0f, Part1);
    }

    void Part1()
    {
        Player.Instance.PlaySubtitles("Unknown voice", "Oi! Hold up!", 3.0f, Part2);
    }

    void Part2()
    {
        Player.Instance.PlaySubtitles("Me", "Oh no...", 3.0f, Part3);
    }

    void Part3()
    {
        Player.Instance.PlaySubtitles("Unknown voice", "We are the super elite bandit clan. We've heard you kill a bunch of our boys...", 7.0f, Part4);
    }

    void Part4()
    {
        Player.Instance.PlaySubtitles("Me", "Oh no, let's prepare to fight!", 5.0f, StartWave);
    }

    void StartWave()
    {
        SoundManager.Instance.ForceResetFades();
        SoundManager.Instance.FadeInSoundByName("NormalBattleBGM", 2.0f);
        RunWave("StartWave");
    }

    public void StartWaveClear()
    {
        Player.Instance.PlaySubtitles("Me", "Ha too easy!", 5.0f, Part5);
    }

    void Part5()
    {
        Player.Instance.PlaySubtitles("Unknown voice", "Alright, you asked for it...", 5.0f, StartHarderWave);
    }

    void StartHarderWave()
    {
        RunWave("HarderWave");
    }

    public void HarderWaveClear()
    {
        Player.Instance.PlaySubtitles("Unknown voice", "You're not bad, looks like I gotta face you...!", 5.0f, StartBossWave);
    }

    void StartBossWave()
    {
        RunWave("BossWave");
    }

    public void BossWaveClear()
    {
        Player.Instance.PlaySubtitles("Bandit Leader", "Argh... you bested me. But you'll get a taste of defeat soon...", 5.0f, Part6);
    }

    void Part6()
    {
        Player.Instance.PlaySubtitles("Me", "That looks like the rest of em, wonder what he meant?", 5.0f, () => { SoundManager.Instance.FadeOutSoundByName("NormalBattleBGM", 1.5f); LoadHandler.Instance.ChangeScene("ForestFightScene_NightPart01"); });
    }
}
