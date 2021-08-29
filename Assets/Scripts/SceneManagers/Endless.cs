using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Endless : BaseFightSceneManager
{

    List<UnityAction> SayEvents = new List<UnityAction>();
    void Start()
    {
        base.Start();
        Player.Instance.pointer.enabled = false;
        Player.Instance.controller.enabled = true;
        Player.Instance.interacter.enabled = false;

        SayEvents.Add(SayStuff1);
        SayEvents.Add(SayStuff2);
        SayEvents.Add(SayStuff3);
        SoundManager.Instance.ForceResetFades();
        SoundManager.Instance.FadeInSoundByName("EndlessBattleBGM", 1.0f);

        Player.Instance.PlaySubtitles("Me", "You believe yourselves wolves? I'll show you a beast!", 3.0f, RunRandomWave);

    }

    public void RunRandomWave()
    {
        if (Random.Range(0, 2) == 0)
        {
            SayEvents[Random.Range(0, SayEvents.Count)].Invoke();
        }
        string n = fightWaves[Random.Range(0, fightWaves.Count)].WaveName;
        RunWave(n);
    }

    public void SayStuff1()
    {
        Player.Instance.PlaySubtitles("Me", "Who's next??? I can take you all day!", 3.0f);
    }

    public void SayStuff2()
    {
        Player.Instance.PlaySubtitles("Me", "That all you got? Come at me!", 3.0f);
    }

    public void SayStuff3()
    {
        Player.Instance.PlaySubtitles("Me", "I've seen schoolgirls scarier than you!", 3.0f);
    }
}
