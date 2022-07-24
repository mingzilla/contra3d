using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.AbstractController;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using UnityEngine;

namespace ProjectContra.Scripts.BaseCommon
{
    public class LookAtPlayerController : AbstractRangeDetectionController
    {
        [SerializeField] private float detectionRange = 40f;

        private GameStoreData storeData;

        private void Start()
        {
            storeData = AppResource.instance.storeData;
        }

        void Update()
        {
            RunIfPlayerIsInRange(storeData, GetDetectionRange(), (closestPlayer) =>
            {
                transform.rotation = UnityFn.LookXZ(transform, closestPlayer);
            });
        }

        public override float GetDetectionRange()
        {
            return detectionRange;
        }
    }
}