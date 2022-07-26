using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Base.Domain;
using ProjectContra.Scripts.AbstractController;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.BaseCommon
{
    public class ShooterAnimationController : AbstractRangeDetectionController
    {
        [SerializeField] private float triggerInterval = 3f;
        [SerializeField] private GameObject animationObject;
        [SerializeField] private float detectionRange = 30f;

        private IntervalState shotIntervalState;
        private EnemyBulletType bulletType;
        private GameStoreData storeData;
        private Animator animatorCtrl;
        private static readonly int isShootingKey = Animator.StringToHash("isShooting");

        void Start()
        {
            storeData = AppResource.instance.storeData;
            if (animationObject) animatorCtrl = animationObject.GetComponent<Animator>();
            shotIntervalState = IntervalState.Create(triggerInterval);
        }

        void Update()
        {
            RunIfPlayerIsInRange(storeData, GetDetectionRange(), (closestPlayer) =>
            {
                UnityFn.RunWithInterval(this, shotIntervalState, () =>
                {
                    if (!closestPlayer) return;
                    if (animatorCtrl) animatorCtrl.SetTrigger(isShootingKey);
                });
            });
        }

        public override float GetDetectionRange()
        {
            return detectionRange;
        }
    }
}