using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    void Start()
    {
        Player.Instance.pointer.enabled = false;
        Player.Instance.controller.enabled = false;
        Player.Instance.interacter.enabled = false;
        Player.Instance.PlaySubtitles("Me", "Ow, I'm dead", 3.0f, Part1);
    }

    void Part1()
    {
        Player.Instance.PlaySubtitles("SYSTEM", "Sending you back to main menu...", 3.0f, () => { SoundManager.Instance.StopAllSounds(); LoadHandler.Instance.ChangeScene("MainMenuScene"); });
    }
}
