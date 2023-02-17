using Managers;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class SceneLoaderInstaller : MonoInstaller
    {
        [SerializeField] private SceneLoader sceneLoader;
        
        public override void InstallBindings()
        {
            Container.Bind<SceneLoader>().FromInstance(sceneLoader).AsSingle().NonLazy();
        }
    }
}