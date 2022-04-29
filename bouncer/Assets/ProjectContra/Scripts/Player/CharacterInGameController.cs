using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Util3D;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.Bullet;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Player.domain;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.Player
{
    public class CharacterInGameController : MonoBehaviour
    {
        public GameObject destroyEffect;

        private GameStoreData storeData;
        private Rigidbody rb;
        private LayerMask groundLayers;
        private bool isFacingRight = true;
        private int playerId;

        public void Init(int id)
        {
            storeData = AppResource.instance.storeData;
            playerId = id;
            gameObject.tag = GameTag.CHARACTER_IN_GAME.name;
            gameObject.layer = GameLayer.PLAYER.GetLayer();
            rb = UnityFn.AddRigidBodyAndFreezeZ(gameObject);
            groundLayers = GameLayer.GetGroundLayerMask();
            storeData.SetPlayer(PlayerAttribute.CreateEmpty(id));
        }

        public void HandleUpdate(UserInput userInput)
        {
            PlayerAttribute playerAttribute = storeData.GetPlayer(playerId);

            PlayerActionHandler3D.MoveX(userInput.fixedHorizontal, rb, playerAttribute.moveSpeed);
            isFacingRight = UserInput.IsFacingRight(isFacingRight, userInput);
            PlayerActionHandler3D.HandleLeftRightFacing(transform, isFacingRight);

            bool isOnGround = GameFn.IsOnGround(transform.position, playerAttribute.playerToGroundDistance, groundLayers);
            if (userInput.jump) PlayerActionHandler3D.HandleJumpFromGround(isOnGround, rb, playerAttribute.jumpForce);
            PlayerActionHandler3D.HandleGravityModification(rb, playerAttribute.gravityMultiplier);

            if (userInput.fire1)
            {
                BulletController.Spawn(transform, isFacingRight, userInput, WeaponType.BLAST, isOnGround);
            }

            playerAttribute.inGameTransform = transform;
            storeData.SetPlayer(playerAttribute);
        }

        public void TakeDamage(Vector3 position, int damage)
        {
            UnityFn.CreateEffect(destroyEffect, position, 1f);
            gameObject.SetActive(false);
        }
    }
}