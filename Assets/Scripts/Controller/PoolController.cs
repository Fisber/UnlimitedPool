using Enums;
using Utils;
using System;
using Models;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;


namespace Controller
{
    public class PoolController : MonoBehaviour
    {
        [SerializeField] private BoxView Instance;
        [SerializeField] private Transform DeadPool;
        [SerializeField] private Transform LifePool;
        [SerializeField] private Transform InitialPosition;
        [SerializeField] private Transform FollowObjectPosition;

        private List<BoxView> DeadPoolHolder = new List<BoxView>();
        private List<BoxView> LifePoolHolder = new List<BoxView>();


        private BoxView CurrentBoxView;
        private int MaxReachedIndex;
      

        public int MaxCount = 100;
        public event Action<float> OnHeadLimitsHit;
        public event Action<float> OnTailLimitsHit;

        private DiskManager DiskManager = new DiskManager();


  
        public bool DoesPoolIsEmpty()
        {
            return !LifePoolHolder.Any();
        }

        public void GenerateGroupsData()
        {
            DiskManager.GenerateGroups(MaxCount);
            DiskManager.PreloadData(1);
        }
       

        public float GetFollowObjectPosition()
        {
            return FollowObjectPosition.position.x;
        }

        public void ClearPool()
        {
            foreach (var boxView in LifePoolHolder)
            {
                boxView.transform.SetParent(DeadPool);
            }

            CurrentBoxView = null;
            MaxReachedIndex = 0;
            LifePoolHolder.Clear();
        }

        public void GenerateDeadPool()
        {
            //generate a deadPool
            for (int i = 0; i < Configuration.DEAD_POOL_MAX_SIZ; i++)
            {
                var currentInstance = Instantiate(Instance, DeadPool);
                DeadPoolHolder.Add(currentInstance);
            }
        }

        public void GenerateInitialLifePool()
        {
            var InitialBoxCount =
                Configuration.INITIAL_POOL_SIZE > MaxCount 
                    ? MaxCount
                    : Configuration.INITIAL_POOL_SIZE;
            
            for (; MaxReachedIndex < InitialBoxCount; MaxReachedIndex++)
            {
            
                LifePoolHolder.Add(DeadPoolHolder[0]);

                // check if pre is not null and set it next
                if (CurrentBoxView != null)
                {
                    CurrentBoxView.NextBoxView = LifePoolHolder.Last();
                }
                
                // boxView parameters 
                LifePoolHolder.Last().transform.SetParent(LifePool);
                LifePoolHolder.Last().PreviosBoxView = CurrentBoxView;
                LifePoolHolder.Last().CurrentIndex = MaxReachedIndex;

                // set current boxView 
                CurrentBoxView = LifePoolHolder.Last();
                // remove it from deadPool
                DeadPoolHolder.RemoveAt(0);

                CurrentBoxView.transform.SetAsLastSibling();
                
                LifePoolHolder.Last().InitialPosition = LifePoolHolder.Count > 1
                    ? LifePoolHolder.Last().PreviosBoxView.InitialPosition
                    : InitialPosition.transform.position;
                
                CurrentBoxView.UpdatePosition();

                if (Configuration.DISABLE_SAVE_TO_DISK == false)
                {
                    var group = DiskManager.FetchData(MaxReachedIndex);

                    CurrentBoxView.UpdateText(group[MaxReachedIndex]);
             
                }
                else
                {
                    CurrentBoxView.UpdateText(UnityEngine.Random.Range(0, 100));
                }

               
            }
        }

        public BoxView GetFirstLifeBoxView()
        {
            return LifePoolHolder?.First();
        }

        public BoxView GetLastRightBoxView()
        {
            return LifePoolHolder?.Last();
        }

        public void AddBoxToTheRight()
        {
            // add check for max index 
            CurrentBoxView = LifePoolHolder.Last();

            if (CurrentBoxView.CurrentIndex >= MaxCount-1)
            {
                OnTailLimitsHit?.Invoke(CurrentBoxView.transform.position.x);
                Debug.LogWarning($" upper bound error !");
                return;
            }
            
          

            LifePoolHolder.Add(DeadPoolHolder[0]);
            DeadPoolHolder.RemoveAt(0);

            // update links 
            LifePoolHolder.Last().PreviosBoxView = CurrentBoxView;
            LifePoolHolder.Last().NextBoxView = null;
            CurrentBoxView.NextBoxView = LifePoolHolder.Last();

            LifePoolHolder.Last().transform.SetParent(LifePool);
            LifePoolHolder.Last().transform.SetAsLastSibling();

            // !!!! maybe we have to update it out side this method 
            MaxReachedIndex = CurrentBoxView.CurrentIndex + 1;
            LifePoolHolder.Last().CurrentIndex = MaxReachedIndex;
            
            LifePoolHolder.Last().PreviosBoxView = CurrentBoxView;
            LifePoolHolder.Last().CurrentIndex = MaxReachedIndex;
            LifePoolHolder.Last().Direction = Direction.Right;
            CurrentBoxView = LifePoolHolder.Last();
            CurrentBoxView.InitialPosition = CurrentBoxView.PreviosBoxView.InitialPosition;
            CurrentBoxView.UpdatePosition();

            if (Configuration.DISABLE_SAVE_TO_DISK == false)
            {
                var data = DiskManager.FetchData(MaxReachedIndex);

                CurrentBoxView.UpdateText(data[MaxReachedIndex]);
            }
            else
            {
                CurrentBoxView.UpdateText(UnityEngine.Random.Range(0, 100));
            }
        }

        public void AddBoxToTheLeft()
        {
            // add check for min index 
            CurrentBoxView = LifePoolHolder.First();

            if (CurrentBoxView.CurrentIndex <= 0)
            {
                OnHeadLimitsHit?.Invoke(CurrentBoxView.transform.position.x);
                Debug.LogWarning(" lower bound error !");
                return;
            }

            LifePoolHolder.Insert(0, DeadPoolHolder[0]);
            DeadPoolHolder.RemoveAt(0);

            // update links 
            LifePoolHolder.First().PreviosBoxView = null;
            LifePoolHolder.First().NextBoxView = CurrentBoxView;
            CurrentBoxView.PreviosBoxView = LifePoolHolder.First();

            LifePoolHolder.First().transform.SetParent(LifePool);
            LifePoolHolder.First().transform.SetAsFirstSibling();

            // !!!! maybe we have to update it out side this method 
            MaxReachedIndex = CurrentBoxView.CurrentIndex - 1;
            LifePoolHolder.First().CurrentIndex = MaxReachedIndex;
            LifePoolHolder.First().Direction = Direction.Left;

            CurrentBoxView = LifePoolHolder.First();
            // start from the very first avaliable box 
            CurrentBoxView.InitialPosition.x = CurrentBoxView.NextBoxView.InitialPosition.x;
            CurrentBoxView.UpdatePosition();
            if (Configuration.DISABLE_SAVE_TO_DISK == false)
            {
                var data = DiskManager.FetchData(MaxReachedIndex);

                CurrentBoxView.UpdateText(data[MaxReachedIndex]);
            }
            else
            {
                CurrentBoxView.UpdateText(UnityEngine.Random.Range(0, 100));
            }
        }

        public void RemoveRight()
        {
            var previosBoxView = LifePoolHolder.Last().PreviosBoxView;
            LifePoolHolder.Last().transform.SetParent(DeadPool);
            DeadPoolHolder.Add(LifePoolHolder.Last());
            LifePoolHolder.RemoveAt(LifePoolHolder.Count - 1);
            previosBoxView.NextBoxView = null;
            previosBoxView.transform.SetAsLastSibling();
        }

        public void RemoveLift()
        {
            var nextBoxView = LifePoolHolder.First().NextBoxView;
            DeadPoolHolder.Add(LifePoolHolder.First());
            LifePoolHolder.First().transform.SetParent(DeadPool);
            LifePoolHolder.RemoveAt(0);
            nextBoxView.PreviosBoxView = null;
            nextBoxView.transform.SetAsFirstSibling();
        }
    }
}