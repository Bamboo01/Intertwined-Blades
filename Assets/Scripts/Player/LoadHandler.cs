using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Bamboo.Utility;

public class LoadHandler : Singleton<LoadHandler>
{
    [SerializeField] Renderer cubeRoomRenderer;
    [SerializeField] GameObject LoadRoom;
    [SerializeField] GameObject percentageContainer;
    [SerializeField] TMP_Text percentageText;
    [SerializeField] float dimDuration = 1.5f;

    float dimTimer = 0.0f;
    float dimPercentage = 0.0f;
    string loadedScene;

    Scene temp;

    void Start()
    {
        LoadRoom.SetActive(false);
    }

    public void ChangeScene(string sceneName)
    {
        LoadRoom.SetActive(true);
        loadedScene = sceneName;
        temp = SceneManager.GetActiveScene();
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("LoadingScene"));
        StartCoroutine(_LoadScene());
    }

    IEnumerator _LoadScene()
    {
        percentageContainer.gameObject.SetActive(true);
        yield return StartCoroutine(StartDimming());
        yield return StartCoroutine(UnloadCurrentScene());
        yield return StartCoroutine(LoadScene(loadedScene));
        yield return StartCoroutine(StartGlowing());
    }

    IEnumerator StartDimming()
    {
        cubeRoomRenderer.gameObject.SetActive(true);
        percentageText.text = "0.00%";
        dimTimer = 0.0f;
        dimPercentage = 0.0f;
        while (dimPercentage <= 1.0f)
        {
            dimTimer += Time.deltaTime;
            dimPercentage += dimTimer / dimDuration;
            cubeRoomRenderer.material.SetFloat("_Opacity", dimPercentage);
            yield return null;
        }
        cubeRoomRenderer.material.SetFloat("_Opacity", 1.0f);
        yield break;
    }

    IEnumerator UnloadCurrentScene()
    {
        if (temp.name != "LoadingScene")
        {
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(temp);
            while (!asyncUnload.isDone)
            {
                yield return null;
            }
        }
        yield break;
    }

    IEnumerator LoadScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        asyncLoad.completed += (loaded) => { SceneManager.SetActiveScene(SceneManager.GetSceneByName(loadedScene)); };

        asyncLoad.allowSceneActivation = false;
        while (!asyncLoad.isDone)
        {
            percentageText.text = (asyncLoad.progress * 100.0f).ToString("0.00") + "%";
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
        percentageText.text = "100.00%";
        yield break;
    }



    IEnumerator StartGlowing()
    {
        percentageContainer.gameObject.SetActive(false);
        dimTimer = dimDuration;
        dimPercentage = 1.0f;
        while (dimPercentage >= 0.0f)
        {
            dimTimer -= Time.deltaTime;
            dimPercentage = dimTimer / dimDuration;
            cubeRoomRenderer.material.SetFloat("_Opacity", dimPercentage);
            yield return null;
        }
        cubeRoomRenderer.material.SetFloat("_Opacity", 0.0f);
        cubeRoomRenderer.gameObject.SetActive(false);
        LoadRoom.SetActive(false);
        yield break;
    }
}
