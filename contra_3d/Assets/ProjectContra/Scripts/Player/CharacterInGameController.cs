using System;
using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Base.Domain;
using BaseUtil.GameUtil.Util3D;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.Bullet;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.GameDataScriptable;
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
        private static readonly int isProneKey = Animator.StringToHash("isProne");
        private static readonly int isOnGroundKey = Animator.StringToHash("isOnGround");
        private static readonly int isMovingKey = Animator.StringToHash("isMoving");
        private static readonly int isPointingUpKey = Animator.StringToHash("isPointingUp");
        private readonly IntervalState pauseInterval = IntervalState.Create(0.1f);
        private readonly IntervalState takeDamageInterval = IntervalState.Create(1f);

        public CharacterInGameController Init(int id, bool isActive)
        {
            storeData = AppResource.instance.storeData;
            playerId = id;
            gameObject.layer = GameLayer.PLAYER.GetLayer();
            PlayerAttribute playerAttribute = storeData.GetPlayer(playerId);
            UnityFn.AddNoFrictionMaterialToCollider<CapsuleCollider>(gameObject.GetComponentInChildren<CapsuleCollider>().gameObject);
            rb = UnityFn.AddRigidbody(gameObject, true, true);
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // for player only
            meshRenderer = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
            Material skin = AppResource.instance.GetSkin(playerAttribute.skinId);
            meshRenderer.materials = new[] {meshRenderer.materials[0], skin}; // setting to a position doesn't work, need to replace the whole array
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
            animatorCtrl.SetBool(isMovingKey, userInput.IsMovingHorizontally());
            isFacingRight = UserInput.IsFacingRight(isFacingRight, userInput);
            UnitDisplayHandler3D.HandleLeftRightFacing(transform, isFacingRight);

            bool isOnGround = GameFn.IsOnGround(transform.position, playerAttribute.playerToGroundDistance, groundLayers);
            animatorCtrl.SetBool(isOnGroundKey, isOnGround);
            animatorCtrl.SetBool(isPointingUpKey, userInput.IsStraightUp());
            bool isProne = (isOnGround && userInput.IsStraightDown());
            animatorCtrl.SetBool(isProneKey, isProne);
            if (userInput.jump && isOnGround) animatorCtrl.SetTrigger(triggerJumpKey);
            if (userInput.jump) PlayerActionHandler3D.HandleJumpFromGround(isOnGround, rb, playerAttribute.jumpForce);
            PlayerActionHandler3D.HandleGravityModification(rb, playerAttribute.gravityMultiplier);

            if (userInput.fire1) SpawnBullets(playerAttribute.weaponType, userInput, isProne, isOnGround);
            if (userInput.fire1) animatorCtrl.SetTrigger(triggerShootingKey);

            playerAttribute.inGameTransform = transform;
            storeData.SetPlayer(playerAttribute);
        }

        private void SpawnBullets(WeaponType weaponType, UserInput userInput, bool isProne, bool isOnGround)
        {
            Vector3 positionDelta = Vector3.zero;
            if ((weaponType == WeaponType.BASIC) || (weaponType == WeaponType.BLAST)) positionDelta = new Vector3(userInput.fixedHorizontal, (isProne ? 0.6f : 1f), 0f);
            if (weaponType == WeaponType.WIDE) positionDelta = new Vector3(userInput.fixedHorizontal, 1.2f, 0f);
            BulletController.Spawn(transform, positionDelta, isFacingRight, userInput, weaponType, isOnGround);
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
                    UnityFn.RemoveForce(rb); // prevent player flying when e.g. has up force
                    UnityFn.SetTimeout(AppResource.instance, 2f, () =>
                    {
                        Destroy(gameObject);
                        if (storeData.AllPlayersDead()) storeData.ReloadScene();
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