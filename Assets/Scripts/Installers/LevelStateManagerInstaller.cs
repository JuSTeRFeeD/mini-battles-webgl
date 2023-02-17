using Managers;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class LevelStateManagerInstaller : MonoInstaller
    {
        [SerializeField] private LevelStateManager levelStateManager;
        
        public override void InstallBindings()
        {
            Container.Bind<LevelStateManager>().FromInstance(levelStateManager).AsSingle().NonLazy();
        }
    }
}