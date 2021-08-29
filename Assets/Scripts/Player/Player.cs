using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Bamboo.Utility;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Player : Singleton<Player>
{

    [Header("Player Addons")]
    [SerializeField] public PlayerInteracter interacter;
    [SerializeField] public PlayerPointer pointer;
    [SerializeField] public PlayerController controller;
    [SerializeField] public Canvas SubtitleCanvas;
    [SerializeField] public TMP_Text title;
    [SerializeField] public TMP_Text subtitle;

    public bool _isScreenTouched
    {
        get
        {
#if UNITY_EDITOR
            return Input.GetMouseButton(0);
#endif
            return Input.touchCount > 0;
        }
    }

    void Start()
    {
        controller._Start();
        pointer._Start();
        SceneManager.activeSceneChanged += OnSceneLoad;
    }

    void OnSceneLoad(Scene oldscene, Scene newscene)
    {
        if (newscene.name == "MainMenuScene")
        {
            pointer.enabled = true;
            controller.enabled = false;
        }

        if (newscene.name == "ForestFightScene")
        {
            pointer.enabled = false;
            controller.enabled = true;
        }
    }

    public void PlaySubtitles(string _title, string _msg, float duration = 1.0f, UnityAction action = null)
    {
        title.text = _title;
        subtitle.text = _msg;

        SubtitleCanvas.gameObject.SetActive(true);
        title.DOFade(0.0f, 0);
        subtitle.DOFade(0.0f, 0).OnComplete(() =>
        {
            title.DOFade(1.0f, 0.2f);
            subtitle.DOFade(1.0f, 0.2f).OnComplete(() =>
            {
                StartCoroutine(delayedFadeAway(duration, action));
            });
        });
    }

    IEnumerator delayedFadeAway(float d, UnityAction action)
    {
        yield return new WaitForSeconds(d);
        title.DOFade(0.0f, 0.2f);
        subtitle.DOFade(0.0f, 0.2f).OnComplete(() =>
        {
            action?.Invoke();
        });
    }

    [ContextMenu("Bruh")]
    public void Bruh()
    {
        PlaySubtitles("Test", "My cock, your ass, you do the math", 5.0f);
    }
}
