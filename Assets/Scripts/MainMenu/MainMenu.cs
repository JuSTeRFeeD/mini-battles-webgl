using Managers;
using UnityEngine;
using Zenject;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [Inject] private SceneLoader _sceneLoader;
        
        public void LoadLevel(string sceneName)
        {
            _sceneLoader.LoadScene(sceneName);
        }
    }
}
