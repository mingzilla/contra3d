using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
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

        public void Init(int playerId)
        {
            gameObject.tag = GameTag.CHARACTER_IN_GAME.name;
            gameObject.layer = GameLayer.PLAYER.GetLayer();
            rb = UnityFn.AddRigidBodyAndFreezeZ(gameObject);
            playerAttribute = PlayerAttribute.CreateEmpty(playerId);
            groundLayers = GameLayer.GetGroundLayerMask();
        }

        public void HandleUpdate(UserInput userInput)
        {
            PlayerActionHandler3D.MoveX(userInput.fixedHorizontal, rb, playerAttribute.moveSpeed);

            bool isOnGround = StatusCheck.IsOnGround(transform.position, playerAttribute.playerToGroundDistance, groundLayers);
            if (userInput.jump) PlayerActionHandler3D.HandleJumpFromGround(isOnGround, rb, playerAttribute.jumpForce);
        }
    }
}