using StarterAssets;
using Zenject;

public class ZenjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<ZenjectGetter>().AsSingle();
        Container.Bind<FirstPersonController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<GameManager>().FromComponentInHierarchy().AsSingle();
    }
}

public class ZenjectGetter
{
    [Inject]
    public FirstPersonController FirstPersonController;

    [Inject]
    public GameManager GameManager;

}

/*public static Getter Getto;
    public struct Getter
    {
        public FirstPersonController firstPersonController;
        public GameManager gameManager;
    }*/