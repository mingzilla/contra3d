using System;
using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Base.Domain;
using BaseUtil.GameUtil.Util3D;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.Bullet;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Player.Domain;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.Player
{
    public class CharacterInGameController : MonoBehaviour
    {
        private GameStoreData storeData;
        public Rigidbody rb;
        private SkinnedMeshRenderer meshRenderer;
        private LayerMask groundLayers;
        private GameObject destroyEffect;
        private int playerId;
        private bool isFacingRight = true;
        private Animator animatorCtrl;
        private static readonly int triggerJumpKey = Animator.StringToHash("triggerJump");
        private static readonly int triggerShootingKey = Animator.StringToHash("triggerShooting");
        private static readonly int triggerDeadKey = Animator.StringToHash("triggerDead");
        private static readonly int isDownKey = Animator.StringToHash("isDown");
        private static readonly int isMovingKey = Animator.StringToHash("isMoving");
        private readonly IntervalState pauseInterval = IntervalState.Create(0.1f);
        private readonly IntervalState takeDamageInterval = IntervalState.Create(1f);

        public CharacterInGameController Init(int id, bool isActive)
        {
            storeData = AppResource.instance.storeData;
            playerId = id;
            gameObject.layer = GameLayer.PLAYER.GetLayer();
            PlayerAttribute playerAttribute = storeData.GetPlayer(playerId);
            UnityFn.AddNoFrictionMaterialToCollider<CapsuleCollider>(gameObject);
            rb = UnityFn.AddRigidbody(gameObject, true, true);
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // for player only
            meshRenderer = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
            // meshRenderer.material = AppResource.instance.GetSkin(playerAttribute.skinId);
            groundLayers = GameLayer.GetGroundLayerMask();
            destroyEffect = AppResource.instance.playerDestroyedEffect;
            animatorCtrl = gameObject.GetComponent<Animator>();
            gameObject.SetActive(isActive);
            return this;
        }

        public void HandleUpdate(UserInput userInput)
        {
            if (gameObject.activeSelf && storeData.GetPlayer(playerId).isAlive) HandlePlayerControl(userInput);
        }

        private void HandlePlayerControl(UserInput userInput)
        {
            PlayerAttribute playerAttribute = storeData.GetPlayer(playerId);

            HandleInvincibilityUi();
            PlayerActionHandler3D.MoveX(userInput.fixedHorizontal, rb, playerAttribute.moveSpeed);
            animatorCtrl.SetBool(isMovingKey, UserInput.IsMoving(userInput));
            isFacingRight = UserInput.IsFacingRight(isFacingRight, userInput);
            UnitDisplayHandler3D.HandleLeftRightFacing(transform, isFacingRight);

            bool isOnGround = GameFn.IsOnGround(transform.position, playerAttribute.playerToGroundDistance, groundLayers);
            animatorCtrl.SetBool(isDownKey, userInput.down);
            if (userInput.jump && isOnGround) animatorCtrl.SetTrigger(triggerJumpKey);
            if (userInput.jump) PlayerActionHandler3D.HandleJumpFromGround(isOnGround, rb, playerAttribute.jumpForce);
            PlayerActionHandler3D.HandleGravityModification(rb, playerAttribute.gravityMultiplier);

            if (userInput.fire1) BulletController.Spawn(transform, isFacingRight, userInput, playerAttribute.weaponType, isOnGround);
            if (userInput.fire1) animatorCtrl.SetTrigger(triggerShootingKey);

            playerAttribute.inGameTransform = transform;
            storeData.SetPlayer(playerAttribute);
        }

        private void HandleInvincibilityUi()
        {
            bool isInvincible = !takeDamageInterval.canRun;
            if (isInvincible)
            {
                meshRenderer.enabled = !meshRenderer.enabled;
            }
            if (!isInvincible && !meshRenderer.enabled)
            {
                meshRenderer.enabled = true;
            }
        }

        public void TakeDamage(Vector3 position, int damage)
        {
            UnityFn.RunWithInterval(AppResource.instance, takeDamageInterval, () =>
            {
                PlayerAttribute playerAttribute = storeData.GetPlayer(playerId);
                playerAttribute.TakeDamage(damage, () =>
                {
                    animatorCtrl.SetTrigger(triggerDeadKey);
                    gameObject.SetActive(false);
                    UnityFn.SetTimeout(AppResource.instance, 1f, () =>
                    {
                        Destroy(gameObject);
                        storeData.ReloadScene();
                    });
                });
                storeData.SetPlayer(playerAttribute);
            });
        }

        public void PowerUp(WeaponType weaponType)
        {
            PlayerAttribute playerAttribute = storeData.GetPlayer(playerId);
            playerAttribute.weaponType = weaponType;
            storeData.SetPlayer(playerAttribute);
        }
    }
}