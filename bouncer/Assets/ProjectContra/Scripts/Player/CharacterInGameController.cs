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

        private void Start()
        {
            gameObject.tag = GameTag.CHARACTER_IN_GAME.name;
            rb = gameObject.AddComponent<Rigidbody>();
            rb.freezeRotation = true;
        }

        public void Init(int playerId)
        {
            playerAttribute = PlayerAttribute.CreateEmpty(playerId);
        }

        public void HandleUpdate(UserInput userInput)
        {
            PlayerActionHandler3D.MoveX(userInput.fixedHorizontal, rb, playerAttribute.moveSpeed);
        }
    }
}