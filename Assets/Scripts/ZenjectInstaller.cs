using StarterAssets;
using Zenject;

public class ZenjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<ZenjectGetter>().AsSingle();
        Container.Bind<FirstPersonController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<GameManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<HUD>().FromComponentInHierarchy().AsSingle();
        Container.Bind<LevelCountdownController>().FromComponentInHierarchy().AsSingle();
    }
}

public class ZenjectGetter
{
    [Inject] public FirstPersonController FirstPersonController;

    [Inject] public GameManager GameManager;

    [Inject] public HUD HUD;
}

/*public static Getter Getto;
    public struct Getter
    {
        public FirstPersonController firstPersonController;
        public GameManager gameManager;
    }*/