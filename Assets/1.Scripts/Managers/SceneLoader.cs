using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : SimpleSingleton<SceneLoader>
{
    protected override void Awake()
    {
        base.Awake();
        sceneLoaderCanvasGroup.alpha = 0;
        progressBar.fillAmount = 0;

        DontDestroyOnLoad(gameObject);
    }


    [SerializeField]

    private CanvasGroup sceneLoaderCanvasGroup;

    [SerializeField]

    private Image progressBar;



    private string loadSceneName;



    public static SceneLoader Create()

    {

        var SceneLoaderPrefab = Resources.Load<SceneLoader>("SceneLoader");

        return Instantiate(SceneLoaderPrefab);

    }

    
    public void LoadScene(string _sceneName)
    {
        gameObject.SetActive(true);

        SceneManager.sceneLoaded += LoadSceneEnd;

        loadSceneName = _sceneName;

        StartCoroutine(Load(_sceneName));


    }


    private IEnumerator Load(string sceneName)
    {
        progressBar.fillAmount = 0f;

        yield return StartCoroutine(Fade(true));

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);

        op.allowSceneActivation = false;



        float timer = 0.0f;

        while (!op.isDone)

        {

            yield return null;

            timer += Time.unscaledDeltaTime;



            if (op.progress < 0.9f)

            {

                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);

                if (progressBar.fillAmount >= op.progress)

                {

                    timer = 0f;

                }

            }

            else

            {

                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);



                if (progressBar.fillAmount == 1.0f)

                {

                    op.allowSceneActivation = true;

                    yield break;

                }

            }

        }

    }



    private void LoadSceneEnd(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(scene.name == loadSceneName)
        {
            StartCoroutine(Fade(false));

            SceneManager.sceneLoaded -= LoadSceneEnd;
        }
    }

    private IEnumerator Fade(bool isFadeIn)
    {
        float timer = 0f;
        while(timer <= 1f)
        {
            yield return null;

            timer += Time.unscaledDeltaTime * 2f;

            sceneLoaderCanvasGroup.alpha = Mathf.Lerp(isFadeIn ? 0 : 1, isFadeIn ? 1 : 0, timer);

        }

        if(!isFadeIn)
        {
            gameObject.SetActive(false);
        }

    }



}
