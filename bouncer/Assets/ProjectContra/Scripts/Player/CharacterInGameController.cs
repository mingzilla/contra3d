using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Util3D;
using ProjectContra.Scripts.Bullet;
using ProjectContra.Scripts.Player.domain;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.Player
{
    public class CharacterInGameController : MonoBehaviour
    {
        private Rigidbody rb;
        private PlayerAttribute playerAttribute;
        private LayerMask groundLayers;
        private bool isFacingForward = true;

        public void Init(int playerId)
        {
            gameObject.tag = GameTag.CHARACTER_IN_GAME.name;
            gameObject.layer = GameLayer.PLAYER_SHOT.GetLayer();
            rb = UnityFn.AddRigidBodyAndFreezeZ(gameObject);
            playerAttribute = PlayerAttribute.CreateEmpty(playerId);
            groundLayers = GameLayer.GetGroundLayerMask();
        }

        public void HandleUpdate(UserInput userInput)
        {
            PlayerActionHandler3D.MoveX(userInput.fixedHorizontal, rb, playerAttribute.moveSpeed);
            isFacingForward = UserInput.GetFacingDirection(isFacingForward, userInput);

            bool isOnGround = StatusCheck.IsOnGround(transform.position, playerAttribute.playerToGroundDistance, groundLayers);
            if (userInput.jump) PlayerActionHandler3D.HandleJumpFromGround(isOnGround, rb, playerAttribute.jumpForce);
            PlayerActionHandler3D.HandleGravityModification(rb, playerAttribute.gravityMultiplier);
            
            if (userInput.fire1)
            {
                BulletController.Spawn(transform, isFacingForward, userInput, WeaponType.BLAST, isOnGround);
            }

        }
    }
}