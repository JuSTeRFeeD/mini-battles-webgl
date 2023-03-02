using Managers;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class SettingsInstaller : MonoInstaller
    {
        [SerializeField] private SettingsManager settingsManager;
        
        public override void InstallBindings()
        {
            Container.Bind<SettingsManager>().FromInstance(settingsManager).AsSingle().NonLazy();
        }
    }
}