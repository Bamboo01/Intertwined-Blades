using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Player.Instance.controller._Start();
        SoundManager.Instance.FadeInSoundByName("MainMenuBGM", 2.0f);
    }
}
