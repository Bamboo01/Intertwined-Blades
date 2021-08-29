using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class InteractableBase : MonoBehaviour
{

    [SerializeField] Image LoadingCircle;
    [SerializeField] RectTransform DescriptionContainer;
    [SerializeField] RectTransform _Canvas;
    [SerializeField] float LoadDuration = 2.0f;
    private BambooOutline outline;
    [SerializeField] public bool hasClickInteraction = true;

    public float loadTimer;
    public bool isClicked;
    public bool isGazing;
    public bool isLoadFinishedEventCalled;

    private Coroutine countdownCoroutine = null;

    public void Awake()
    {
        outline = GetComponent<BambooOutline>();
    }

    public void Start()
    {
        loadTimer = 0.0f;
        isClicked = false;
        isGazing = false;
        isLoadFinishedEventCalled = false;

        outline.OutlineColor = Color.white;
        outline.enabled = true;

        LoadingCircle.fillAmount = loadTimer / LoadDuration;
        countdownCoroutine = null;

        Vector3 temp = _Canvas.localScale;
        temp.x = -temp.x;

        _Canvas.localScale = temp;
    }

    public void Update()
    {
        _Canvas.transform.LookAt(Camera.main.transform.position, Vector3.up);
    }

    public void OnClickStart()
    {
        isClicked = true;
        DescriptionContainer.gameObject.SetActive(false);
        if (isLoadFinishedEventCalled)
        {
            return;
        }

        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
        }
        LoadingCircle.gameObject.SetActive(true);
    }

    public void OnClickHold()
    {
        if (isLoadFinishedEventCalled)
        {
            return;
        }

        loadTimer += Time.deltaTime;
        LoadingCircle.fillAmount = loadTimer / LoadDuration;

        if (loadTimer >= LoadDuration)
        {
            ResetEverything();
            isLoadFinishedEventCalled = true;
            OnLoadCompleteEvent();
        }
    }

    public void OnClickRelease()
    {
        isClicked = false;
        if (countdownCoroutine == null)
        {
            DescriptionContainer.gameObject.SetActive(false);
            countdownCoroutine = StartCoroutine(waitBeforeCountdown());
        }
    }

    public virtual void OnGazeEnter(GameObject gazer)
    {
        isGazing = true;
        outline.OutlineColor = Color.blue;
        if (countdownCoroutine == null)
        {
            DescriptionContainer.gameObject.SetActive(true);
        }
    }

    public virtual void OnGazeStay(GameObject gazer)
    {

    }

    public virtual void OnGazeExit(GameObject gazer)
    {
        isGazing = false;
        outline.OutlineColor = Color.white;
        DescriptionContainer.gameObject.SetActive(false);
    }

    public virtual void OnLoadCompleteEvent()
    {
    }

    IEnumerator waitBeforeCountdown()
    {
        yield return new WaitForSeconds(2.0f);

        while (loadTimer >= 0)
        {
            loadTimer -= Time.deltaTime;
            LoadingCircle.fillAmount = loadTimer / LoadDuration;
            yield return null;
        }

        loadTimer = 0.0f;
        isClicked = false;
        isLoadFinishedEventCalled = false;
        countdownCoroutine = null;
        ResetEverything();

        if (isGazing)
        {
            DescriptionContainer.gameObject.SetActive(true);
        }
        yield break;
    }

    void ResetEverything()
    {
        loadTimer = 0.0f;
        isClicked = false;
        isLoadFinishedEventCalled = false;

        outline.OutlineColor = Color.white;
        outline.enabled = true;

        DescriptionContainer.gameObject.SetActive(false);
        LoadingCircle.gameObject.SetActive(false);
    }
}
