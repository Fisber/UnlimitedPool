using System;
using Utils;
using UnityEngine;


namespace Controller
{
    public class CameraMoveController : MonoBehaviour
    {
        [SerializeField] private Transform Target;
        private float Speed;
        private Vector3 MoveDirection;

        public float HeadLimit = 0;
        public float TailLimit = float.MaxValue;

        private float TouchDistance;
        private bool MouseState;

        private bool LockHead;
        private bool LockTail;
        

        public void ResetCamera()
        {
            Target.position = new Vector3(0, 0, -10);
            LockHead = false;
            LockTail = false;
        }

        private void Update()
        {
            MoveTarget();
            SetDirection();
            CheckLimits();
        }

        private void MoveTarget()
        {
            if (TouchDistance > Configuration.STABILIZER_DISTANCE)
            {
                if (Input.GetMouseButton(0) == false && MouseState == false)
                {
                    Target.Translate(MoveDirection.normalized * Speed * Time.deltaTime / 10f);

                    DecreaseSpeed();
                }
            }
        }


        private void SetDirection()
        {
            if (Input.GetMouseButton(0) && MouseState == false)
            {
                MouseState = true;
                MoveDirection = Vector3.zero;
                Speed = Configuration.SPEED_MOVE;
                MoveDirection.x = Input.mousePosition.x; 

            }
            
            if (Input.GetMouseButtonUp(0) && MouseState )
            {
                MouseState = false;
               
                CLampSpeed(MoveDirection.x - Input.mousePosition.x);
              
                if (MoveDirection.x > Input.mousePosition.x)
                {
                    MoveDirection.x = -1;
                    LockHead = true;
                    LockTail = false;
                  
                }
                else
                {
                    LockHead = false;
                    LockTail = true;
                    MoveDirection.x = 1;
                
                }
            }
        }

        private void CheckLimits()
        {
           
            if (LockTail)
            {
                if (TailLimit+ Configuration.TAIL_THRESHOLD_CAMERAS_POSITION < Target.position.x  )
                {
                    Speed = 0;
                }
            }

            if (LockHead)
            {
                if (HeadLimit > Target.position.x)
                {
                    Speed = 0;
                }
            }
        }

        private void CLampSpeed(float distance)
        {
            TouchDistance = Math.Abs(distance);

            if (TouchDistance < Configuration.STABILIZER_DISTANCE)
            {
                Speed = 0;
            }
        }

        private void DecreaseSpeed()
        {
            Speed = Mathf.Lerp(Speed, 0, Time.deltaTime * Configuration.SWIPE_ELASTIC);
        }
    }
}