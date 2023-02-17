using Managers;
using Zenject;

namespace Installers
{
    public class PlayersScoreInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<PlayersScore>().FromInstance(new PlayersScore()).AsSingle();
        }
    }
}