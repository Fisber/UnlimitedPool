using Utils;
using System;
using VContainer;
using Controller;
using VContainer.Unity;

namespace EntryPoint
{
    public class PoolUtil : IFixedTickable
    {
        [Inject] private PoolController PoolController;
        
        public void FixedTick()
        {

            if (PoolController.DoesPoolIsEmpty() )
            {
                return;
            }
            
            if (Math.Abs(PoolController.GetFollowObjectPosition() - PoolController.GetFirstLifeBoxView().transform.position.x) > Configuration.REMOVE_DISTANCE)
            {
                PoolController.RemoveLift();
            }

            if (Math.Abs(PoolController.GetFollowObjectPosition()  - PoolController.GetLastRightBoxView().transform.position.x) > Configuration.REMOVE_DISTANCE)
            {
                PoolController.RemoveRight();
            }

            if (Math.Abs(PoolController.GetFollowObjectPosition()  - PoolController.GetFirstLifeBoxView().transform.position.x) < Configuration.ADD_DISTANCE)
            {
                PoolController.AddBoxToTheLeft();
            }

            if (Math.Abs(PoolController.GetFollowObjectPosition()  - PoolController.GetLastRightBoxView().transform.position.x) < Configuration.ADD_DISTANCE)
            {
                PoolController.AddBoxToTheRight();
            }
        }
    }
}