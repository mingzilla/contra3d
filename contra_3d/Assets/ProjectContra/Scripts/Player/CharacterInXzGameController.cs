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
    /// <summary>
    /// With X and Z direction movement
    /// </summary>
    public class CharacterInXzGameController : MonoBehaviour
    {
        private GameStoreData storeData;
        public Rigidbody rb;
        private SkinnedMeshRenderer meshRenderer;
        private LayerMask groundLayers;
        private int playerId;
        private Animator animatorCtrl;
        private static readonly int triggerJumpKey = Animator.StringToHash("triggerJump");
        private static readonly int triggerShootingKey = Animator.StringToHash("triggerShooting");
        private static readonly int triggerDeadKey = Animator.StringToHash("triggerDead");
        private static readonly int isOnGroundKey = Animator.StringToHash("isOnGround");
        private static readonly int isMovingKey = Animator.StringToHash("isMoving");
        private readonly IntervalState takeDamageInterval = IntervalState.Create(1f);

        public CharacterInXzGameController Init(int id, bool isActive)
        {
            storeData = AppResource.instance.storeData;
            playerId = id;
            gameObject.layer = GameLayer.PLAYER.GetLayer();
            PlayerAttribute playerAttribute = storeData.GetPlayer(playerId);
            UnityFn.AddNoFrictionMaterialToCollider<CapsuleCollider>(gameObject.GetComponentInChildren<CapsuleCollider>().gameObject);
            rb = UnityFn.GetOrAddInterpolateRigidbody(gameObject, true, false);
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // for player only
            meshRenderer = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
            Material skin = AppResource.instance.GetSkin(playerAttribute.skinId);
            meshRenderer.materials = UnityFn.UpdateMaterialAt(meshRenderer.materials, 1, skin);
            groundLayers = GameLayer.GetGroundLayerMask();
            animatorCtrl = gameObject.GetComponent<Animator>();
            gameObject.SetActive(isActive);
            return this;
        }

        public void HandleUpdate(UserInput userInput)
        {
            if (storeData.GetPlayer(playerId).isAlive) HandlePlayerControl(userInput);
        }

        private void HandlePlayerControl(UserInput userInput)
        {
            PlayerAttribute playerAttribute = storeData.GetPlayer(playerId);

            UnitDisplayHandler3D.HandleInvincibility(meshRenderer, takeDamageInterval);
            PlayerActionHandler3D.MoveXZ(userInput, rb, playerAttribute.moveSpeed);
            animatorCtrl.SetBool(isMovingKey, userInput.IsMoving());
            UnitDisplayHandler3D.HandleXZFacing(transform, userInput);

            bool isOnGround = UnityFn.IsOnGround(transform.position, playerAttribute.playerToGroundDistance, groundLayers);
            animatorCtrl.SetBool(isOnGroundKey, isOnGround);
            if (userInput.jump && isOnGround) animatorCtrl.SetTrigger(triggerJumpKey);
            if (userInput.jump) PlayerActionHandler3D.HandleJumpFromGround(isOnGround, rb, playerAttribute.jumpForce);
            PlayerActionHandler3D.HandleGravityModification(rb, playerAttribute.gravityMultiplier);

            if (userInput.fire1) SpawnBullets(playerAttribute.weaponType, userInput);
            if (userInput.fire1) animatorCtrl.SetTrigger(triggerShootingKey);

            playerAttribute.inGameTransform = transform;
            storeData.SetPlayer(playerAttribute);
        }

        private void SpawnBullets(WeaponType weaponType, UserInput userInput)
        {
            Vector3 positionDelta = weaponType.GetBulletPositionXzDelta(userInput.fixedHorizontal);
            BulletController.SpawnXZ(transform, positionDelta, weaponType);
        }

        public void TakeDamage(Vector3 position, int damage)
        {
            UnityFn.RunWithInterval(AppResource.instance, takeDamageInterval, () =>
            {
                PlayerAttribute playerAttribute = storeData.GetPlayer(playerId);
                playerAttribute.TakeDamage(damage, () =>
                {
                    AppSfx.Play(AppSfx.instance.playerDeath);
                    animatorCtrl.SetTrigger(triggerDeadKey);
                    UnityFn.RemoveForce(rb); // prevent player flying when e.g. has up force
                    UnityFn.SetTimeout(AppResource.instance, 2f, () =>
                    {
                        Destroy(gameObject);
                        if (storeData.AllPlayersDead())
                        {
                            AppMusic.instance.Stop();
                            storeData.ReloadScene();
                        }
                    });
                });
                storeData.SetPlayer(playerAttribute);
            });
        }

        public void PowerUp(WeaponType weaponType)
        {
            AppSfx.Play(AppSfx.instance.powerUpCollected);
            PlayerAttribute playerAttribute = storeData.GetPlayer(playerId);
            playerAttribute.weaponType = weaponType;
            storeData.SetPlayer(playerAttribute);
        }
    }
}