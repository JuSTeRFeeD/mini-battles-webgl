using UnityEngine;
using Yandex;
using Zenject;

namespace Installers
{
    public class YandexSDKInstaller : MonoInstaller
    {
        [SerializeField] private YandexSDK yandexSDK;
        
        public override void InstallBindings()
        {
            Container.Bind<YandexSDK>().FromInstance(yandexSDK).AsSingle().NonLazy();
        }
    }
}
