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
        private bool isFacingForward = true;
        private int playerId;

        public void Init(int id)
        {
            storeData = AppResource.instance.storeData;
            storeData.SetPlayer(PlayerAttribute.CreateEmpty(id));
            playerId = id;
            gameObject.tag = GameTag.CHARACTER_IN_GAME.name;
            gameObject.layer = GameLayer.PLAYER.GetLayer();
            rb = UnityFn.AddRigidBodyAndFreezeZ(gameObject);
            groundLayers = GameLayer.GetGroundLayerMask();
        }

        public void HandleUpdate(UserInput userInput)
        {
            PlayerAttribute playerAttribute = storeData.GetPlayer(playerId);

            PlayerActionHandler3D.MoveX(userInput.fixedHorizontal, rb, playerAttribute.moveSpeed);
            isFacingForward = UserInput.GetFacingDirection(isFacingForward, userInput);

            bool isOnGround = GameFn.IsOnGround(transform.position, playerAttribute.playerToGroundDistance, groundLayers);
            if (userInput.jump) PlayerActionHandler3D.HandleJumpFromGround(isOnGround, rb, playerAttribute.jumpForce);
            PlayerActionHandler3D.HandleGravityModification(rb, playerAttribute.gravityMultiplier);

            if (userInput.fire1)
            {
                BulletController.Spawn(transform, isFacingForward, userInput, WeaponType.BLAST, isOnGround);
            }

            playerAttribute.inGamePosition = transform.position;
            storeData.SetPlayer(playerAttribute);
        }

        public void TakeDamage(Vector3 position, int damage)
        {
            UnityFn.CreateEffect(destroyEffect, position, 1f);
            gameObject.SetActive(false);
        }
    }
}