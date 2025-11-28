using AlmostControl.DialogSystems;
using AlmostControl.Game;
using AlmostControl.InputSystem;
using MessagePipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace AlmostControl.Boot
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        [SerializeField] private DialogView _dialogView;
        [SerializeField] private GameObject[] _levels;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterMessagePipe();

            builder.Register<PlayerInputService>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.RegisterInstance(_levels);
            builder.Register<LevelsManager>(Lifetime.Singleton);
            builder.Register<DialogService>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            
            builder.RegisterInstance(_dialogView);
        }
    }
}
