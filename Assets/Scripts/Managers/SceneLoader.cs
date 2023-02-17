using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private Animator wipeAnimator;
        private static readonly int Active = Animator.StringToHash("active");

        private void Start()
        {
            #if DEBUG
            #else
            Application.targetFrameRate = 60;
            #endif
        }

        public void LoadScene(string sceneName)
        {
            StartCoroutine(nameof(LoadLevel), sceneName);
        }

        private IEnumerator LoadLevel(string sceneName)
        {
            wipeAnimator.SetBool(Active, true);
            yield return new WaitForSeconds(1.5f);
            yield return SceneManager.LoadSceneAsync(sceneName);
            wipeAnimator.SetBool(Active, false);
        }
    }
}
