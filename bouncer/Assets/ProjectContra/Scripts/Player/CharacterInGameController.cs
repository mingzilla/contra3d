using BaseUtil.GameUtil;
using ProjectContra.Scripts.Player.domain;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.Player
{
    public class CharacterInGameController : MonoBehaviour
    {
        private Rigidbody rb;
        private PlayerAttribute playerAttribute;

        public void Init(int playerId)
        {
            gameObject.tag = GameTag.CHARACTER_IN_GAME.name;
            gameObject.layer = GameLayer.PLAYER.GetLayer();
            rb = gameObject.AddComponent<Rigidbody>();
            rb.constraints = (RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation);
            playerAttribute = PlayerAttribute.CreateEmpty(playerId);
        }

        public void HandleUpdate(UserInput userInput)
        {
            PlayerActionHandler3D.MoveX(userInput.fixedHorizontal, rb, playerAttribute.moveSpeed);
        }
    }
}