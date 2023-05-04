using System;
using UnityEngine;

namespace Controllers
{
    public class RayCastController : MonoBehaviour
    {
        [SerializeField] private Camera Camera;
        private int LayerMask;
        public event Action<BoxView> OnRayHitTargetLayer;

        private void Start()
        {
            SetUpRayCastLayer();
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                SendRay();
            }
        }

        private void SetUpRayCastLayer()
        {
            LayerMask = 1 << 8;
        }

        private void SendRay()
        {
            Ray ray = Camera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask) == false)
            {
                return;
            }

            var boxView = hit.transform.GetComponent<BoxView>();

            if (boxView == null)
            {
                return;
            }


            OnRayHitTargetLayer?.Invoke(boxView);
        }
    }
}