using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreenScene : MonoBehaviour
{
    [SerializeField] private Image progress;
    [SerializeField] private TextMeshProUGUI text;

    private void Start()
    {
        StartCoroutine(nameof(LoadMainMenu));
    }

    private IEnumerator LoadMainMenu()
    {
        yield return new WaitForSeconds(0.1f);
        var waiter = SceneManager.LoadSceneAsync("MainMenu");
        while (!waiter.isDone)
        {
            progress.fillAmount = waiter.progress;
            text.text = $"{progress.fillAmount * 100.0f}%";
            yield return null;
        }
    }
}
