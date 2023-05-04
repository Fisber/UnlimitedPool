using Controller;
using Controllers;
using Models;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace EntryPoint
{
    public class ProjectLifeScope : LifetimeScope
    {
        [SerializeField] private PoolController PoolController;
        [SerializeField] private UiController UiController;
        [SerializeField] private RayCastController RayCastController;
        [SerializeField] private CameraMoveController CameraMoveController;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.UseEntryPoints(Lifetime.Singleton, entryPoints =>
            {
                entryPoints.Add<ProjectMain>();
                entryPoints.Add<PoolUtil>();
            });

            builder.RegisterComponent(PoolController);
            builder.RegisterComponent(UiController);
            builder.RegisterComponent(RayCastController);
            builder.RegisterComponent(CameraMoveController);
        }
    }
}