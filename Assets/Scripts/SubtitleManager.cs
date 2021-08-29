using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using DG.Tweening;
using System.Text.RegularExpressions;
using Bamboo.Utility;

[System.Serializable]
class Subtitle
{
    [SerializeField] public Sprite Face;
    [SerializeField] public string Title;
    [SerializeField] public string Message;
    [SerializeField] public UnityAction Action;

    public Subtitle(Sprite face, string title, string message)
    {
        Face = face;
        Title = title;
        Message = message;
        Action = null;
    }

    public Subtitle(Sprite face, string title, string message, UnityAction action)
    {
        Face = face;
        Title = title;
        Message = message;
        Action = action;
    }
}


public class SubtitleManager : Singleton<SubtitleManager>
{
    [SerializeField] Queue<Subtitle> Subtitles = new Queue<Subtitle>();
    [SerializeField] RectTransform MessageBox;
    [SerializeField] Image Face;
    [SerializeField] TMP_Text Title;
    [SerializeField] TMP_Text Message;
    [SerializeField] bool isMessageBoxActive;
    private bool skipMessages;

    void Awake()
    {
        Title.text = "";
        Message.text = "";
        Color newcolor = Face.color;
        newcolor.a = 0;
        Face.color = newcolor;
        isMessageBoxActive = false;

        Vector3 newscale = MessageBox.localScale;
        newscale.x = 0;
        MessageBox.localScale = newscale;
        Face.rectTransform.localScale = newscale;
    }

    private void Update()
    {
        //too lazy
        if (Input.GetMouseButtonDown(0))
        {
            skipMessages = true;
        }
    }
    public void QueueSubtitle(Sprite face, string title, string message, UnityAction action)
    {
        Subtitles.Enqueue(new Subtitle(face, title, message, action));
        if (!isMessageBoxActive)
        {
            ActivateSubtitles(face);
        }
    }

    public void QueueSubtitle(Sprite face, string title, string message)
    {
        Subtitles.Enqueue(new Subtitle(face, title, message));
        if (!isMessageBoxActive)
        {
            ActivateSubtitles(face);
        }
    }

    void DeactivateSubtitles()
    {   
        Face.rectTransform.DOScaleX(0, 0.3f).Play().OnComplete(
        () =>
        {
            Title.text = "";
            Message.text = "";
            MessageBox.DOScaleX(0, 0.6f).OnComplete(
            () =>
                {
                    isMessageBoxActive = false;

                    Vector3 newscale = MessageBox.localScale;
                    newscale.x = 0;
                    MessageBox.localScale = newscale;
                    Face.rectTransform.localScale = newscale;
                    Color newcolor = Face.color;
                    newcolor.a = 0;
                    Face.color = newcolor;
                }
            ).Play();
        }).Play();
    }

    void ActivateSubtitles(Sprite face)
    {
        if (Subtitles.Count == 0)
        {
            return;
        }
        isMessageBoxActive = true;

        Color newcolor = Face.color;
        newcolor.a = 1;
        Face.color = newcolor;
        Face.sprite = face;

        MessageBox.DOScaleX(1, 0.6f).OnComplete(
            () =>
            {
                Face.rectTransform.DOScaleX(1, 0.3f).Play().OnComplete(
                () =>
                {
                    Face.sprite = Subtitles.Peek().Face;
                    StartCoroutine(DisplayMessage());
                }).Play();
            }
        ).Play();
    }
    
    IEnumerator DisplayMessage()
    {
        if (Subtitles.Count == 0)
        {
            yield break;
        }
        skipMessages = false;
        Subtitle newest = Subtitles.Dequeue();
        Title.text = "";
        Message.text = "";
        Title.text = newest.Title;

        if (Face.sprite != newest.Face)
        {
            Vector3 newscale = MessageBox.localScale;
            newscale.x = 0;
            Face.rectTransform.localScale = newscale;
            Face.rectTransform.DOScaleX(1, 0.3f).Play();
        }

        Face.sprite = newest.Face;

        for (int i = 0; i < newest.Message.Length; i++)
        {
            // Check if have command to stall
            if (newest.Message[i] == '(')
            {
                string next = newest.Message.Substring(i, 3);
                Debug.Log("Found command " + next);
                switch (next)
                {
                    case "(w:":
                        float d;
                        int length = TestWaitCommand(newest.Message, i, out d);
                        if (length < 0)
                        {
                            break;
                        }
                        else
                        {
                            newest.Message = newest.Message.Remove(i, length);
                            Debug.Log("Trimmed Subtitle " + newest.Message + " at index " + i + " of length " + length);
                        }
                        yield return new WaitForSeconds(d);
                        break;
                }
            }
            if (i < newest.Message.Length)
            {
                Message.text += newest.Message[i];
            }

            if (skipMessages)
            {
                //regex to remove wait commands
                Message.text = Regex.Replace(newest.Message, "(\\(.*\\))","");
                newest.Action?.Invoke();
                if (Subtitles.Count == 0)
                {
                    yield return new WaitForSeconds(1f);
                    DeactivateSubtitles();
                }
                else
                {
                    yield return new WaitForSeconds(2.5f);
                    StartCoroutine(DisplayMessage());
                }
                yield break;
            }
            yield return new WaitForSeconds(0.02f);
        }
        newest.Action?.Invoke();
        if (Subtitles.Count == 0)
        {
            yield return new WaitForSeconds(1f);
            DeactivateSubtitles();
        }
        else
        {
            yield return new WaitForSeconds(1.5f);
            StartCoroutine(DisplayMessage());
        }
        yield break;
    }
    public void ForceStopSubtitles()
    {
        StopCoroutine(DisplayMessage());
        Subtitles.Clear();
        DeactivateSubtitles();
    }

    #region Commands
    int TestWaitCommand(string message, int i, out float d)
    {
        int commandlength = -1;
        d = 0.05f; d = 0.0f;

        Debug.Log("Found wait command");
        int startindex = i + 3;
        int endindex = message.IndexOf(')', startindex);
        if (endindex > 0)
        {
            string duration = message.Substring(startindex, endindex - startindex);
            Debug.Log("Wait command processing, string is " + duration);
            if (float.TryParse(duration, out d))
            {
                Debug.Log("Wait command successful, time is " + d);
                commandlength = endindex - i + 1;
                if (d <= 0f)
                {
                    d = 0.05f;
                    commandlength = -1;
                }
            }
        }
        else
        {
            Debug.Log("Wait command fail");
        }

        return commandlength;
    }
    #endregion
}
