using Models;
using Controller;
using VContainer;
using Controllers;
using VContainer.Unity;

namespace EntryPoint
{
    public class ProjectMain : IStartable
    {
        [Inject] private UiController UiController;
        [Inject] private PoolController PoolController;
        [Inject] private CameraMoveController CameraMoveController;
        [Inject] private RayCastController RayCastController;

        /// <summary>
        /// subscribe to the main event
        /// I do not unsubscribe from the event as the lifescope
        /// is on one scene.
        /// even if we have multi lifescope vcontainer will release
        /// dependancy when we switch scopes  
        /// </summary>
        public void Start()
        {
            PoolController.GenerateDeadPool();
            
            UiController.OnGenerateButtonClicked += ClickGenerateButton;
            UiController.OnClearButtonClicked += ClickClearButton;

            PoolController.OnTailLimitsHit += HitTailLimitsHit;
            PoolController.OnHeadLimitsHit += HitHeadLimitsHit;

            RayCastController.OnRayHitTargetLayer += RayHitTargetLayer;
        }

        private void RayHitTargetLayer(BoxView boxView)
        {
            new SortBoxView(boxView);
        }

        private void HitHeadLimitsHit(float position)
        {
            CameraMoveController.HeadLimit = position;
        }

        private void HitTailLimitsHit(float position)
        {
            CameraMoveController.TailLimit = position;
        }

        private void ClickClearButton()
        {
            PoolController.ClearPool();
            CameraMoveController.ResetCamera();
        }

        private void ClickGenerateButton(int maxBoxCount)
        {
            PoolController.MaxCount = maxBoxCount;
            PoolController.GenerateGroupsData();
            PoolController.GenerateInitialLifePool();
        }
    }
}