using Managers;
using Zenject;

namespace Installers
{
    public class PlayersScoreInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var score = new PlayersScore();
            Container.Bind<PlayersScore>().FromInstance(score).AsSingle().NonLazy();
        }
    }
}