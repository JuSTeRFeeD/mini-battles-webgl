using Managers;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class RespawnManagerInstaller : MonoInstaller
    {
        [SerializeField] private RespawnManager respawnManager;
        
        public override void InstallBindings()
        {
            Container.Bind<RespawnManager>().FromInstance(respawnManager).AsSingle().NonLazy();
        }
    }
}