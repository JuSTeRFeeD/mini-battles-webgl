using System.Collections;
using PlayerSave;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YandexProvider;

public class SplashScreenScene : MonoBehaviour
{
    [SerializeField] private Image progress;
    [SerializeField] private TextMeshProUGUI text;

    private void Start()
    {
        YandexSDK.CallbackLogging = true;
        StartCoroutine(nameof(LoadMainMenu));
    }

    private IEnumerator LoadMainMenu()
    {
        // #if DEBUG
        // SceneManager.LoadSceneAsync("MainMenu");
        // yield break;
        // #endif
        
        yield return new WaitForSeconds(0.1f);

        // YandexSDK initialization & loading player data
        var isDataLoaded = false;
        yield return YandexSDK.Initialize(() =>
        {
            SaveAndLoad.Load(() => isDataLoaded = true);    
        });

        while (!isDataLoaded)
            yield return null;
        
        var waiter = SceneManager.LoadSceneAsync("MainMenu");
        while (!waiter.isDone)
        {
            progress.fillAmount = waiter.progress;
            text.text = $"{progress.fillAmount * 100.0f}%";
            yield return null;
        }
        
        YandexSDK.MakeGameReady();
    }
}
