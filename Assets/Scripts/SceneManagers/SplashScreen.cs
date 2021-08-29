using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(prepareGame());
    }

    IEnumerator prepareGame()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("LoadingScene", LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        LoadHandler.Instance.ChangeScene("MainMenuScene");
        yield break;
    }
}
