using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightEscort_Part02 : BaseFightSceneManager
{
    void Start()
    {
        base.Start();
        Player.Instance.pointer.enabled = false;
        Player.Instance.controller.enabled = true;
        Player.Instance.interacter.enabled = false;
        Player.Instance.PlaySubtitles("Trumpet", "*Blares*", 3.0f, Part1);
    }

    void Part1()
    {
        Player.Instance.PlaySubtitles("Me", "That can't be good...", 3.0f, Part2);
    }

    void Part2()
    {
        Player.Instance.PlaySubtitles("Unknown Voice", "By the name of the unholy, the bandit clan shall smite thee!", 3.0f, Part3);
    }

    void Part3()
    {
        SoundManager.Instance.ForceResetFades();
        SoundManager.Instance.FadeInSoundByName("FinalBattleBGM", 2.0f);
        Player.Instance.PlaySubtitles("Cacophony of voices", "CHARGEEE!!!", 3.0f, ()=> { RunWave("StartWave"); });
    }

    public void ClearStartWave()
    {
        Player.Instance.PlaySubtitles("Me", "Here comes more imps...", 3.0f, () => { RunWave("MegaImpWave"); });
    }

    public void ClearMegaImpWave()
    {
        Player.Instance.PlaySubtitles("Me", "That all you got?", 3.0f, () => { RunWave("GigaImpWave"); });
    }

    public void ClearGigaImpWave()
    {
        RunWave("CombinedAssault");
    }

    public void ClearCombinedAssault()
    {
        RunWave("CombinedAssault2");
    }

    public void ClearCombinedAssault2()
    {
        Player.Instance.PlaySubtitles("Me", "Heh...barely broke a sweat", 3.0f, Part4);
    }

    void Part4() 
    {
        Player.Instance.PlaySubtitles("Unknown Voice", "One last charge boys...huzzah!", 3.0f, () => { RunWave("FinalAssault"); });
    }

    public void ClearFinalAssault()
    {
        Player.Instance.PlaySubtitles("Bandit Leader", "You really are a magnificent warrior, we concede...", 3.0f, Part5);
    }

    void Part5()
    {
        Player.Instance.PlaySubtitles("Me", "You too, you all fought well...", 3.0f, () => { SoundManager.Instance.FadeOutSoundByName("FinalBattleBGM", 1.5f); LoadHandler.Instance.ChangeScene("WinCaravan"); });
    }
}
