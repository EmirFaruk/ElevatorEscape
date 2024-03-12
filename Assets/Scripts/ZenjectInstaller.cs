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
    public FirstPersonController firstPersonController;

    [Inject]
    public GameManager gameManager;

    /*public static Getter Getto;
    public struct Getter
    {
        public FirstPersonController firstPersonController;
        public GameManager gameManager;
    }*/
}