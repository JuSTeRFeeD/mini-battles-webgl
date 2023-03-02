using Managers;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class SkinsInstaller : MonoInstaller
    {
        [SerializeField] private Skins skins;
        
        public override void InstallBindings()
        {
            Container.Bind<Skins>().FromInstance(skins).AsSingle().NonLazy();
        }
    }
}